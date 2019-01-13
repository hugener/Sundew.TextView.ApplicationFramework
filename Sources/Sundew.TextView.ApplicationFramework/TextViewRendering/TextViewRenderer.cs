// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextViewRenderer.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Sundew.Base.Collections;
    using Sundew.Base.Computation;
    using Sundew.Base.Threading;
    using Sundew.Base.Threading.Jobs;
    using Sundew.TextView.ApplicationFramework.TextViewRendering.Internal;

    /// <inheritdoc />
    /// <summary>
    /// Renders <see cref="ITextView"/> on to a text based LCD display.
    /// </summary>
    /// <seealso cref="T:IDisposable" />
    public class TextViewRenderer : ITextViewRenderer
    {
        private readonly AsyncLock textViewLock = new AsyncLock();
        private readonly IRenderingContextFactory renderContextFactory;
        private readonly ITextViewRendererReporter textViewRendererReporter;
        private readonly ViewTimerCache viewTimerCache;
        private readonly AutoResetEventAsync renderInterruptEvent = new AutoResetEventAsync(false);
        private readonly ContinuousJob renderJob;
        private IInvalidaterChecker invalidater = new Invalidater.NullInvalidater();
        private IRenderingContext renderingContext;
        private ICharacterContext characterContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextViewRenderer" /> class.
        /// </summary>
        /// <param name="renderContextFactory">The render context factory.</param>
        /// <param name="timerFactory">The timer factory.</param>
        /// <param name="textViewRendererReporter">The textView renderer logger.</param>
        public TextViewRenderer(IRenderingContextFactory renderContextFactory, ITimerFactory timerFactory, ITextViewRendererReporter textViewRendererReporter)
        {
            this.renderContextFactory = renderContextFactory;
            this.textViewRendererReporter = textViewRendererReporter;
            this.viewTimerCache = new ViewTimerCache(timerFactory);
            this.renderJob = new ContinuousJob(this.RenderAsync, e => this.textViewRendererReporter?.OnRendererException(e));
            this.textViewRendererReporter?.SetSource(this);
        }

        /// <summary>
        /// Gets the current text view.
        /// </summary>
        /// <value>
        /// The current text view.
        /// </value>
        public ITextView CurrentTextView { get; private set; }

        /// <summary>
        /// Starts rendering.
        /// </summary>
        public void Start()
        {
            if (this.renderJob.IsRunning)
            {
                return;
            }

            using (this.textViewLock.Lock())
            {
                this.renderingContext = this.renderContextFactory.CreateRenderingContext();
                this.renderingContext.Clear();
                this.renderingContext.ForEach(renderInstruction => renderInstruction());
                this.characterContext = this.renderContextFactory.TryCreateCustomCharacterBuilder().Value;
            }

            this.renderJob.Start();
            this.renderInterruptEvent.Reset();
            this.textViewRendererReporter?.Started();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.renderJob.Dispose();
            this.viewTimerCache.Dispose();
            this.CurrentTextView = null;
            this.textViewRendererReporter?.Stopped();
        }

        /// <summary>
        /// Tries to set the textView.
        /// </summary>
        /// <param name="newTextView">The new textView.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <returns>
        ///   <c>true</c>, if the textView was set, otherwise <c>false</c>.
        /// </returns>
        public async Task<Result<ITextView, SetTextViewError>> TrySetViewAsync(
            ITextView newTextView,
            Action<ITextView> onNavigatingAction)
        {
            using (await this.textViewLock.LockAsync())
            {
                if (this.CurrentTextView != newTextView)
                {
                    this.renderInterruptEvent.Set();
                    var oldView = this.CurrentTextView;
                    if (oldView != null)
                    {
                        await oldView.OnClosingAsync();
                    }

                    onNavigatingAction?.Invoke(oldView);

                    this.CurrentTextView = newTextView;
                    this.viewTimerCache.Reset();
                    var newInvalidater = new Invalidater(this.viewTimerCache);
                    this.invalidater = newInvalidater;
                    if (newTextView != null)
                    {
                        await newTextView.OnShowingAsync(newInvalidater, this.characterContext);
                    }

                    this.textViewRendererReporter?.OnViewChanged(newTextView, oldView);
                    this.renderInterruptEvent.Reset();
                    return Result.Success(oldView);
                }

                return Result.Error(SetTextViewError.AlreadySet);
            }
        }

        private async Task RenderAsync(CancellationToken cancellationToken)
        {
            if (this.invalidater.IsRenderRequiredAndReset())
            {
                var currentTextView = await this.GetCurrentTextViewAsync(cancellationToken);
                if (currentTextView == null)
                {
                    return;
                }

                if (await this.renderInterruptEvent.WaitAsync(TimeSpan.Zero, cancellationToken))
                {
                    return;
                }

                currentTextView.Render(this.renderingContext);
                cancellationToken.ThrowIfCancellationRequested();

                foreach (var renderInstruction in this.renderingContext)
                {
                    renderInstruction();
                    if (await this.renderInterruptEvent.WaitAsync(TimeSpan.Zero, cancellationToken))
                    {
                        break;
                    }
                }

                this.renderingContext.Reset();
            }
        }

        private async Task<ITextView> GetCurrentTextViewAsync(CancellationToken cancellationToken)
        {
            using (var lockResult = await this.textViewLock.TryLockAsync(cancellationToken))
            {
                if (lockResult)
                {
                    return this.CurrentTextView;
                }

                return null;
            }
        }
    }
}
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
    public sealed class TextViewRenderer : ITextViewRenderer
    {
        private readonly AsyncLock textViewLock = new AsyncLock();
        private readonly IRenderingContextFactory renderContextFactory;
        private readonly ITextViewRendererReporter textViewRendererReporter;
        private readonly ViewTimerCache viewTimerCache;
        private readonly AutoResetEventAsync abortRenderingEvent = new AutoResetEventAsync(false);
        private readonly ManualResetEventAsync renderingEvent = new ManualResetEventAsync(false);
        private readonly ContinuousJob renderJob;
        private IInvalidaterChecker invalidater = new Invalidater(null, false);
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
            this.abortRenderingEvent.Reset();
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
            this.textViewRendererReporter?.WaitingForAccessToChangeView();
            using (await this.textViewLock.LockAsync())
            {
                var oldView = this.CurrentTextView;
                if (this.CurrentTextView != newTextView)
                {
                    this.abortRenderingEvent.Set();
                    this.textViewRendererReporter?.WaitingForRenderingAborted(oldView);
                    this.renderingEvent.Wait(Timeout.InfiniteTimeSpan);
                    if (oldView != null)
                    {
                        await oldView.OnClosingAsync();
                    }

                    onNavigatingAction?.Invoke(oldView);

                    this.CurrentTextView = newTextView;

                    this.viewTimerCache.Reset();
                    this.invalidater.Dispose();
                    this.invalidater = new Invalidater(this.viewTimerCache, true);
                    if (newTextView != null)
                    {
                        await newTextView.OnShowingAsync(this.invalidater, this.characterContext);
                    }

                    this.textViewRendererReporter?.OnViewChanged(newTextView, oldView);
                    return Result.Success(oldView);
                }

                return Result.Error(SetTextViewError.AlreadySet);
            }
        }

        private async Task RenderAsync(CancellationToken cancellationToken)
        {
            this.textViewRendererReporter?.WaitingForAccessToRenderView();
            this.renderingEvent.Set();
            var currentTextView = await this.GetCurrentTextViewAsync(cancellationToken).ConfigureAwait(true);
            if (currentTextView == null)
            {
                this.abortRenderingEvent.Wait(Timeout.InfiniteTimeSpan);
                return;
            }

            this.textViewRendererReporter?.AcquiredViewForRendering(currentTextView);
            while (true)
            {
                var abortRendering = await this.abortRenderingEvent.WaitAsync(TimeSpan.Zero, cancellationToken).ConfigureAwait(true);
                if (abortRendering)
                {
                    this.textViewRendererReporter?.AbortingRendering(currentTextView);
                    break;
                }

                this.textViewRendererReporter?.WaitingForViewToInvalidate(currentTextView);
                if (this.invalidater.WaitForInvalidatedAndReset())
                {
                    this.textViewRendererReporter?.OnRender(currentTextView);
                    currentTextView.Render(this.renderingContext);
                    cancellationToken.ThrowIfCancellationRequested();

                    foreach (var renderInstruction in this.renderingContext)
                    {
                        renderInstruction();
                        abortRendering = await this.abortRenderingEvent.WaitAsync(TimeSpan.Zero, cancellationToken).ConfigureAwait(true);
                        if (abortRendering)
                        {
                            break;
                        }
                    }

                    if (abortRendering)
                    {
                        this.textViewRendererReporter?.AbortingRendering(currentTextView);
                        break;
                    }

                    this.textViewRendererReporter?.OnRendered(currentTextView, this.renderingContext);
                    this.renderingContext.Reset();
                }
                else
                {
                    this.textViewRendererReporter?.AbortingRendering(currentTextView);
                    break;
                }
            }

            this.renderingEvent.Reset();
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
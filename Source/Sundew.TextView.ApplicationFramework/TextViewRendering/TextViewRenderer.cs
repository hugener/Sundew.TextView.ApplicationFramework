// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextViewRenderer.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Sundew.Base.Collections;
    using Sundew.Base.Computation;
    using Sundew.Base.Reporting;
    using Sundew.Base.Threading;
    using Sundew.Base.Threading.Jobs;
    using Sundew.TextView.ApplicationFramework.TextViewRendering.Internal;

    /// <inheritdoc />
    /// <summary>
    /// Renders <see cref="ITextView"/> on to a text based LCD display.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public sealed class TextViewRenderer : ITextViewRenderer
    {
        private static readonly ITextView EmptyTextView = new NullTextView();
        private readonly AsyncLock textViewLock = new AsyncLock();
        private readonly IRenderingContextFactory renderContextFactory;
        private readonly ITextViewRendererReporter? textViewRendererReporter;
        private readonly ViewTimerCache viewTimerCache;
        private readonly AutoResetEventAsync abortRenderingEvent = new AutoResetEventAsync(false);
        private readonly ManualResetEventAsync renderingEvent = new ManualResetEventAsync(false);
        private readonly ContinuousJob renderJob;
        private readonly TimeIntervalSynchronizer timeIntervalSynchronizer;
        private IInvalidaterChecker invalidater = new Invalidater.NullInvalidater();
        private IRenderingContext renderingContext = default!;
        private ICharacterContext? characterContext;

        /// <summary>Initializes a new instance of the <see cref="TextViewRenderer"/> class.</summary>
        /// <param name="renderContextFactory">The render context factory.</param>
        /// <param name="timerFactory">The timer factory.</param>
        /// <param name="refreshInterval">The refresh interval.</param>
        /// <param name="textViewRendererReporter">The textView renderer logger.</param>
        /// <param name="timeIntervalSynchronizerReporter">The time interval synchronizer reporter.</param>
        public TextViewRenderer(IRenderingContextFactory renderContextFactory, ITimerFactory timerFactory, TimeSpan refreshInterval, ITextViewRendererReporter? textViewRendererReporter = null, ITimeIntervalSynchronizerReporter? timeIntervalSynchronizerReporter = null)
        {
            this.renderContextFactory = renderContextFactory;
            this.textViewRendererReporter = textViewRendererReporter;
            this.timeIntervalSynchronizer = new TimeIntervalSynchronizer(timeIntervalSynchronizerReporter)
            {
                Interval = refreshInterval,
            };
            this.viewTimerCache = new ViewTimerCache(timerFactory);
            this.renderJob = new ContinuousJob(
                this.RenderAsync,
                (Exception e, ref bool handled) =>
                {
                    this.textViewRendererReporter?.OnRendererException(e);
                    var renderExceptionEventArgs = new RenderExceptionEventArgs(e, this.CurrentTextView);
                    this.RenderException?.Invoke(this, renderExceptionEventArgs);
                    this.renderingEvent.Reset();
                    handled = renderExceptionEventArgs.Continue;
                });
            this.textViewRendererReporter?.SetSource(this);
            this.CurrentTextView = EmptyTextView;
        }

        /// <summary>
        /// Occurs when [render exception].
        /// </summary>
        public event EventHandler<RenderExceptionEventArgs>? RenderException;

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
                this.renderingContext.Instructions.ForEach(renderInstruction => renderInstruction());
                this.characterContext = this.renderContextFactory.TryCreateCustomCharacterBuilder().Value;
            }

            this.renderJob.Start();
            this.abortRenderingEvent.Reset();
            this.textViewRendererReporter?.Started();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.textViewRendererReporter?.Stopping();
            this.renderJob.Dispose();
            this.viewTimerCache.Dispose();
            using (this.textViewLock.Lock())
            {
                this.CurrentTextView = EmptyTextView;
            }

            this.textViewLock.Dispose();
            this.invalidater.Dispose();
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
            Action<ITextView>? onNavigatingAction)
        {
            this.textViewRendererReporter?.WaitingForAccessToChangeViewTo(newTextView);
            using (await this.textViewLock.LockAsync().ConfigureAwait(true))
            {
                var oldView = this.CurrentTextView;
                if (this.CurrentTextView != newTextView)
                {
                    this.abortRenderingEvent.Set();
                    this.textViewRendererReporter?.WaitingForRenderingAborted(oldView);
                    this.renderingEvent.Wait(Timeout.InfiniteTimeSpan);
                    await oldView.OnClosingAsync().ConfigureAwait(true);

                    this.viewTimerCache.Reset();
                    this.invalidater?.Dispose();
                    onNavigatingAction?.Invoke(oldView);

                    this.CurrentTextView = newTextView;

                    this.invalidater = new Invalidater(this.viewTimerCache, true);
                    await newTextView.OnShowingAsync(this.invalidater, this.characterContext).ConfigureAwait(true);

                    this.textViewRendererReporter?.OnViewChanged(newTextView, oldView);
                    return Result.Success(oldView);
                }

                this.textViewRendererReporter?.ViewAlreadySet(newTextView);
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
                await this.abortRenderingEvent.WaitAsync(Timeout.InfiniteTimeSpan, CancellationToken.None).ConfigureAwait(true);
                this.renderingEvent.Reset();
                return;
            }

            this.textViewRendererReporter?.AcquiredViewForRendering(currentTextView);
            while (true)
            {
                if (await this.abortRenderingEvent.WaitAsync(TimeSpan.Zero, cancellationToken).ConfigureAwait(true))
                {
                    this.textViewRendererReporter?.AbortingRendering(currentTextView);
                    break;
                }

                this.textViewRendererReporter?.WaitingForViewToInvalidate(currentTextView);
                if (this.invalidater.WaitForInvalidatedAndReset())
                {
                    if (await this.timeIntervalSynchronizer.SynchronizeAsync(this.abortRenderingEvent, cancellationToken, true).ConfigureAwait(true))
                    {
                        this.textViewRendererReporter?.AbortingRendering(currentTextView);
                        break;
                    }

                    var actualRenderContext = this.renderingContext!;
                    this.textViewRendererReporter?.OnDraw(currentTextView);
                    currentTextView.OnDraw(actualRenderContext);
                    cancellationToken.ThrowIfCancellationRequested();

                    foreach (var renderInstruction in actualRenderContext.Instructions)
                    {
                        renderInstruction();
                    }

                    this.textViewRendererReporter?.OnRendered(currentTextView, actualRenderContext);
                    actualRenderContext.Reset();
                }
                else
                {
                    this.textViewRendererReporter?.AbortingRendering(currentTextView);
                    break;
                }
            }

            this.renderingEvent.Reset();
        }

        private async Task<ITextView?> GetCurrentTextViewAsync(CancellationToken cancellationToken)
        {
            using var lockResult = await this.textViewLock.TryLockAsync(cancellationToken).ConfigureAwait(true);
            if (lockResult)
            {
                return this.CurrentTextView;
            }

            return null;
        }

        /*
        private class RenderTaskScheduler : TaskScheduler, IDisposable
        {
            private readonly BlockingCollection<Task> tasks = new BlockingCollection<Task>();
            private readonly Thread thread;

            public RenderTaskScheduler()
            {
                this.thread = new Thread(() =>
                {
                    foreach (var task in this.tasks.GetConsumingEnumerable())
                    {
                        this.TryExecuteTask(task);
                    }
                })
                {
                    Name = "RenderThread",
                    Priority = ThreadPriority.AboveNormal,
                };
                this.thread.Start();
            }

            public void Dispose()
            {
                this.tasks.CompleteAdding();
                this.thread.Join();
                this.tasks.Dispose();
            }

            protected override IEnumerable<Task> GetScheduledTasks()
            {
                return this.tasks;
            }

            protected override void QueueTask(Task task)
            {
                if (!this.tasks.IsAddingCompleted)
                {
                    this.tasks.Add(task);
                }
            }

            protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            {
                return false;
            }
        }*/
    }
}
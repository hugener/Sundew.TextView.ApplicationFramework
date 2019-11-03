// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Application.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Sundew.Base.Disposal;
    using Sundew.Base.Threading;
    using Sundew.TextView.ApplicationFramework.DeviceInterface;
    using Sundew.TextView.ApplicationFramework.Input;
    using Sundew.TextView.ApplicationFramework.Navigation;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Represents an application.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "The run method blocks until application close, when this happens cancellationTokenSource is disposed.")]
    public sealed class Application : IApplication
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly List<IIdleMonitor> idleMonitors = new List<IIdleMonitor>();
        private readonly DisposingList<IDisposable> disposer = new DisposingList<IDisposable>();
        private InputManager inputManager = default!;

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        public Application()
        {
            Console.CancelKeyPress += this.OnConsoleCancelKeyPress;
        }

        /// <summary>
        /// Occurs when [exiting].
        /// </summary>
        public event EventHandler<ExitRequestEventArgs>? ExitRequest;

        /// <summary>
        /// Occurs when [exit].
        /// </summary>
        public event EventHandler<EventArgs>? Exiting;

        /// <summary>
        /// Gets the exit cancellation token.
        /// </summary>
        /// <value>
        /// The exit cancellation token.
        /// </value>
        public CancellationToken ExitCancellationToken => this.cancellationTokenSource.Token;

        /// <summary>
        /// Gets or sets the idle controller reporter.
        /// </summary>
        /// <value>
        /// The idle controller reporter.
        /// </value>
        public IIdleMonitorReporter? IdleMonitorReporter { get; set; }

        /// <summary>
        /// Gets or sets the input manager reporter.
        /// </summary>
        /// <value>
        /// The input manager reporter.
        /// </value>
        public IInputManagerReporter? InputManagerReporter { get; set; }

        /// <summary>
        /// Gets or sets the text view renderer reporter.
        /// </summary>
        /// <value>
        /// The text view renderer reporter.
        /// </value>
        public ITextViewRendererReporter? TextViewRendererReporter { get; set; }

        /// <summary>Gets or sets the time interval synchronizer reporter.</summary>
        /// <value>The time interval synchronizer reporter.</value>
        public ITimeIntervalSynchronizerReporter? TimeIntervalSynchronizerReporter { get; set; }

        /// <summary>
        /// Gets the input manager.
        /// </summary>
        /// <value>
        /// The input manager.
        /// </value>
        public IInputManager InputManager
        {
            get
            {
                this.inputManager = this.EnsureInputManager();
                return this.inputManager;
            }
        }

        /// <summary>
        /// Starts the rendering.
        /// </summary>
        /// <param name="textDisplayDevice">The text display device.</param>
        /// <returns>A <see cref="TextViewNavigator" />.</returns>
        public ITextViewNavigator StartRendering(ITextDisplayDevice textDisplayDevice)
        {
            return this.StartRendering(textDisplayDevice, TimeSpan.Zero);
        }

        /// <summary>
        /// Starts the rendering.
        /// </summary>
        /// <param name="textDisplayDevice">The text display device.</param>
        /// <param name="refreshInterval">The refresh interval.</param>
        /// <returns>A <see cref="TextViewNavigator" />.</returns>
        public ITextViewNavigator StartRendering(ITextDisplayDevice textDisplayDevice, TimeSpan refreshInterval)
        {
            var timerFactory = new TimerFactory();
            var textViewRendererFactory = this.disposer.Add(
                new TextViewRendererFactory(textDisplayDevice, timerFactory, this.TextViewRendererReporter, this.TimeIntervalSynchronizerReporter));
            this.disposer.Add(timerFactory);
            return this.StartRendering(textViewRendererFactory, refreshInterval);
        }

        /// <summary>Starts the rendering.</summary>
        /// <param name="textViewRendererFactory">The text view renderer factory.</param>
        /// <returns>A <see cref="TextViewNavigator"/>.</returns>
        public ITextViewNavigator StartRendering(ITextViewRendererFactory textViewRendererFactory)
        {
            return this.StartRendering(textViewRendererFactory, TimeSpan.Zero);
        }

        /// <summary>Starts the rendering.</summary>
        /// <param name="textViewRendererFactory">The text view renderer factory.</param>
        /// <param name="refreshInterval">The refresh interval.</param>
        /// <returns>A <see cref="TextViewNavigator"/>.</returns>
        public ITextViewNavigator StartRendering(ITextViewRendererFactory textViewRendererFactory, TimeSpan refreshInterval)
        {
            var textViewRenderer = textViewRendererFactory.Create(refreshInterval);
            textViewRenderer.Start();
            return this.disposer.Add(new TextViewNavigator(textViewRenderer, this.EnsureInputManager()));
        }

        /// <summary>
        /// Starts the idle monitoring.
        /// </summary>
        /// <param name="systemActivityAggregator">The activity aggregator.</param>
        /// <param name="inputIdleTimeSpan">The input idle time span.</param>
        /// <param name="systemIdleTimeSpan">The system idle time span.</param>
        /// <returns>An <see cref="IdleMonitor" />.</returns>
        public IIdleMonitor CreateIdleMonitoring(
            IActivityAggregator systemActivityAggregator,
            TimeSpan inputIdleTimeSpan,
            TimeSpan systemIdleTimeSpan)
        {
            return this.CreateIdleMonitoring(null, systemActivityAggregator, inputIdleTimeSpan, systemIdleTimeSpan);
        }

        /// <summary>
        /// Starts the idle monitoring.
        /// </summary>
        /// <param name="additionalInputAggregator">The input aggregator.</param>
        /// <param name="systemActivityAggregator">The activity aggregator.</param>
        /// <param name="inputIdleTimeSpan">The input idle time span.</param>
        /// <param name="systemIdleTimeSpan">The system idle time span.</param>
        /// <returns>An <see cref="IdleMonitor" />.</returns>
        public IIdleMonitor CreateIdleMonitoring(
            IActivityAggregator? additionalInputAggregator,
            IActivityAggregator systemActivityAggregator,
            TimeSpan inputIdleTimeSpan,
            TimeSpan systemIdleTimeSpan)
        {
            var idleMonitor = new IdleMonitor(this.EnsureInputManager(), additionalInputAggregator, systemActivityAggregator, inputIdleTimeSpan, systemIdleTimeSpan, this.IdleMonitorReporter);
            this.disposer.Add(idleMonitor);
            this.idleMonitors.Add(idleMonitor);
            return idleMonitor;
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            try
            {
                this.idleMonitors.ForEach(x => x.Start());
                Task.Delay(Timeout.InfiniteTimeSpan, this.cancellationTokenSource.Token).Wait(this.cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                Console.CancelKeyPress -= this.OnConsoleCancelKeyPress;
                this.cancellationTokenSource.Dispose();
                this.disposer.Dispose();
            }
        }

        /// <summary>
        /// Requests to exit the application.
        /// </summary>
        /// <returns>
        /// A value indicating whether the request was successful.
        /// </returns>
        public bool RequestExit()
        {
            var exitingEventArgs = new ExitRequestEventArgs();
            this.ExitRequest?.Invoke(this, exitingEventArgs);
            if (!exitingEventArgs.Cancel)
            {
                this.Exit();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        public void Exit()
        {
            this.Exiting?.Invoke(this, EventArgs.Empty);
            this.cancellationTokenSource.Cancel();
        }

        private InputManager EnsureInputManager()
        {
            return this.inputManager ?? new InputManager(this.InputManagerReporter);
        }

        private void OnConsoleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            this.RequestExit();
        }
    }
}
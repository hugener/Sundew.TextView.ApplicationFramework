// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Application.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework
{
    using System;
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
    public class Application : IApplication
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly DisposingList<IDisposable> disposer = new DisposingList<IDisposable>();
        private ITextViewRendererFactory textViewRendererFactory;
        private ITextViewRenderer textViewRenderer;
        private InputManager inputManager;
        private IdleMonitor idleMonitor;

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
        public event EventHandler<ExitRequestEventArgs> ExitRequest;

        /// <summary>
        /// Occurs when [exit].
        /// </summary>
        public event EventHandler<EventArgs> Exiting;

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
        public IIdleMonitorReporter IdleMonitorReporter { get; set; }

        /// <summary>
        /// Gets or sets the input manager reporter.
        /// </summary>
        /// <value>
        /// The input manager reporter.
        /// </value>
        public IInputManagerReporter InputManagerReporter { get; set; }

        /// <summary>
        /// Gets or sets the text view renderer reporter.
        /// </summary>
        /// <value>
        /// The text view renderer reporter.
        /// </value>
        public ITextViewRendererReporter TextViewRendererReporter { get; set; }

        /// <summary>
        /// Gets the input manager.
        /// </summary>
        /// <value>
        /// The input manager.
        /// </value>
        public IInputManager InputManager => this.EnsureInputManager();

        /// <summary>
        /// Starts the rendering.
        /// </summary>
        /// <param name="textDisplayDevice">The text display device.</param>
        /// <returns>A <see cref="TextViewNavigator" />.</returns>
        public ITextViewNavigator StartRendering(ITextDisplayDevice textDisplayDevice)
        {
            var timerFactory = this.disposer.Add(new TimerFactory());
            var textViewRendererFactory = this.disposer.Add(
                new TextViewRendererFactory(textDisplayDevice, timerFactory, this.TextViewRendererReporter));
            return this.StartRendering(textViewRendererFactory);
        }

        /// <summary>
        /// Starts the rendering.
        /// </summary>
        /// <param name="textViewRendererFactory">The text view renderer factory.</param>
        /// <returns>A <see cref="TextViewNavigator"/>.</returns>
        public ITextViewNavigator StartRendering(ITextViewRendererFactory textViewRendererFactory)
        {
            this.textViewRendererFactory = textViewRendererFactory;
            this.textViewRenderer = this.textViewRendererFactory.Create();
            this.textViewRenderer.Start();
            return new TextViewNavigator(this.textViewRenderer, this.EnsureInputManager());
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
            IActivityAggregator additionalInputAggregator,
            IActivityAggregator systemActivityAggregator,
            TimeSpan inputIdleTimeSpan,
            TimeSpan systemIdleTimeSpan)
        {
            this.idleMonitor = new IdleMonitor(this.EnsureInputManager(), additionalInputAggregator, systemActivityAggregator, inputIdleTimeSpan, systemIdleTimeSpan, this.IdleMonitorReporter);
            return this.idleMonitor;
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            try
            {
                this.idleMonitor?.Start();
                Task.Delay(Timeout.InfiniteTimeSpan, this.cancellationTokenSource.Token).Wait(this.cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                Console.CancelKeyPress -= this.OnConsoleCancelKeyPress;
                this.idleMonitor?.Dispose();
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

        private IInputManager EnsureInputManager()
        {
            return this.inputManager ?? (this.inputManager = new InputManager(this.InputManagerReporter));
        }

        private void OnConsoleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            this.RequestExit();
        }
    }
}
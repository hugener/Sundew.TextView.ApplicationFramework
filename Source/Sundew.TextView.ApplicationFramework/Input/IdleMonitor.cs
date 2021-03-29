// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdleMonitor.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.Input
{
    using System;
    using Sundew.Base.Reporting;
    using Sundew.Base.Threading;
    using Sundew.Base.Timers;

    /// <summary>
    /// Tracks whether the application receives input or has activity.
    /// </summary>
    public sealed class IdleMonitor : IIdleMonitor
    {
        private readonly IActivityAggregator? inputManagerAggregator;
        private readonly IActivityAggregator? additionalInputAggregator;
        private readonly IActivityAggregator activityAggregator;
        private readonly TimeSpan inputIdleTimeSpan;
        private readonly TimeSpan systemIdleTimeSpan;
        private readonly IIdleMonitorReporter? idleMonitorReporter;
        private readonly object lockObject = new();
        private readonly ITimer inputIdleTimer;
        private readonly ITimer systemIdleTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdleMonitor"/> class.
        /// </summary>
        /// <param name="inputManager">The input manager.</param>
        /// <param name="activityAggregator">The activity aggregator.</param>
        /// <param name="inputIdleTimeSpan">The input idle time span.</param>
        /// <param name="systemIdleTimeSpan">The system idle time span.</param>
        /// <param name="idleMonitorReporter">The idle controller reporter.</param>
        public IdleMonitor(
            IInputManager inputManager,
            IActivityAggregator activityAggregator,
            TimeSpan inputIdleTimeSpan,
            TimeSpan systemIdleTimeSpan,
            IIdleMonitorReporter idleMonitorReporter)
        : this(inputManager, null, activityAggregator, inputIdleTimeSpan, systemIdleTimeSpan, idleMonitorReporter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdleMonitor" /> class.
        /// </summary>
        /// <param name="inputAggregator">The input aggregator.</param>
        /// <param name="activityAggregator">The activity aggregator.</param>
        /// <param name="inputIdleTimeSpan">The input idle time span.</param>
        /// <param name="systemIdleTimeSpan">The system idle time span.</param>
        /// <param name="idleMonitorReporter">The idle controller reporter.</param>
        public IdleMonitor(
            IActivityAggregator inputAggregator,
            IActivityAggregator activityAggregator,
            TimeSpan inputIdleTimeSpan,
            TimeSpan systemIdleTimeSpan,
            IIdleMonitorReporter idleMonitorReporter)
            : this(null, inputAggregator, activityAggregator, inputIdleTimeSpan, systemIdleTimeSpan, idleMonitorReporter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdleMonitor" /> class.
        /// </summary>
        /// <param name="inputManagerAggregator">The input manager aggregator.</param>
        /// <param name="additionalInputAggregator">The additional input aggregator.</param>
        /// <param name="activityAggregator">The activity aggregator.</param>
        /// <param name="inputIdleTimeSpan">The input idle time span.</param>
        /// <param name="systemIdleTimeSpan">The system idle time span.</param>
        /// <param name="idleMonitorReporter">The idle controller observer.</param>
        public IdleMonitor(
            IActivityAggregator? inputManagerAggregator,
            IActivityAggregator? additionalInputAggregator,
            IActivityAggregator activityAggregator,
            TimeSpan inputIdleTimeSpan,
            TimeSpan systemIdleTimeSpan,
            IIdleMonitorReporter? idleMonitorReporter)
        {
            this.inputManagerAggregator = inputManagerAggregator;
            this.additionalInputAggregator = additionalInputAggregator;
            this.activityAggregator = activityAggregator;
            this.inputIdleTimeSpan = inputIdleTimeSpan;
            this.systemIdleTimeSpan = systemIdleTimeSpan;
            this.idleMonitorReporter = idleMonitorReporter;
            this.idleMonitorReporter?.SetSource(this);

            this.inputIdleTimer = new Timer();
            this.inputIdleTimer.Tick += this.OnInputIdle;
            this.systemIdleTimer = new Timer();
            this.systemIdleTimer.Tick += this.OnSystemIdle;
        }

        /// <summary>
        /// Occurs when the application has not received input for a given time.
        /// </summary>
        public event EventHandler? InputIdle;

        /// <summary>
        /// Occurs when the application has not had activity for a given time.
        /// </summary>
        public event EventHandler? SystemIdle;

        /// <summary>
        /// Occurs when the application received input after being idle.
        /// </summary>
        public event EventHandler<ActivatedEventArgs>? Activated;

        /// <summary>
        /// Gets a value indicating whether this instance is input idle.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is input idle; otherwise, <c>false</c>.
        /// </value>
        public bool IsInputIdle { get; private set; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            lock (this.lockObject)
            {
                this.inputIdleTimer.StartOnce(this.inputIdleTimeSpan);
                this.systemIdleTimer.StartOnce(this.systemIdleTimeSpan);
            }

            if (this.inputManagerAggregator != null)
            {
                this.inputManagerAggregator.ActivityOccured -= this.OnInputActivity;
                this.inputManagerAggregator.ActivityOccured += this.OnInputActivity;
            }

            if (this.additionalInputAggregator != null)
            {
                this.additionalInputAggregator.ActivityOccured -= this.OnInputActivity;
                this.additionalInputAggregator.ActivityOccured += this.OnInputActivity;
            }

            this.activityAggregator.ActivityOccured += this.OnSystemActivity;
            this.Activated?.Invoke(this, new ActivatedEventArgs(true));
            this.idleMonitorReporter?.Started();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (this.inputManagerAggregator != null)
            {
                this.inputManagerAggregator.ActivityOccured -= this.OnInputActivity;
            }

            if (this.additionalInputAggregator != null)
            {
                this.additionalInputAggregator.ActivityOccured -= this.OnInputActivity;
            }

            this.activityAggregator.ActivityOccured -= this.OnSystemActivity;

            this.inputIdleTimer.Dispose();
            this.systemIdleTimer.Dispose();
        }

        private void OnInputActivity(object sender, EventArgs eventArgs)
        {
            bool oldIsInputIdle;
            lock (this.lockObject)
            {
                oldIsInputIdle = this.IsInputIdle;
                this.IsInputIdle = false;
                this.systemIdleTimer.StartOnce(this.systemIdleTimeSpan);
                this.inputIdleTimer.StartOnce(this.inputIdleTimeSpan);
            }

            this.idleMonitorReporter?.OnInputActivity();
            if (oldIsInputIdle)
            {
                this.Activated?.Invoke(this, new ActivatedEventArgs(false));
                this.idleMonitorReporter?.Activated();
            }
        }

        private void OnSystemActivity(object sender, EventArgs eventArgs)
        {
            lock (this.lockObject)
            {
                this.systemIdleTimer.StartOnce(this.systemIdleTimeSpan);
            }

            this.idleMonitorReporter?.OnSystemActivity();
        }

        private void OnInputIdle(ITimer timer)
        {
            bool oldIsInputIdle;
            lock (this.lockObject)
            {
                this.inputIdleTimer.Stop();
                oldIsInputIdle = this.IsInputIdle;
                this.IsInputIdle = true;
            }

            if (!oldIsInputIdle)
            {
                this.idleMonitorReporter?.OnInputIdle();
                this.InputIdle?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnSystemIdle(ITimer timer)
        {
            lock (this.lockObject)
            {
                this.systemIdleTimer.Stop();
            }

            this.idleMonitorReporter?.OnSystemIdle();
            this.SystemIdle?.Invoke(this, EventArgs.Empty);
        }
    }
}
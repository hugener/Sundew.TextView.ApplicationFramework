// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeIntervalSynchronizer.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Sundew.Base.Threading;

    /// <summary>Implements time interval synchronization between calls.</summary>
    public class TimeIntervalSynchronizer
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly ITimeIntervalSynchronizerReporter? timeIntervalSynchronizerReporter;
        private TimeSpan interval;

        /// <summary>Initializes a new instance of the <see cref="TimeIntervalSynchronizer"/> class.</summary>
        /// <param name="timeIntervalSynchronizerReporter">The time interval synchronizer reporter.</param>
        public TimeIntervalSynchronizer(ITimeIntervalSynchronizerReporter? timeIntervalSynchronizerReporter)
        {
            this.timeIntervalSynchronizerReporter = timeIntervalSynchronizerReporter;
            this.timeIntervalSynchronizerReporter?.SetSource(this);
        }

        /// <summary>Gets or sets the interval.</summary>
        /// <value>The interval.</value>
        public TimeSpan Interval
        {
            get => this.interval;

            set
            {
                if (this.interval != value)
                {
                    this.interval = value;
                    this.timeIntervalSynchronizerReporter?.IntervalChanged(this.interval);
                }
            }
        }

        /// <summary>Synchronizes the time between calls.</summary>
        /// <param name="autoResetEventAsync">The auto reset event async.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A value task with a value indicating whether the operation was cancelled.</returns>
        public async Task<bool> SynchronizeAsync(
            AutoResetEventAsync autoResetEventAsync,
            CancellationToken cancellationToken)
        {
            var elapsed = this.stopwatch.Elapsed;
            var delay = this.interval - elapsed;
            if (delay > TimeSpan.Zero)
            {
                var wasAborted = await autoResetEventAsync.WaitAsync(delay, cancellationToken).ConfigureAwait(false);
                if (wasAborted)
                {
                    this.timeIntervalSynchronizerReporter?.WasAborted();
                    this.stopwatch.Restart();
                    return true;
                }

                var wasCancelled = cancellationToken.IsCancellationRequested;
                if (wasCancelled)
                {
                    this.timeIntervalSynchronizerReporter?.WasAborted();
                }
                else
                {
                    this.timeIntervalSynchronizerReporter?.DelayedBy(delay);
                }

                this.stopwatch.Restart();
                return wasCancelled;
            }

            this.timeIntervalSynchronizerReporter?.DelayNotNeeded(elapsed);
            this.stopwatch.Restart();
            return false;
        }
    }
}
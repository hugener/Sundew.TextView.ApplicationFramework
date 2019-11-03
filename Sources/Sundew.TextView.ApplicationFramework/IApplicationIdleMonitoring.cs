// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationIdleMonitoring.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework
{
    using System;
    using Sundew.TextView.ApplicationFramework.Input;

    /// <summary>
    /// Interface for implementing application idle monitoring.
    /// </summary>
    public interface IApplicationIdleMonitoring : IIApplicationInputManagement
    {
        /// <summary>
        /// Gets or sets the idle controller reporter.
        /// </summary>
        /// <value>
        /// The idle controller reporter.
        /// </value>
        IIdleMonitorReporter? IdleMonitorReporter { get; set; }

        /// <summary>
        /// Starts the input handling.
        /// </summary>
        /// <param name="systemActivityAggregator">The activity aggregator.</param>
        /// <param name="inputIdleTimeSpan">The input idle time span.</param>
        /// <param name="systemIdleTimeSpan">The system idle time span.</param>
        /// <returns>An <see cref="IdleMonitor" />.</returns>
        IIdleMonitor CreateIdleMonitoring(
            IActivityAggregator systemActivityAggregator,
            TimeSpan inputIdleTimeSpan,
            TimeSpan systemIdleTimeSpan);

        /// <summary>
        /// Starts the input handling.
        /// </summary>
        /// <param name="additionalInputAggregator">The input aggregator.</param>
        /// <param name="systemActivityAggregator">The activity aggregator.</param>
        /// <param name="inputIdleTimeSpan">The input idle time span.</param>
        /// <param name="systemIdleTimeSpan">The system idle time span.</param>
        /// <returns>An <see cref="IdleMonitor" />.</returns>
        IIdleMonitor CreateIdleMonitoring(
            IActivityAggregator? additionalInputAggregator,
            IActivityAggregator systemActivityAggregator,
            TimeSpan inputIdleTimeSpan,
            TimeSpan systemIdleTimeSpan);
    }
}
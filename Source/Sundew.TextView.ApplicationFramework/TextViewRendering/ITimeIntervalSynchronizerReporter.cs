// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITimeIntervalSynchronizerReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    using System;
    using Sundew.Base.Reporting;

    /// <summary>Interface for implementing a reporter for <see cref="TimeIntervalSynchronizer"/>.</summary>
    public interface ITimeIntervalSynchronizerReporter : IReporter
    {
        /// <summary>Reports that the interval was the changed.</summary>
        /// <param name="interval">The interval.</param>
        void IntervalChanged(TimeSpan interval);

        /// <summary>Reports that a delay occured.</summary>
        /// <param name="delay">The delay.</param>
        void DelayedBy(TimeSpan delay);

        /// <summary>Reports that a delay was not needed.</summary>
        /// <param name="elapsed">The elapsed.</param>
        void DelayNotNeeded(TimeSpan elapsed);

        /// <summary>Reports that the time interval synchronization was aborted.</summary>
        void WasAborted();
    }
}
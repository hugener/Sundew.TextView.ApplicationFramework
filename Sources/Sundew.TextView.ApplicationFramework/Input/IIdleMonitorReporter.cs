// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIdleMonitorReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.Input
{
    using Sundew.Base.Reporting;

    /// <summary>
    /// Interface for implementing an observer for the <see cref="IdleMonitor"/>.
    /// </summary>
    public interface IIdleMonitorReporter : IReporter
    {
        /// <summary>
        /// Called when the <see cref="IdleMonitor"/> is started.
        /// </summary>
        void Started();

        /// <summary>
        /// Called when the <see cref="IdleMonitor"/> receives input.
        /// </summary>
        void OnInputActivity();

        /// <summary>
        /// Called when the <see cref="IdleMonitor"/> is activated.
        /// </summary>
        void Activated();

        /// <summary>
        /// Called when the <see cref="IdleMonitor"/> receives system activity.
        /// </summary>
        void OnSystemActivity();

        /// <summary>
        /// Called when the <see cref="IdleMonitor"/> does not receive input after some time.
        /// </summary>
        void OnInputIdle();

        /// <summary>
        /// Called when the <see cref="IdleMonitor"/> does not receive activity after some time.
        /// </summary>
        void OnSystemIdle();
    }
}
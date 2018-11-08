// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIdleMonitor.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.Input
{
    using System;

    /// <summary>
    /// Interface for implementing an idle monitor.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IIdleMonitor : IDisposable
    {
        /// <summary>
        /// Occurs when the application has not received input for a given time.
        /// </summary>
        event EventHandler InputIdle;

        /// <summary>
        /// Occurs when the application has not had activity for a given time.
        /// </summary>
        event EventHandler SystemIdle;

        /// <summary>
        /// Occurs when the application received input after being idle.
        /// </summary>
        event EventHandler<ActivatedEventArgs> Activated;

        /// <summary>
        /// Gets a value indicating whether this instance is input idle.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is input idle; otherwise, <c>false</c>.
        /// </value>
        bool IsInputIdle { get; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start();
    }
}
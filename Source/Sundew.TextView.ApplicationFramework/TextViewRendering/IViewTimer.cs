// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewTimer.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    using System;

    /// <summary>
    /// A timer to be used in implemtations of <see cref="ITextView"/> for animation.
    /// </summary>
    public interface IViewTimer
    {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        event EventHandler Tick;

        /// <summary>
        /// Gets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the interval.
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
        TimeSpan Interval { get; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <param name="startDelay">The delay before the first occurence.</param>
        void StartOnce(TimeSpan startDelay);

        /// <summary>
        /// Starts the specified interval.
        /// </summary>
        /// <param name="interval">The interval.</param>
        void Start(TimeSpan interval);

        /// <summary>
        /// Starts the specified start delay.
        /// </summary>
        /// <param name="startDelay">The start delay.</param>
        /// <param name="interval">The interval.</param>
        void Start(TimeSpan startDelay, TimeSpan interval);

        /// <summary>
        /// Stops this instance.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "It's an application framework.")]
        void Stop();
    }
}
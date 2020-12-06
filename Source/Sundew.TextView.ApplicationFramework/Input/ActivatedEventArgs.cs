// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActivatedEventArgs.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.Input
{
    using System;

    /// <summary>
    /// Event args for the <see cref="IdleMonitor"/> Activated event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ActivatedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivatedEventArgs"/> class.
        /// </summary>
        /// <param name="isFirstActivation">if set to <c>true</c> [is first activation].</param>
        public ActivatedEventArgs(bool isFirstActivation)
        {
            this.IsFirstActivation = isFirstActivation;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is first activation.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is first activation; otherwise, <c>false</c>.
        /// </value>
        public bool IsFirstActivation { get; }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExitRequestEventArgs.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework
{
    using System;

    /// <summary>
    /// Exiting event args used when application is exiting.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class ExitRequestEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ExitRequestEventArgs"/> is cancel.
        /// </summary>
        /// <value>
        ///   <c>true</c> if cancel; otherwise, <c>false</c>.
        /// </value>
        public bool Cancel { get; set; }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationExitEvents.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework
{
    using System;
    using System.Threading;

    /// <summary>
    /// Interface for listening to application exit requests.
    /// </summary>
    public interface IApplicationExitEvents
    {
        /// <summary>
        /// Occurs when request exit is called, but allows the app to cancel the request.
        /// </summary>
        event EventHandler<ExitRequestEventArgs> ExitRequest;

        /// <summary>
        /// Occurs when the application will exit.
        /// </summary>
        event EventHandler<EventArgs> Exiting;

        /// <summary>
        /// Gets the exit cancellation token.
        /// </summary>
        /// <value>
        /// The exit cancellation token.
        /// </value>
        CancellationToken ExitCancellationToken { get; }
    }
}
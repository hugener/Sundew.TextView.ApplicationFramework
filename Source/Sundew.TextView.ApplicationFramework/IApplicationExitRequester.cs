// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationExitRequester.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework
{
    /// <summary>
    /// Interface for requesting application exit.
    /// </summary>
    public interface IApplicationExitRequester
    {
        /// <summary>
        /// Requests to exit the application.
        /// </summary>
        /// <returns>A value indicating whether the request was successful.</returns>
        bool RequestExit();

        /// <summary>
        /// Exits this instance.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "It's an application framework.")]
        void Exit();
    }
}
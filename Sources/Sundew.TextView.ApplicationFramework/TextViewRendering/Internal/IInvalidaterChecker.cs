// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInvalidaterChecker.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering.Internal
{
    using System;

    /// <summary>
    /// Interface for checking if the current <see cref="ITextView"/> should be rendered.
    /// </summary>
    internal interface IInvalidaterChecker : IInvalidater, IDisposable
    {
        /// <summary>
        /// Waits until the invalidater is invalidated and resets it.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is render required and reset]; otherwise, <c>false</c>.
        /// </returns>
        bool WaitForInvalidatedAndReset();
    }
}
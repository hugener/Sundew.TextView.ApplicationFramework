// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IActivityAggregator.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.Input
{
    using System;

    /// <summary>
    /// Interface for aggregating activity in an application.
    /// </summary>
    public interface IActivityAggregator
    {
        /// <summary>
        /// Occurs when there is activity in the application.
        /// </summary>
        event EventHandler<EventArgs> ActivityOccured;
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIApplicationInputManagement.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework
{
    using Sundew.TextView.ApplicationFramework.Input;

    /// <summary>
    /// Application Input management interface.
    /// </summary>
    public interface IIApplicationInputManagement
    {
        /// <summary>
        /// Gets or sets the input manager reporter.
        /// </summary>
        /// <value>
        /// The input manager reporter.
        /// </value>
        IInputManagerReporter? InputManagerReporter { get; set; }

        /// <summary>
        /// Gets the input manager.
        /// </summary>
        /// <value>
        /// The input manager.
        /// </value>
        IInputManager InputManager { get; }
    }
}
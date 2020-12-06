// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInputTarget.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Sundew.TextView.ApplicationFramework.Input
{
    /// <summary>
    /// Interface for implementing input targets, which may be activated or deactivated by the <see cref="InputManager"/>.
    /// </summary>
    public interface IInputTarget
    {
        /// <summary>
        /// Called when [activated].
        /// </summary>
        void OnActivated();

        /// <summary>
        /// Called when [deactivated].
        /// </summary>
        void OnDeactivated();
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScrollMode.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Sundew.TextView.ApplicationFramework.TextViewRendering.Animation
{
    /// <summary>
    /// Specifies the scroll mode for <see cref="TextScroller"/>.
    /// </summary>
    public enum ScrollMode
    {
        /// <summary>
        /// The text bounces back and forth.
        /// </summary>
        Bounce,

        /// <summary>
        /// The text scrolls and then restarts.
        /// </summary>
        Restart,
    }
}
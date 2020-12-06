// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICharacterContext.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    /// <summary>
    /// Interface for implementing a character context.
    /// </summary>
    public interface ICharacterContext
    {
        /// <summary>
        /// Gets the size of the pattern.
        /// </summary>
        /// <value>
        /// The size of the pattern.
        /// </value>
        Size PatternSize { get; }

        /// <summary>
        /// Sets the custom character.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="pattern">The pattern.</param>
        void SetCustomCharacter(byte character, byte[] pattern);
    }
}
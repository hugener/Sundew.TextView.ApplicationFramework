// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CharacterContext.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Sundew.TextView.ApplicationFramework.TextViewRendering;

namespace Sundew.TextView.Iot.Devices.Drivers
{
    /// <summary>
    /// Builder for custom charactors on a Hd44780 LCD display.
    /// </summary>
    public class CharacterContext : ICharacterContext
    {
        private readonly global::Iot.Device.CharacterLcd.Hd44780 hd44780;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterContext" /> class.
        /// </summary>
        /// <param name="hd44780">The HD44780.</param>
        /// <param name="patternSize">Size of the pattern.</param>
        public CharacterContext(global::Iot.Device.CharacterLcd.Hd44780 hd44780, Size patternSize)
        {
            this.hd44780 = hd44780;
            this.PatternSize = patternSize;
        }

        /// <summary>
        /// Gets the size of the pattern.
        /// </summary>
        /// <value>
        /// The size of the pattern.
        /// </value>
        public Size PatternSize { get; }

        /// <summary>
        /// Sets the custom character.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="pattern">The pattern.</param>
        public void SetCustomCharacter(byte character, byte[] pattern)
        {
            this.hd44780.CreateCustomCharacter(character, pattern);
        }
    }
}

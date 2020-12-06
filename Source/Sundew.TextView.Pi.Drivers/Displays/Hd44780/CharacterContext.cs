// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CharacterContext.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.Pi.Drivers.Displays.Hd44780
{
    using global::Pi.IO.Devices.Displays.Hd44780;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Builder for custom charactors on a Hd44780 LCD display.
    /// </summary>
    public class CharacterContext : ICharacterContext
    {
        private readonly Hd44780LcdDevice hd44780LcdDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterContext" /> class.
        /// </summary>
        /// <param name="hd44780LcdDevice">The HD44780 LCD connection.</param>
        /// <param name="patternSize">Size of the pattern.</param>
        public CharacterContext(Hd44780LcdDevice hd44780LcdDevice, Size patternSize)
        {
            this.PatternSize = patternSize;
            this.hd44780LcdDevice = hd44780LcdDevice;
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
            this.hd44780LcdDevice.SetCustomCharacter(character, pattern);
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Hd44780TextDisplayDevice.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Sundew.Base.Computation;
using Sundew.TextView.ApplicationFramework.DeviceInterface;
using Sundew.TextView.ApplicationFramework.TextViewRendering;
using LcdConsole = Iot.Device.CharacterLcd.LcdConsole;

namespace Sundew.TextView.Iot.Devices.Drivers
{
    /// <summary>
    /// Implementation of <see cref="ITextDisplayDevice"/> for <see cref="Hd44780"/>.
    /// </summary>
    /// <seealso cref="ITextDisplayDevice" />
    public class LcdTextDisplayDevice : ITextDisplayDevice
    {
        private readonly global::Iot.Device.CharacterLcd.Hd44780 hd44780;
        private readonly LcdConsole lcdConsole;

        /// <summary>
        /// Initializes a new instance of the <see cref="LcdTextDisplayDevice" /> class.
        /// </summary>
        /// <param name="hd44780">The HD44780 LCD device.</param>
        public LcdTextDisplayDevice(global::Iot.Device.CharacterLcd.Hd44780 hd44780, string romType)
        {
            this.hd44780 = hd44780;
            this.lcdConsole = new LcdConsole(hd44780, romType, false);
        }

        /// <summary>
        /// Gets or sets a value indicating whether [cursor enabled].
        /// </summary>
        /// <value>
        ///  <c>true</c> if [cursor enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool CursorEnabled
        {
            get => this.hd44780.UnderlineCursorVisible;
            set => this.hd44780.UnderlineCursorVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [cursor blinking].
        /// </summary>
        /// <value>
        ///  <c>true</c> if [cursor blinking]; otherwise, <c>false</c>.
        /// </value>
        public bool CursorBlinking
        {
            get => this.hd44780.BlinkingCursorVisible;
            set => this.hd44780.BlinkingCursorVisible = value;
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public Size Size => new Size(this.hd44780.Size.Width, this.hd44780.Size.Height);

        /// <summary>
        /// Gets the cursor position.
        /// </summary>
        /// <value>
        /// The cursor position.
        /// </value>
        public Point CursorPosition => new Point(this.lcdConsole.CursorLeft, this.lcdConsole.CursorTop);

        /// <summary>
        /// Tries the create character context.
        /// </summary>
        /// <returns>The result.</returns>
        public Result.IfSuccess<ICharacterContext> TryCreateCharacterContext()
        {
            return Result.Success<ICharacterContext>(new CharacterContext(this.hd44780, new Size(5, 8)));
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="text">The text.</param>
        public void WriteLine(object text)
        {
            var value = text.ToString();
            if (value != null)
            {
                this.lcdConsole.WriteLine(value);
            }
        }

        /// <summary>
        /// Writes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Write(object text)
        {
            var value = text.ToString();
            if (value != null)
            {
                this.lcdConsole.WriteLine(value);
            }
        }

        /// <summary>
        /// Writes the format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        public void WriteFormat(string format, params object[] values)
        {
            this.Write(string.Format(format, values));
        }

        /// <summary>
        /// Writes the line format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        public void WriteLineFormat(string format, params object[] values)
        {
            this.WriteLine(string.Format(format, values));
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.lcdConsole.Clear();
        }

        /// <summary>
        /// Sets the position.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void SetPosition(int x, int y)
        {
            this.lcdConsole.SetCursorPosition(x, y);
        }
    }
}
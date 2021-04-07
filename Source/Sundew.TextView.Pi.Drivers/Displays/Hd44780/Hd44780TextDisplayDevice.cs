// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Hd44780TextDisplayDevice.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.Pi.Drivers.Displays.Hd44780
{
    using global::Pi.IO.Devices.Displays.Hd44780;
    using Sundew.Base.Primitives.Computation;
    using Sundew.TextView.ApplicationFramework.DeviceInterface;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Implementation of <see cref="ITextDisplayDevice"/> for <see cref="Hd44780LcdDevice"/>.
    /// </summary>
    /// <seealso cref="ITextDisplayDevice" />
    public class Hd44780TextDisplayDevice : ITextDisplayDevice
    {
        private readonly Hd44780LcdDevice hd44780LcdDevice;
        private readonly Hd44780LcdDeviceSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="Hd44780TextDisplayDevice"/> class.
        /// </summary>
        /// <param name="hd44780LcdDevice">The HD44780 LCD device.</param>
        /// <param name="settings">The settings.</param>
        public Hd44780TextDisplayDevice(Hd44780LcdDevice hd44780LcdDevice, Hd44780LcdDeviceSettings settings)
        {
            this.hd44780LcdDevice = hd44780LcdDevice;
            this.settings = settings;
            this.Size = new Size(this.settings.ScreenWidth, this.settings.ScreenHeight);
        }

        /// <summary>
        /// Gets or sets a value indicating whether [cursor enabled].
        /// </summary>
        /// <value>
        ///  <c>true</c> if [cursor enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool CursorEnabled
        {
            get => this.hd44780LcdDevice.CursorEnabled;
            set => this.hd44780LcdDevice.CursorEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [cursor blinking].
        /// </summary>
        /// <value>
        ///  <c>true</c> if [cursor blinking]; otherwise, <c>false</c>.
        /// </value>
        public bool CursorBlinking
        {
            get => this.hd44780LcdDevice.CursorBlinking;
            set => this.hd44780LcdDevice.CursorBlinking = value;
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public Size Size { get; }

        /// <summary>
        /// Gets the cursor position.
        /// </summary>
        /// <value>
        /// The cursor position.
        /// </value>
        public Point CursorPosition => new(this.hd44780LcdDevice.CursorPosition.Column, this.hd44780LcdDevice.CursorPosition.Row);

        /// <summary>
        /// Tries the create character context.
        /// </summary>
        /// <returns>The result.</returns>
        public Result.IfSuccess<ICharacterContext> TryCreateCharacterContext()
        {
            return Result.Success<ICharacterContext>(new CharacterContext(this.hd44780LcdDevice, new Size(this.settings.PatternWidth, this.settings.PatternHeight)));
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="text">The text.</param>
        public void WriteLine(object text)
        {
            this.hd44780LcdDevice.WriteLine(text);
        }

        /// <summary>
        /// Writes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Write(object text)
        {
            this.hd44780LcdDevice.Write(text);
        }

        /// <summary>
        /// Writes the format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        public void WriteFormat(string format, params object[] values)
        {
            this.hd44780LcdDevice.Write(format, values);
        }

        /// <summary>
        /// Writes the line format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        public void WriteLineFormat(string format, params object[] values)
        {
            this.hd44780LcdDevice.WriteLine(format, values);
        }

        /// <summary>
        /// Homes this instance.
        /// </summary>
        public void Home()
        {
            this.hd44780LcdDevice.Home();
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.hd44780LcdDevice.Clear();
        }

        /// <summary>
        /// Sets the position.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void SetPosition(int x, int y)
        {
            this.hd44780LcdDevice.SetCursorPosition(new Hd44780Position { Column = x, Row = y });
        }

        /// <summary>
        /// Moves the specified offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
        public void Move(int offset)
        {
            this.hd44780LcdDevice.Move(offset);
        }
    }
}
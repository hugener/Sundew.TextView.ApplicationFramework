// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleDisplayDevice.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.DeviceInterface
{
    using System;
    using Sundew.Base.Computation;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Uses the console as a display device.
    /// </summary>
    /// <seealso cref="ITextDisplayDevice" />
    public sealed class ConsoleDisplayDevice : ITextDisplayDevice
    {
        /// <summary>
        /// Gets or sets a value indicating whether cursor is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if [cursor enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool CursorEnabled
        {
            get => Console.CursorVisible;
            set => Console.CursorVisible = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [cursor blinking].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [cursor blinking]; otherwise, <c>false</c>.
        /// </value>
        public bool CursorBlinking
        {
            get => true;
            set { }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public Size Size => new Size(Console.WindowWidth, Console.WindowHeight);

        /// <summary>
        /// Gets the cursor position.
        /// </summary>
        /// <value>
        /// The cursor position.
        /// </value>
        public Point CursorPosition => new Point(Console.CursorLeft, Console.CursorTop);

        /// <summary>
        /// Tries the create character context.
        /// </summary>
        /// <returns>Null.</returns>
        public Result<ICharacterContext> TryCreateCharacterContext()
        {
            return Result.Error();
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="text">The text.</param>
        public void WriteLine(object text)
        {
            Console.WriteLine(text);
        }

        /// <summary>
        /// Writes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Write(object text)
        {
            Console.Write(text);
        }

        /// <summary>
        /// Writes the format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        public void WriteFormat(string format, params object[] values)
        {
            Console.Write(format, values);
        }

        /// <summary>
        /// Writes the line format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        public void WriteLineFormat(string format, params object[] values)
        {
            Console.WriteLine(format, values);
        }

        /// <summary>
        /// Homes this instance.
        /// </summary>
        public void Home()
        {
            Console.SetCursorPosition(0, 0);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            Console.Clear();
        }

        /// <summary>
        /// Sets the position.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void SetPosition(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        /// <summary>
        /// Moves the specified offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
        public void Move(int offset)
        {
            var newPosition = Console.CursorLeft + offset;
            Console.SetCursorPosition(newPosition % Console.WindowWidth, newPosition / Console.WindowHeight);
        }
    }
}
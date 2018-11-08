// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextDisplayDevice.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.DeviceInterface
{
    using Sundew.Base.Computation;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Interface for implementing a display device.
    /// </summary>
    public interface ITextDisplayDevice
    {
        /// <summary>
        /// Gets or sets a value indicating whether cursor is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [cursor enabled]; otherwise, <c>false</c>.
        /// </value>
        bool CursorEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether cursor is blinking.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [cursor blinking]; otherwise, <c>false</c>.
        /// </value>
        bool CursorBlinking { get; set; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        Size Size { get; }

        /// <summary>
        /// Gets the cursor position.
        /// </summary>
        /// <value>
        /// The cursor position.
        /// </value>
        Point CursorPosition { get; }

        /// <summary>
        /// Tries the create character context.
        /// </summary>
        /// <returns>The result with an <see cref="ICharacterContext"/> if successfull.</returns>
        Result<ICharacterContext> TryCreateCharacterContext();

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="text">The text.</param>
        void WriteLine(object text);

        /// <summary>
        /// Writes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        void Write(object text);

        /// <summary>
        /// Writes the format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        void WriteFormat(string format, params object[] values);

        /// <summary>
        /// Writes the line format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        void WriteLineFormat(string format, params object[] values);

        /// <summary>
        /// Homes this instance.
        /// </summary>
        void Home();

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();

        /// <summary>
        /// Sets the position.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        void SetPosition(int x, int y);

        /// <summary>
        /// Moves the specified offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
        void Move(int offset);
    }
}
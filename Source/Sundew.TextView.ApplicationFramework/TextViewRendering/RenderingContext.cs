// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderingContext.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Sundew.TextView.ApplicationFramework.DeviceInterface;

    /// <summary>
    /// Render context for rendering on an <see cref="ITextDisplayDevice"/>.
    /// </summary>
    /// <seealso cref="IRenderContext" />
    public sealed class RenderingContext : IRenderingContext
    {
        private readonly ITextDisplayDevice textDisplayDevice;
        private readonly LinkedList<Action> renderInstructions = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingContext" /> class.
        /// </summary>
        /// <param name="textDisplayDevice">The HD44780 LCD connection.</param>
        public RenderingContext(ITextDisplayDevice textDisplayDevice)
        {
            this.textDisplayDevice = textDisplayDevice;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cursor is enabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [cursor enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool CursorEnabled
        {
            get => this.textDisplayDevice.CursorEnabled;
            set => this.textDisplayDevice.CursorEnabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cursor is blinking.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [cursor blinking]; otherwise, <c>false</c>.
        /// </value>
        public bool CursorBlinking
        {
            get => this.textDisplayDevice.CursorBlinking;
            set => this.textDisplayDevice.CursorBlinking = value;
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public Size Size => this.textDisplayDevice.Size;

        /// <summary>
        /// Gets the cursor position.
        /// </summary>
        /// <value>
        /// The cursor position.
        /// </value>
        public Point CursorPosition => this.textDisplayDevice.CursorPosition;

        /// <summary>Gets the number of instructions.</summary>
        /// <value>The number of instructions.</value>
        public int InstructionCount => this.renderInstructions.Count;

        /// <summary>Gets the instructions.</summary>
        /// <value>The instructions.</value>
        public IEnumerable<Action> Instructions => this.renderInstructions;

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="text">The text.</param>
        public void WriteLine(object text)
        {
            this.renderInstructions.AddLast(() => this.textDisplayDevice.WriteLine(text));
        }

        /// <summary>
        /// Writes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void Write(object text)
        {
            this.renderInstructions.AddLast(() => this.textDisplayDevice.Write(text));
        }

        /// <summary>
        /// Writes the format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        public void WriteFormat(string format, params object[] values)
        {
            this.renderInstructions.AddLast(() => this.textDisplayDevice.WriteFormat(format, values));
        }

        /// <summary>
        /// Writes the line format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        public void WriteLineFormat(string format, params object[] values)
        {
            this.renderInstructions.AddLast(() => this.textDisplayDevice.WriteLineFormat(format, values));
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.renderInstructions.AddLast(() => this.textDisplayDevice.Clear());
        }

        /// <summary>
        /// Sets the position.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void SetPosition(int x, int y)
        {
            this.renderInstructions.AddLast(() =>
                this.textDisplayDevice.SetPosition(x, y));
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            this.renderInstructions.Clear();
        }
    }
}
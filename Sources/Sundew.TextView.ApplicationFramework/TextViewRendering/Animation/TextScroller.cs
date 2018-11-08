// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextScroller.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering.Animation
{
    using System;
    using Sundew.Base.Text;
    using Sundew.Base.Threading;

    /// <summary>
    /// Scrolls text based on a timer.
    /// </summary>
    public class TextScroller
    {
        private const char Space = ' ';
        private readonly DirtyFlag dirtyFlag = new DirtyFlag(true);
        private readonly IInvalidater invalidater;
        private readonly ScrollMode scrollMode;
        private readonly TimeSpan startDelay;
        private readonly TimeSpan interval;
        private readonly TimeSpan endDelay;
        private readonly IViewTimer viewTimer;
        private readonly ScrollData scrollData = new ScrollData(null, 0, Direction.Right, 0);
        private string frame = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextScroller" /> class.
        /// </summary>
        /// <param name="invalidater">The invalidater.</param>
        /// <param name="scrollMode">The scroll mode.</param>
        /// <param name="startDelay">The start delay.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="endDelay">The pause delay.</param>
        public TextScroller(IInvalidater invalidater, ScrollMode scrollMode, TimeSpan startDelay, TimeSpan interval, TimeSpan endDelay)
        {
            this.invalidater = invalidater;
            this.scrollMode = scrollMode;
            this.startDelay = startDelay;
            this.interval = interval;
            this.endDelay = endDelay;
            this.viewTimer = invalidater.CreateTimer();
            this.viewTimer.Tick += this.OnViewTimerTick;
        }

        private enum Direction
        {
            Left,
            Right,
        }

        /// <summary>
        /// Gets a value indicating whether this instance is changed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is changed; otherwise, <c>false</c>.
        /// </value>
        public bool IsChanged => this.dirtyFlag;

        /// <summary>
        /// Scrolls the specified text.
        /// </summary>
        /// <returns>
        /// A frame of the animated text.
        /// </returns>
        public string GetFrame(string text, int width)
        {
            if (this.scrollData.Text != text || this.scrollData.Width != width)
            {
                this.scrollData.Text = text;
                this.scrollData.Width = width;
                this.scrollData.Direction = Direction.Right;
                this.scrollData.Ticks = 0;

                if (this.scrollData.Text?.Length > this.scrollData.Width)
                {
                    this.viewTimer.Start(this.startDelay, this.interval);
                }
                else
                {
                    this.viewTimer.Stop();
                }

                this.UpdateFrame();
                return this.frame;
            }

            if (this.dirtyFlag.Clear())
            {
                switch (this.scrollData.Direction)
                {
                    case Direction.Left:
                        if (this.scrollData.Ticks > 0)
                        {
                            this.scrollData.Ticks--;
                            if (this.scrollData.Ticks == 0)
                            {
                                this.scrollData.Direction = Direction.Right;
                                this.viewTimer.Start(this.startDelay, this.interval);
                            }
                        }

                        break;
                    default:
                        var actualTextLength = GetActualTextLength(this.scrollData);
                        if (actualTextLength > this.scrollData.Width)
                        {
                            this.scrollData.Ticks++;
                            actualTextLength = GetActualTextLength(this.scrollData);
                            if (actualTextLength == this.scrollData.Width)
                            {
                                if (this.scrollMode == ScrollMode.Bounce)
                                {
                                    this.scrollData.Direction = Direction.Left;
                                }

                                this.viewTimer.Start(this.endDelay, this.interval);
                            }
                        }
                        else if (actualTextLength == this.scrollData.Width && this.scrollMode == ScrollMode.Restart)
                        {
                            this.scrollData.Ticks = 0;
                            this.viewTimer.Start(this.startDelay, this.interval);
                        }

                        break;
                }

                this.UpdateFrame();
            }

            return this.frame;
        }

        private static int GetActualTextLength(ScrollData scrollData)
        {
            return scrollData.Text.Length - scrollData.Ticks;
        }

        private void UpdateFrame()
        {
            this.frame = string.IsNullOrEmpty(this.scrollData.Text)
                ? string.Empty
                : this.scrollData.Text.Substring(this.scrollData.Ticks).LimitAndPadRight(this.scrollData.Width, Space);
        }

        private void OnViewTimerTick(object sender, EventArgs eventArgs)
        {
            this.dirtyFlag.Mark();
            this.invalidater.Invalidate();
        }

        private class ScrollData
        {
            public ScrollData(string text, int ticks, Direction direction, int width)
            {
                this.Text = text;
                this.Ticks = ticks;
                this.Direction = direction;
                this.Width = width;
            }

            public string Text { get; set; }

            public int Ticks { get; set; }

            public Direction Direction { get; set; }

            public int Width { get; set; }
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBlinker.cs" company="Hukano">
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
    /// Text animation that blinks the text.
    /// </summary>
    public class TextBlinker
    {
        private const char Space = ' ';
        private readonly Flag dirtyFlag = new(true);
        private readonly IInvalidater invalidater;
        private readonly TimeSpan interval;
        private readonly IViewTimer viewTimer;
        private string text = string.Empty;
        private bool isShowing = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBlinker" /> class.
        /// </summary>
        /// <param name="invalidater">The invalidater.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        public TextBlinker(IInvalidater invalidater, TimeSpan interval, bool isEnabled)
        {
            this.invalidater = invalidater;
            this.interval = interval;
            this.viewTimer = invalidater.CreateTimer();
            this.viewTimer.Tick += this.OnViewTimerTick;
            this.IsEnabled = isEnabled;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is changed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is changed; otherwise, <c>false</c>.
        /// </value>
        public bool IsChanged => this.dirtyFlag;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled
        {
            get => this.viewTimer.IsEnabled;
            set
            {
                if (this.viewTimer.IsEnabled != value)
                {
                    this.dirtyFlag.Set();
                    if (value)
                    {
                        this.viewTimer.Start(this.interval, this.interval);
                    }
                    else
                    {
                        this.viewTimer.Stop();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the frame.
        /// </summary>
        /// <returns>
        /// The text to be displayed.
        /// </returns>
        public string GetFrame(string text)
        {
            if (this.dirtyFlag.Clear())
            {
                this.isShowing = !this.isShowing;
            }

            if (this.text != text)
            {
                this.text = text;
                this.isShowing = true;
            }

            var actualText = this.text;
            return this.isShowing
                ? actualText
                : Space.Repeat(actualText.Length);
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            this.text = string.Empty;
            this.isShowing = true;
        }

        private void OnViewTimerTick(object sender, EventArgs e)
        {
            this.dirtyFlag.Set();
            this.invalidater.Invalidate();
        }
    }
}
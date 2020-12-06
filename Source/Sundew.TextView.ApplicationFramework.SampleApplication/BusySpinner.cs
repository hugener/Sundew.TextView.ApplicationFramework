// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusySpinner.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.SampleApplication
{
    using System;
    using TextViewRendering;

    /// <summary>
    /// Animates -\|/ into a spinner.
    /// </summary>
    public class BusySpinner
    {
        private static readonly string[] AnimationSteps = { "-", "\\", "|", "/" };
        private int animationState = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusySpinner"/> class.
        /// </summary>
        /// <param name="invalidater">The invalidater.</param>
        /// <param name="interval">The interval.</param>
        public BusySpinner(IInvalidater invalidater, TimeSpan interval)
        {
            var timer = invalidater.CreateTimer();
            timer.Tick += (s, e) => invalidater.Invalidate();
            timer.Start(interval, interval);
        }

        /// <summary>
        /// Gets the frame.
        /// </summary>
        /// <returns></returns>
        public string GetFrame()
        {
            var state = AnimationSteps[this.animationState++];
            if (this.animationState >= 4)
            {
                this.animationState = 0;
            }

            return state;
        }
    }
}
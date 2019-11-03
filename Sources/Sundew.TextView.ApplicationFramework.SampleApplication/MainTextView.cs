// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainTextView.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.SampleApplication
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TextViewRendering;

    public class MainTextView : ITextView
    {
        private BusySpinner busySpinner;

        public IEnumerable<object> InputTargets
        {
            get { yield return this; }
        }

        public Task OnShowingAsync(IInvalidater invalidater, ICharacterContext characterContext)
        {
            this.busySpinner = new BusySpinner(invalidater, TimeSpan.FromMilliseconds(15));
            return Task.CompletedTask;
        }

        public void OnDraw(IRenderContext renderContext)
        {
            renderContext.CursorEnabled = false;
            renderContext.Home();
            renderContext.WriteLine($"Hello World {this.busySpinner.GetFrame()}");
            renderContext.CursorEnabled = true;
        }

        public Task OnClosingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
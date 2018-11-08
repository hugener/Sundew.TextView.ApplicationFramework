// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewTimer.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Sundew.Base.Threading;

    internal class ViewTimer : IViewTimer, IDisposable
    {
        private readonly ITimer timer;
        private readonly LinkedList<EventHandler> tick = new LinkedList<EventHandler>();
        private bool isListening = false;

        public ViewTimer(ITimer timer)
        {
            this.timer = timer;
        }

        public event EventHandler Tick
        {
            add => this.tick.AddLast(value);
            remove => this.tick.Remove(value);
        }

        public bool IsEnabled => this.timer.IsEnabled;

        public TimeSpan Interval => this.timer.Interval;

        public void StartOnce(TimeSpan startDelay)
        {
            this.Start(startDelay, Timeout.InfiniteTimeSpan);
        }

        public void Start(TimeSpan startDelay)
        {
            this.Start(startDelay, this.Interval);
        }

        public void Start(TimeSpan startDelay, TimeSpan interval)
        {
            this.AttachToTimerTick();
            this.timer.Start(startDelay, interval);
        }

        public void Stop()
        {
            this.timer.Stop();
            this.DetachFromTimerTick();
        }

        public void Reset()
        {
            this.Stop();
            this.tick.Clear();
        }

        public void Dispose()
        {
            this.Reset();
            this.timer.Dispose();
        }

        private void OnTimerTick(ITimer timer)
        {
            foreach (var eventHandler in this.tick)
            {
                eventHandler?.Invoke(this, EventArgs.Empty);
            }
        }

        private void AttachToTimerTick()
        {
            if (!this.isListening)
            {
                this.timer.Tick += this.OnTimerTick;
                this.isListening = true;
            }
        }

        private void DetachFromTimerTick()
        {
            this.timer.Tick -= this.OnTimerTick;
            this.isListening = false;
        }
    }
}
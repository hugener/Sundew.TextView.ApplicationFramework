// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Invalidater.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering.Internal
{
    using System;
    using System.Threading;

    internal sealed class Invalidater : IInvalidaterChecker
    {
        private readonly ViewTimerCache viewTimerCache;
        private AutoResetEvent? autoResetEvent;
        private int isActive = 1;

        public Invalidater(ViewTimerCache viewTimerCache, bool initialState)
        {
            this.autoResetEvent = new AutoResetEvent(initialState);
            this.viewTimerCache = viewTimerCache;
        }

        public IViewTimer CreateTimer()
        {
            return this.viewTimerCache.GetOrCreate();
        }

        public bool WaitForInvalidatedAndReset()
        {
            var isActuallyActive = this.isActive > 0;
            if (isActuallyActive)
            {
                this.autoResetEvent?.WaitOne(Timeout.InfiniteTimeSpan);
            }

            return isActuallyActive;
        }

        public bool Invalidate()
        {
            if (this.isActive > 0)
            {
                this.autoResetEvent?.Set();
                return true;
            }

            return false;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Interlocked.Exchange(ref this.isActive, 0);
            var resetEvent = this.autoResetEvent;
            this.autoResetEvent = null;
            if (resetEvent != null)
            {
                resetEvent.Set();
                resetEvent.Dispose();
            }
        }

        internal class NullInvalidater : IInvalidaterChecker
        {
            private const string NullInvalidaterCannotCreateTimersText = "NullInvalidater cannot create timers.";

            public IViewTimer CreateTimer()
            {
                throw new NotSupportedException(NullInvalidaterCannotCreateTimersText);
            }

            public bool Invalidate()
            {
                return false;
            }

            public void Dispose()
            {
            }

            public bool WaitForInvalidatedAndReset()
            {
                return false;
            }
        }
    }
}
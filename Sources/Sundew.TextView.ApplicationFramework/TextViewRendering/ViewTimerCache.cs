// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewTimerCache.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    using System;
    using System.Collections.Generic;
    using Sundew.Base.Threading;

    internal class ViewTimerCache : IDisposable
    {
        private readonly ITimerFactory timerFactory;
        private readonly List<ViewTimer> timers = new List<ViewTimer>();
        private int usedTimers;

        public ViewTimerCache(ITimerFactory timerFactory)
        {
            this.timerFactory = timerFactory;
        }

        public IViewTimer GetOrCreate()
        {
            if (this.timers.Count <= this.usedTimers)
            {
                this.timers.Add(new ViewTimer(this.timerFactory.Create()));
            }

            return this.timers[this.usedTimers++];
        }

        public void Reset()
        {
            this.timers.ForEach(x => x.Reset());
            this.usedTimers = 0;
        }

        public void Dispose()
        {
            this.timers.ForEach(x => x.Dispose());
            this.usedTimers = 0;
            this.timers.Clear();
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Invalidater.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering.Internal
{
    using System.Threading;

    internal class Invalidater : IInvalidaterChecker, IInvalidater
    {
        private readonly ViewTimerCache viewTimerCache;
        private int invalidate = 1;

        public Invalidater(ViewTimerCache viewTimerCache)
        {
            this.viewTimerCache = viewTimerCache;
        }

        public IViewTimer CreateTimer()
        {
            return this.viewTimerCache.GetOrCreate();
        }

        public bool IsRenderRequiredAndReset()
        {
            return Interlocked.Exchange(ref this.invalidate, 0) == 1;
        }

        public void Invalidate()
        {
            Interlocked.Exchange(ref this.invalidate, 1);
        }

        internal class NullInvalidater : IInvalidaterChecker
        {
            public bool IsRenderRequiredAndReset()
            {
                return false;
            }
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputEvent.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.Input
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    /// Input event.
    /// </summary>
    /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
    public class InputEvent<TEventArgs>
    {
        private readonly ConcurrentDictionary<object, EventHandler<TEventArgs>> targetEventHandlers = new();
        private EventHandler<TEventArgs>? alwaysEventHandlers;

        /// <summary>
        /// Registers the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="eventHandler">The event handler.</param>
        public void Register(object target, EventHandler<TEventArgs> eventHandler)
        {
            this.targetEventHandlers.AddOrUpdate(target, eventHandler, (_, handler) =>
            {
                handler += eventHandler;
                return handler;
            });
        }

        /// <summary>
        /// Registers the specified target.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        public void Register(EventHandler<TEventArgs> eventHandler)
        {
            this.alwaysEventHandlers += eventHandler;
        }

        internal void RaiseLocal(object target, object sender, TEventArgs eventArgs)
        {
            if (this.targetEventHandlers.TryGetValue(target, out var targetEventHandler))
            {
                targetEventHandler?.Invoke(sender, eventArgs);
            }
        }

        internal void RaiseGlobal(object sender, TEventArgs eventArgs)
        {
            this.alwaysEventHandlers?.Invoke(sender, eventArgs);
        }
    }
}
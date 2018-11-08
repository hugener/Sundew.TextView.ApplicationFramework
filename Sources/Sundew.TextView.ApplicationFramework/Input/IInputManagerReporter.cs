// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInputManagerReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.Input
{
    using System.Collections.Generic;
    using Sundew.Base.Reporting;

    /// <summary>
    /// Interface for implementing an observer for <see cref="InputManager"/>.
    /// </summary>
    public interface IInputManagerReporter : IReporter
    {
        /// <summary>
        /// Called when the <see cref="InputManager" /> started a frame.
        /// </summary>
        /// <param name="inputTargets">The input targets.</param>
        void StartedInputContext(IReadOnlyList<object> inputTargets);

        /// <summary>
        /// Called when the <see cref="InputManager"/> added a target.
        /// </summary>
        /// <param name="inputTarget">The input target.</param>
        void AddedTarget(object inputTarget);

        /// <summary>
        /// Called when the <see cref="InputManager"/> removed a target.
        /// </summary>
        /// <param name="inputTarget">The input target.</param>
        void RemovedTarget(object inputTarget);

        /// <summary>
        /// Called when the <see cref="InputManager"/> ended a frame.
        /// </summary>
        /// <param name="inputTargets">The input targets.</param>
        void EndedInputContext(IReadOnlyList<object> inputTargets);

        /// <summary>
        /// Called when the <see cref="InputManager"/> raise an event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="inputEvent">The input event.</param>
        /// <param name="eventArgs">The event arguments instance containing the event data.</param>
        void RaisingEvent<TEventArgs>(InputEvent<TEventArgs> inputEvent, TEventArgs eventArgs);

        /// <summary>
        /// Raiseds the event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="inputEvent">The input event.</param>
        /// <param name="eventArgs">The event arguments.</param>
        void RaisedEvent<TEventArgs>(InputEvent<TEventArgs> inputEvent, TEventArgs eventArgs);
    }
}
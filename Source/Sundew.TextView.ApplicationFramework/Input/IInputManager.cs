// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInputManager.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.Input
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for implementing an input manager.
    /// </summary>
    /// <seealso cref="IActivityAggregator" />
    public interface IInputManager : IActivityAggregator
    {
        /// <summary>
        /// Starts an input context.
        /// </summary>
        /// <param name="inputTargets">The input targets.</param>
        /// <param name="isTemporary">if set to <c>true</c> [is temporary].</param>
        void StartInputContext(IEnumerable<object> inputTargets, bool isTemporary);

        /// <summary>
        /// Starts an input context.
        /// </summary>
        /// <param name="isTemporary">if set to <c>true</c> [is temporary].</param>
        /// <param name="inputTargets">The input targets.</param>
        void StartInputContext(bool isTemporary, params object[] inputTargets);

        /// <summary>
        /// Adds the target.
        /// </summary>
        /// <param name="inputTarget">The input target.</param>
        void AddTarget(object inputTarget);

        /// <summary>
        /// Removes the target.
        /// </summary>
        /// <param name="inputTarget">The input target.</param>
        void RemoveTarget(object inputTarget);

        /// <summary>
        /// Ends the frame.
        /// </summary>
        void EndInputContext();

        /// <summary>
        /// Creates the event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <returns>An <see cref="InputEvent{TEventArgs}"/>.</returns>
        InputEvent<TEventArgs> CreateEvent<TEventArgs>();

        /// <summary>
        /// Raises the specified input event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="inputEvent">The input event.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The event args instance containing the event data.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "InputEvents have two modes, which cannot be implemented with .NET events.")]
        void Raise<TEventArgs>(InputEvent<TEventArgs> inputEvent, object sender, TEventArgs eventArgs);
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputManager.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.Input
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Manages which listener are notified by <see cref="InputEvent{TEventArgs}"/>.
    /// </summary>
    public sealed class InputManager : IInputManager
    {
        private readonly ConcurrentStack<List<object>> inputTargetStack = new ConcurrentStack<List<object>>();
        private readonly IInputManagerReporter inputManagerReporter;
        private List<object> temporaryInputTargets;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputManager" /> class.
        /// </summary>
        /// <param name="inputManagerReporter">The input manager logger.</param>
        public InputManager(IInputManagerReporter inputManagerReporter = null)
        {
            this.inputManagerReporter = inputManagerReporter;
            this.inputManagerReporter?.SetSource(this);
        }

        /// <summary>
        /// Occurs when there is activity in the application.
        /// </summary>
        public event EventHandler<EventArgs> ActivityOccured;

        /// <summary>
        /// Starts an input context.
        /// </summary>
        /// <param name="inputTargets">The input targets.</param>
        /// <param name="isTemporary">if set to <c>true</c> [is temporary].</param>
        public void StartInputContext(IEnumerable<object> inputTargets, bool isTemporary)
        {
            if (this.TryPeekCurrentTarget(out var previousInputTargets))
            {
                previousInputTargets.ForEach(inputTarget =>
                {
                    var actualInputTarget = inputTarget as IInputTarget;
                    actualInputTarget?.OnDeactivated();
                });

                this.inputManagerReporter.EndedInputContext(previousInputTargets);
            }

            var newInputTargets = inputTargets.ToList();
            if (isTemporary)
            {
                this.temporaryInputTargets = newInputTargets;
            }
            else
            {
                this.inputTargetStack.Push(newInputTargets);
                this.temporaryInputTargets = null;
            }

            newInputTargets.ForEach(inputTarget =>
            {
                var actualInputTarget = inputTarget as IInputTarget;
                actualInputTarget?.OnActivated();
            });

            this.inputManagerReporter?.StartedInputContext(newInputTargets);
        }

        /// <summary>
        /// Starts an input context.
        /// </summary>
        /// <param name="isTemporary">if set to <c>true</c> [is temporary].</param>
        /// <param name="inputTargets">The input targets.</param>
        public void StartInputContext(bool isTemporary, params object[] inputTargets)
        {
            this.StartInputContext(inputTargets, isTemporary);
        }

        /// <summary>
        /// Adds the target.
        /// </summary>
        /// <param name="inputTarget">The input target.</param>
        public void AddTarget(object inputTarget)
        {
            if (this.TryPeekCurrentTarget(out var inputTargets))
            {
                inputTargets.Add(inputTarget);
                var actualInputTarget = inputTarget as IInputTarget;
                actualInputTarget?.OnActivated();
                this.inputManagerReporter?.AddedTarget(inputTarget);
            }
        }

        /// <summary>
        /// Removes the target.
        /// </summary>
        /// <param name="inputTarget">The input target.</param>
        public void RemoveTarget(object inputTarget)
        {
            if (this.TryPeekCurrentTarget(out var inputTargets))
            {
                inputTargets.Remove(inputTarget);
                var actualInputTarget = inputTarget as IInputTarget;
                actualInputTarget?.OnDeactivated();
                this.inputManagerReporter?.RemovedTarget(inputTarget);
            }
        }

        /// <summary>
        /// Ends the frame.
        /// </summary>
        public void EndInputContext()
        {
            if (this.TryPopCurrentTarget(out var inputTargets))
            {
                inputTargets.ForEach(inputTarget =>
                {
                    var actualInputTarget = inputTarget as IInputTarget;
                    actualInputTarget?.OnDeactivated();
                });

                this.inputManagerReporter?.EndedInputContext(inputTargets);

                if (this.inputTargetStack.TryPeek(out var currentInputTargets))
                {
                    currentInputTargets.ForEach(inputTarget =>
                    {
                        var actualInputTarget = inputTarget as IInputTarget;
                        actualInputTarget?.OnActivated();
                    });

                    this.inputManagerReporter?.StartedInputContext(currentInputTargets);
                }
            }
        }

        /// <summary>
        /// Creates the event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <returns>An <see cref="InputEvent{TEventArgs}"/>.</returns>
        public InputEvent<TEventArgs> CreateEvent<TEventArgs>()
        {
            return new InputEvent<TEventArgs>();
        }

        /// <summary>
        /// Raises the specified input event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="inputEvent">The input event.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The event args instance containing the event data.</param>
        public void Raise<TEventArgs>(InputEvent<TEventArgs> inputEvent, object sender, TEventArgs eventArgs)
        {
            this.ActivityOccured?.Invoke(this, EventArgs.Empty);
            this.inputManagerReporter?.RaisingEvent(inputEvent, eventArgs);
            inputEvent.RaiseGlobal(sender, eventArgs);

            if (this.TryPeekCurrentTarget(out var inputTargets))
            {
                foreach (var inputTarget in inputTargets)
                {
                    inputEvent.RaiseLocal(inputTarget, sender, eventArgs);
                }
            }

            this.inputManagerReporter?.RaisedEvent(inputEvent, eventArgs);
        }

        private bool TryPeekCurrentTarget(out List<object> inputTargets)
        {
            if (this.temporaryInputTargets != null)
            {
                inputTargets = this.temporaryInputTargets;
                return true;
            }

            return this.inputTargetStack.TryPeek(out inputTargets);
        }

        private bool TryPopCurrentTarget(out List<object> inputTargets)
        {
            if (this.temporaryInputTargets != null)
            {
                inputTargets = this.temporaryInputTargets;
                this.temporaryInputTargets = null;
                return true;
            }

            return this.inputTargetStack.TryPeek(out inputTargets);
        }
    }
}
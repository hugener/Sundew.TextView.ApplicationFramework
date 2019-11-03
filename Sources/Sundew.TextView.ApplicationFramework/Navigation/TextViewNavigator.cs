// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextViewNavigator.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.Navigation
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Sundew.Base.Threading;
    using Sundew.TextView.ApplicationFramework.Input;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Navigates between <see cref="ITextView"/>.
    /// </summary>
    public sealed class TextViewNavigator : ITextViewNavigator
    {
        private static readonly object[] EmptyInputTargets = Array.Empty<object>();
        private readonly TextViewInfo initialTextViewInfo;
        private readonly AsyncLock asyncLock = new AsyncLock();
        private readonly ConcurrentStack<TextViewInfo> screenStack = new ConcurrentStack<TextViewInfo>();
        private readonly ITextViewRenderer textViewRenderer;
        private readonly IInputManager inputManager;
        private TextViewInfo showTextViewInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextViewNavigator" /> class.
        /// </summary>
        /// <param name="textViewRenderer">The text view renderer.</param>
        /// <param name="inputManager">The input manager.</param>
        public TextViewNavigator(ITextViewRenderer textViewRenderer, IInputManager inputManager)
        {
            this.textViewRenderer = textViewRenderer;
            this.inputManager = inputManager;
            this.initialTextViewInfo = new TextViewInfo(this.textViewRenderer.CurrentTextView, false);
            this.screenStack.Push(this.initialTextViewInfo);
            this.showTextViewInfo = this.initialTextViewInfo;
        }

        /// <summary>
        /// Gets the current view.
        /// </summary>
        /// <value>
        /// The current view.
        /// </value>
        public ITextView CurrentView
        {
            get
            {
                if (this.showTextViewInfo.TextView != this.initialTextViewInfo.TextView)
                {
                    return this.showTextViewInfo.TextView;
                }

                if (this.screenStack.TryPeek(out var textViewInfo))
                {
                    return textViewInfo.TextView;
                }

                return this.initialTextViewInfo.TextView;
            }
        }

        /// <summary>
        /// Shows the asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <returns>An async task with a value indicating whether the navigation was successful.</returns>
        public Task<bool> ShowAsync(ITextView textView)
        {
            return this.PrivateShowAsync(textView, null, false, EmptyInputTargets);
        }

        /// <summary>
        /// Shows the modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <returns>An async task with a value indicating whether the navigation was successful.</returns>
        public Task<bool> ShowModalAsync(ITextView textView)
        {
            return this.PrivateShowAsync(textView, null, true, EmptyInputTargets);
        }

        /// <summary>
        /// Shows the asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        public Task<bool> ShowAsync(ITextView textView, Action<ITextView>? onNavigatingAction)
        {
            return this.PrivateShowAsync(textView, onNavigatingAction, false, EmptyInputTargets);
        }

        /// <summary>
        /// Shows the modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        public Task<bool> ShowModalAsync(ITextView textView, Action<ITextView>? onNavigatingAction)
        {
            return this.PrivateShowAsync(textView, onNavigatingAction, true, EmptyInputTargets);
        }

        /// <summary>
        /// Shows the modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="additionalInputTargets">The additional input targets.</param>
        /// <returns>An async task with a value indicating whether the navigation was successful.</returns>
        public Task<bool> ShowModalAsync(ITextView textView, params object[] additionalInputTargets)
        {
            return this.PrivateShowAsync(textView, null, true, additionalInputTargets);
        }

        /// <summary>
        /// Shows the modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="additionalInputTargets">The additional input targets.</param>
        /// <returns>An async task with a value indicating whether the navigation was successful.</returns>
        public Task<bool> ShowModalAsync(ITextView textView, IEnumerable<object> additionalInputTargets)
        {
            return this.PrivateShowAsync(textView, null, true, additionalInputTargets);
        }

        /// <summary>
        /// Shows the modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <param name="additionalInputTargets">The additional input targets.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        public Task<bool> ShowModalAsync(ITextView textView, Action<ITextView>? onNavigatingAction, params object[] additionalInputTargets)
        {
            return this.PrivateShowAsync(textView, onNavigatingAction, true, additionalInputTargets);
        }

        /// <summary>
        /// Shows the modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <param name="additionalInputTargets">The additional input targets.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        public Task<bool> ShowModalAsync(ITextView textView, Action<ITextView>? onNavigatingAction, IEnumerable<object> additionalInputTargets)
        {
            return this.PrivateShowAsync(textView, onNavigatingAction, true, additionalInputTargets);
        }

        /// <summary>
        /// Navigates to asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <returns>An async task with a value indicating whether the navigation was successful.</returns>
        public Task<bool> NavigateToAsync(ITextView textView)
        {
            return this.PrivateNavigateToAsync(textView, null, false, null);
        }

        /// <summary>
        /// Navigates to asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        public Task<bool> NavigateToAsync(ITextView textView, Action<ITextView>? onNavigatingAction)
        {
            return this.PrivateNavigateToAsync(textView, onNavigatingAction, false, null);
        }

        /// <summary>
        /// Navigates to modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <returns>An async task with a value indicating whether the navigation was successful.</returns>
        public Task<bool> NavigateToModalAsync(ITextView textView)
        {
            return this.PrivateNavigateToAsync(textView, null, true, EmptyInputTargets);
        }

        /// <summary>
        /// Navigates to modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        public Task<bool> NavigateToModalAsync(ITextView textView, Action<ITextView>? onNavigatingAction)
        {
            return this.PrivateNavigateToAsync(textView, onNavigatingAction, true, EmptyInputTargets);
        }

        /// <summary>
        /// Navigates to modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="additionalInputTargets">The additional input targets.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        public Task<bool> NavigateToModalAsync(ITextView textView, params object[] additionalInputTargets)
        {
            return this.PrivateNavigateToAsync(textView, null, true, additionalInputTargets);
        }

        /// <summary>
        /// Navigates to modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="additionalInputTargets">The additional input targets.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        public Task<bool> NavigateToModalAsync(ITextView textView, IEnumerable<object> additionalInputTargets)
        {
            return this.PrivateNavigateToAsync(textView, null, true, additionalInputTargets);
        }

        /// <summary>
        /// Navigates to modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <param name="additionalInputTargets">The additional input targets.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        public Task<bool> NavigateToModalAsync(ITextView textView, Action<ITextView>? onNavigatingAction, params object[] additionalInputTargets)
        {
            return this.PrivateNavigateToAsync(textView, onNavigatingAction, true, additionalInputTargets);
        }

        /// <summary>
        /// Navigates to modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <param name="additionalInputTargets">The additional input targets.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        public Task<bool> NavigateToModalAsync(ITextView textView, Action<ITextView>? onNavigatingAction, IEnumerable<object> additionalInputTargets)
        {
            return this.PrivateNavigateToAsync(textView, onNavigatingAction, true, additionalInputTargets);
        }

        /// <summary>
        /// Navigates the back asynchronous.
        /// </summary>
        /// <returns>An async task with a value indicating whether the navigation was successful.</returns>
        public Task<bool> NavigateBackAsync()
        {
            return this.NavigateBackAsync(null);
        }

        /// <summary>
        /// Goes the back.
        /// </summary>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <returns>An async task with a value indicating whether the navigation was successful.</returns>
        public async Task<bool> NavigateBackAsync(Action<ITextView>? onNavigatingAction)
        {
            using (await this.asyncLock.LockAsync().ConfigureAwait(false))
            {
                if (this.showTextViewInfo.TextView != this.initialTextViewInfo.TextView)
                {
                    var oldTextViewInfo = this.showTextViewInfo;
                    this.showTextViewInfo = this.initialTextViewInfo;
                    if (this.screenStack.TryPeek(out var peekedTextViewInfo))
                    {
                        return await NavigateBackAsync(peekedTextViewInfo, oldTextViewInfo).ConfigureAwait(false);
                    }

                    return false;
                }

                if (this.screenStack.TryPop(out var oldCurrentTextViewInfo))
                {
                    if (this.screenStack.TryPeek(out var newCurrentTextViewInfo))
                    {
                        return await NavigateBackAsync(newCurrentTextViewInfo, oldCurrentTextViewInfo).ConfigureAwait(false);
                    }
                }

                return false;
            }

            async Task<bool> NavigateBackAsync(TextViewInfo newTextViewInfo, TextViewInfo oldTextViewInfo)
            {
                var result = await this.textViewRenderer.TrySetViewAsync(newTextViewInfo.TextView, onNavigatingAction).ConfigureAwait(false);
                if (result && oldTextViewInfo.IsModal)
                {
                    this.inputManager?.EndInputContext();
                }

                return result;
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this.asyncLock.Dispose();
        }

        private async Task<bool> PrivateShowAsync(
            ITextView textView,
            Action<ITextView>? onNavigatingAction,
            bool isModal,
            IEnumerable<object>? additionalInputTargets)
        {
            using (await this.asyncLock.LockAsync().ConfigureAwait(false))
            {
                var result = await this.textViewRenderer.TrySetViewAsync(textView, onNavigatingAction).ConfigureAwait(false);
                if (result)
                {
                    this.showTextViewInfo = new TextViewInfo(textView, isModal);
                    if (isModal)
                    {
                        this.inputManager?.StartInputContext(textView.InputTargets.Concat(additionalInputTargets), true);
                    }
                }

                return result;
            }
        }

        private async Task<bool> PrivateNavigateToAsync(
            ITextView textView,
            Action<ITextView>? onNavigatingAction,
            bool isModal,
            IEnumerable<object>? additionalInputTargets)
        {
            using (await this.asyncLock.LockAsync().ConfigureAwait(false))
            {
                var result = await this.textViewRenderer.TrySetViewAsync(textView, oldTextView =>
                {
                    this.showTextViewInfo = this.initialTextViewInfo;
                    onNavigatingAction?.Invoke(oldTextView);
                }).ConfigureAwait(false);
                if (result)
                {
                    this.screenStack.Push(new TextViewInfo(textView, isModal));
                    if (isModal)
                    {
                        if (textView.InputTargets == null || !textView.InputTargets.Any())
                        {
                            throw new NavigationException(textView);
                        }

                        this.inputManager?.StartInputContext(textView.InputTargets.Concat(additionalInputTargets), false);
                    }
                }

                return result;
            }
        }

        private readonly struct TextViewInfo
        {
            public TextViewInfo(ITextView textView, bool isModal)
            {
                this.TextView = textView;
                this.IsModal = isModal;
            }

            public ITextView TextView { get; }

            public bool IsModal { get; }
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextViewNavigator.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Interface for implementing a text view navigator.
    /// </summary>
    public interface ITextViewNavigator
    {
        /// <summary>
        /// Gets the current view.
        /// </summary>
        /// <value>
        /// The current view.
        /// </value>
        ITextView CurrentView { get; }

        /// <summary>
        /// Shows the asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <returns>An async task with a value indicating whether the navigation was successful.</returns>
        Task<bool> ShowAsync(ITextView textView);

        /// <summary>
        /// Shows the modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <returns>An async task with a value indicating whether the navigation was successful.</returns>
        Task<bool> ShowModalAsync(ITextView textView);

        /// <summary>
        /// Shows the asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        Task<bool> ShowAsync(ITextView textView, Action<ITextView> onNavigatingAction);

        /// <summary>
        /// Shows the modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        Task<bool> ShowModalAsync(ITextView textView, Action<ITextView> onNavigatingAction);

        /// <summary>
        /// Shows the modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="additionalInputTargets">The additional input targets.</param>
        /// <returns>An async task with a value indicating whether the navigation was successful.</returns>
        Task<bool> ShowModalAsync(ITextView textView, params object[] additionalInputTargets);

        /// <summary>
        /// Shows the modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="additionalInputTargets">The additional input targets.</param>
        /// <returns>An async task with a value indicating whether the navigation was successful.</returns>
        Task<bool> ShowModalAsync(ITextView textView, IEnumerable<object> additionalInputTargets);

        /// <summary>
        /// Shows the modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <param name="additionalInputTargets">The additional input targets.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        Task<bool> ShowModalAsync(ITextView textView, Action<ITextView> onNavigatingAction, params object[] additionalInputTargets);

        /// <summary>
        /// Shows the modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <param name="additionalInputTargets">The additional input targets.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        Task<bool> ShowModalAsync(ITextView textView, Action<ITextView> onNavigatingAction, IEnumerable<object> additionalInputTargets);

        /// <summary>
        /// Navigates to asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <returns>An async task with a value indicating whether the navigation was successful.</returns>
        Task<bool> NavigateToAsync(ITextView textView);

        /// <summary>
        /// Navigates to asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        Task<bool> NavigateToAsync(ITextView textView, Action<ITextView> onNavigatingAction);

        /// <summary>
        /// Navigates to modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <returns>An async task with a value indicating whether the navigation was successful.</returns>
        Task<bool> NavigateToModalAsync(ITextView textView);

        /// <summary>
        /// Navigates to modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        Task<bool> NavigateToModalAsync(ITextView textView, Action<ITextView> onNavigatingAction);

        /// <summary>
        /// Navigates to modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="additionalInputTargets">The additional input targets.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        Task<bool> NavigateToModalAsync(ITextView textView, params object[] additionalInputTargets);

        /// <summary>
        /// Navigates to modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="additionalInputTargets">The additional input targets.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        Task<bool> NavigateToModalAsync(ITextView textView, IEnumerable<object> additionalInputTargets);

        /// <summary>
        /// Navigates to modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <param name="additionalInputTargets">The additional input targets.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        Task<bool> NavigateToModalAsync(ITextView textView, Action<ITextView> onNavigatingAction, params object[] additionalInputTargets);

        /// <summary>
        /// Navigates to modal asynchronous.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <param name="additionalInputTargets">The additional input targets.</param>
        /// <returns>
        /// An async task with a value indicating whether the navigation was successful.
        /// </returns>
        Task<bool> NavigateToModalAsync(ITextView textView, Action<ITextView> onNavigatingAction, IEnumerable<object> additionalInputTargets);

        /// <summary>
        /// Navigates the back asynchronous.
        /// </summary>
        /// <returns>An async task with a value indicating whether the navigation was successful.</returns>
        Task<bool> NavigateBackAsync();

        /// <summary>
        /// Goes the back.
        /// </summary>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <returns>An async task with a value indicating whether the navigation was successful.</returns>
        Task<bool> NavigateBackAsync(Action<ITextView> onNavigatingAction);
    }
}
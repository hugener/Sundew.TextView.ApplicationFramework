// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextViewRenderer.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    using System;
    using System.Threading.Tasks;
    using Sundew.Base.Computation;

    /// <summary>
    /// Interface for implementing an <see cref="ITextViewRenderer"/>.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface ITextViewRenderer : IDisposable
    {
        /// <summary>
        /// Occurs when [render exception].
        /// </summary>
        event EventHandler<RenderExceptionEventArgs>? RenderException;

        /// <summary>
        /// Gets the current text view.
        /// </summary>
        /// <value>
        /// The current text view.
        /// </value>
        ITextView CurrentTextView { get; }

        /// <summary>
        /// Starts rendering.
        /// </summary>
        void Start();

        /// <summary>
        /// Tries to set the textView.
        /// </summary>
        /// <param name="newTextView">The new textView.</param>
        /// <param name="onNavigatingAction">The on navigating action.</param>
        /// <returns>
        ///   <c>true</c>, if the textView was set, otherwise <c>false</c>.
        /// </returns>
        Task<Result<ITextView, SetTextViewError>> TrySetViewAsync(
            ITextView newTextView,
            Action<ITextView>? onNavigatingAction);
    }
}
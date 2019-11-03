// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextViewRendererFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    using System;

    /// <summary>
    /// Interface for implementing a custom <see cref="ITextViewRendererFactory"/>.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface ITextViewRendererFactory : IDisposable
    {
        /// <summary>
        /// Disposes the specified text view renderer.
        /// </summary>
        /// <param name="textViewRenderer">The text view renderer.</param>
        void Dispose(ITextViewRenderer textViewRenderer);

        /// <summary>
        /// Gets the textView renderer.
        /// </summary>
        /// <value>The textView renderer.</value>
        /// <returns>A new <see cref="ITextViewRenderer"/>.</returns>
        ITextViewRenderer Create();

        /// <summary>Gets the textView renderer.</summary>
        /// <param name="refreshInterval">The refresh interval.</param>
        /// <value>The textView renderer.</value>
        /// <returns>A new <see cref="ITextViewRenderer"/>.</returns>
        ITextViewRenderer Create(TimeSpan refreshInterval);
    }
}
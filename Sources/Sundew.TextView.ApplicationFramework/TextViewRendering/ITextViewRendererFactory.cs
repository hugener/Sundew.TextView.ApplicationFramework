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
        /// Creates a new <see cref="ITextViewRenderer" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="ITextViewRenderer" />.
        /// </returns>
        ITextViewRenderer Create();

        /// <summary>
        /// Disposes the specified text view renderer.
        /// </summary>
        /// <param name="textViewRenderer">The text view renderer.</param>
        void Dispose(ITextViewRenderer textViewRenderer);
    }
}
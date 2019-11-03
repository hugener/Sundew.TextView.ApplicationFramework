// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextView.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for implementing text views.
    /// </summary>
    public interface ITextView
    {
        /// <summary>
        /// Gets the input targets.
        /// </summary>
        /// <value>
        /// The input targets.
        /// </value>
        IEnumerable<object>? InputTargets { get; }

        /// <summary>
        /// Called when is showing.
        /// </summary>
        /// <param name="invalidater">The invalidater.</param>
        /// <param name="characterContext">The character context.</param>
        /// <returns>
        /// An async task.
        /// </returns>
        Task OnShowingAsync(IInvalidater invalidater, ICharacterContext? characterContext);

        /// <summary>
        /// Called when the text view should render.
        /// </summary>
        /// <param name="renderContext">The render context.</param>
        void OnDraw(IRenderContext renderContext);

        /// <summary>
        /// Called when the view is closing.
        /// </summary>
        /// <returns>An async task.</returns>
        Task OnClosingAsync();
    }
}
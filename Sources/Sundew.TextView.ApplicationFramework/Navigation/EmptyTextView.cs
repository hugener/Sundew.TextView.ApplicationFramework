// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmptyTextView.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.Navigation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// A text view with no content. The view is cleared.
    /// </summary>
    /// <seealso cref="ITextView" />
    internal class EmptyTextView : ITextView
    {
        /// <summary>
        /// Gets the input targets.
        /// </summary>
        /// <value>
        /// The input targets.
        /// </value>
        public IEnumerable<object> InputTargets { get; } = null;

        /// <summary>
        /// Called when is showing.
        /// </summary>
        /// <param name="invalidater">The invalidater.</param>
        /// <param name="characterContext">The character context.</param>
        /// <returns>
        /// An async task.
        /// </returns>
        public Task OnShowingAsync(IInvalidater invalidater, ICharacterContext characterContext)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called when the text view should render.
        /// </summary>
        /// <param name="renderContext">The render context.</param>
        public void Render(IRenderContext renderContext)
        {
            renderContext.Clear();
        }

        /// <summary>
        /// Called when the view is closing.
        /// </summary>
        /// <returns>
        /// An async task.
        /// </returns>
        public Task OnClosingAsync()
        {
            return Task.CompletedTask;
        }
    }
}
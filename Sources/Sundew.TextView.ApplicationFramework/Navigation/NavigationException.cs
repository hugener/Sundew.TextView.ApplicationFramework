// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationException.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.Navigation
{
    using System;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Exception thrown when a navigation problem occurs.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class NavigationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationException"/> class.
        /// </summary>
        /// <param name="textView">The text view.</param>
        public NavigationException(ITextView textView)
            : base($"Cannot navigate modally to {textView} because it does not define any input targets.")
        {
            this.TextView = textView;
        }

        /// <summary>
        /// Gets the text view.
        /// </summary>
        /// <value>
        /// The text view.
        /// </value>
        public ITextView TextView { get; }
    }
}
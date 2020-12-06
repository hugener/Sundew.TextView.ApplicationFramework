// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderExceptionEventArgs.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    using System;

    /// <summary>
    /// Event args that are used when a render throws an exception.
    /// </summary>
    public class RenderExceptionEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderExceptionEventArgs"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="textView">The text view.</param>
        public RenderExceptionEventArgs(Exception exception, ITextView? textView)
        {
            this.Exception = exception;
            this.TextView = textView;
        }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; }

        /// <summary>
        /// Gets the text view.
        /// </summary>
        /// <value>
        /// The text view.
        /// </value>
        public ITextView? TextView { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RenderExceptionEventArgs"/> is continue.
        /// </summary>
        /// <value>
        ///   <c>true</c> if continue; otherwise, <c>false</c>.
        /// </value>
        public bool Continue { get; set; }
    }
}
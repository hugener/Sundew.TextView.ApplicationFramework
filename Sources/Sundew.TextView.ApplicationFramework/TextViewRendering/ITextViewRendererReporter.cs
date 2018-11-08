// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextViewRendererReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    using System;
    using Sundew.Base.Reporting;

    /// <summary>
    /// Interface for implementing an observer for <see cref="TextViewRenderer"/>.
    /// </summary>
    public interface ITextViewRendererReporter : IReporter
    {
        /// <summary>
        /// Called when the <see cref="TextViewRenderer"/> has started.
        /// </summary>
        void Started();

        /// <summary>
        /// Called when the <see cref="ITextView"/> has changed.
        /// </summary>
        /// <param name="newTextView">The new text view.</param>
        /// <param name="oldTextView">The old text view.</param>
        void OnViewChanged(ITextView newTextView, ITextView oldTextView);

        /// <summary>
        /// Called when the <see cref="TextViewRenderer"/> has stopped.
        /// </summary>
        void Stopped();

        /// <summary>
        /// Called when [renderer exception].
        /// </summary>
        /// <param name="exception">The exception.</param>
        void OnRendererException(Exception exception);
    }
}
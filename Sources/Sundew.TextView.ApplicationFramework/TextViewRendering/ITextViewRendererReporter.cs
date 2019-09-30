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

        /// <summary>Called when a view has to render.</summary>
        /// <param name="currentTextView">The current text view.</param>
        void OnRender(ITextView currentTextView);

        /// <summary>Called when [rendered].</summary>
        /// <param name="currentTextView">The current text view.</param>
        /// <param name="renderingContext">The rendering context.</param>
        void OnRendered(ITextView currentTextView, IRenderingContext renderingContext);

        /// <summary>
        /// Called when the <see cref="TextViewRenderer"/> has stopped.
        /// </summary>
        void Stopped();

        /// <summary>
        /// Called when [renderer exception].
        /// </summary>
        /// <param name="exception">The exception.</param>
        void OnRendererException(Exception exception);

        /// <summary>Waits for access to change the view.</summary>
        void WaitingForAccessToChangeView();

        /// <summary>Waits for rendering aborted.</summary>
        /// <param name="view">The view.</param>
        void WaitingForRenderingAborted(ITextView view);

        /// <summary>Waits for access to render the view.</summary>
        void WaitingForAccessToRenderView();

        /// <summary>Acquired the view for rendering.</summary>
        /// <param name="view">The view to be rendered.</param>
        void AcquiredViewForRendering(ITextView view);

        /// <summary> The rendering is being aborted.</summary>
        /// <param name="view">The view.</param>
        void AbortingRendering(ITextView view);

        /// <summary>Waiting for the view to invalidate.</summary>
        /// <param name="view">The view.</param>
        void WaitingForViewToInvalidate(ITextView view);
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationRendering.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework
{
    using System;
    using Sundew.TextView.ApplicationFramework.DeviceInterface;
    using Sundew.TextView.ApplicationFramework.Navigation;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// Interface for application rendering.
    /// </summary>
    public interface IApplicationRendering : IIApplicationInputManagement
    {
        /// <summary>
        /// Gets or sets the text view renderer reporter.
        /// </summary>
        /// <value>
        /// The text view renderer reporter.
        /// </value>
        ITextViewRendererReporter? TextViewRendererReporter { get; set; }

        /// <summary>Gets or sets the time interval synchronizer reporter.</summary>
        /// <value>The time interval synchronizer reporter.</value>
        ITimeIntervalSynchronizerReporter? TimeIntervalSynchronizerReporter { get; set; }

        /// <summary>
        /// Starts the rendering.
        /// </summary>
        /// <param name="textDisplayDevice">The text display device.</param>
        /// <returns>A <see cref="TextViewNavigator" />.</returns>
        ITextViewNavigator StartRendering(ITextDisplayDevice textDisplayDevice);

        /// <summary>
        /// Starts the rendering.
        /// </summary>
        /// <param name="textViewRendererFactory">The text view renderer factory.</param>
        /// <returns>A <see cref="TextViewNavigator"/>.</returns>
        ITextViewNavigator StartRendering(ITextViewRendererFactory textViewRendererFactory);

        /// <summary>
        /// Starts the rendering.
        /// </summary>
        /// <param name="textDisplayDevice">The text display device.</param>
        /// <param name="refreshInterval">The refresh interval.</param>
        /// <returns>A <see cref="TextViewNavigator" />.</returns>
        ITextViewNavigator StartRendering(ITextDisplayDevice textDisplayDevice, TimeSpan refreshInterval);

        /// <summary>Starts the rendering.</summary>
        /// <param name="textViewRendererFactory">The text view renderer factory.</param>
        /// <param name="refreshInterval">The refresh interval.</param>
        /// <returns>A <see cref="TextViewNavigator"/>.</returns>
        ITextViewNavigator StartRendering(ITextViewRendererFactory textViewRendererFactory, TimeSpan refreshInterval);
    }
}
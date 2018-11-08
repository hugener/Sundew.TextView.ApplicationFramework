// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextViewRendererFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    using Sundew.Base.Disposal;
    using Sundew.Base.Threading;
    using Sundew.TextView.ApplicationFramework.DeviceInterface;

    /// <summary>
    /// Factory for creating a view renderer.
    /// </summary>
    /// <seealso cref="T:System.IDisposable" />
    public class TextViewRendererFactory : ITextViewRendererFactory
    {
        private readonly DisposingList<ITextViewRenderer> textViewRenderers = new DisposingList<ITextViewRenderer>();
        private readonly ITextDisplayDevice textDisplayDevice;
        private readonly ITimerFactory timerFactory;
        private readonly ITextViewRendererReporter textViewRendererReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextViewRendererFactory" /> class.
        /// </summary>
        /// <param name="textDisplayDevice">The HD44780 LCD device.</param>
        /// <param name="timerFactory">The timer factory.</param>
        /// <param name="textViewRendererReporter">The text view renderer reporter.</param>
        public TextViewRendererFactory(ITextDisplayDevice textDisplayDevice, ITimerFactory timerFactory, ITextViewRendererReporter textViewRendererReporter = null)
        {
            this.textDisplayDevice = textDisplayDevice;
            this.timerFactory = timerFactory;
            this.textViewRendererReporter = textViewRendererReporter;
        }

        /// <summary>
        /// Gets the textView renderer.
        /// </summary>
        /// <value>
        /// The textView renderer.
        /// </value>
        public ITextViewRenderer Create()
        {
            var renderContextFactory = new RenderingContextFactory(this.textDisplayDevice);
            return this.textViewRenderers.Add(new TextViewRenderer(renderContextFactory, this.timerFactory, this.textViewRendererReporter));
        }

        /// <summary>
        /// Disposes the specified text view renderer.
        /// </summary>
        /// <param name="textViewRenderer">The text view renderer.</param>
        public void Dispose(ITextViewRenderer textViewRenderer)
        {
            this.textViewRenderers.Dispose(textViewRenderer);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.textViewRenderers.Dispose();
        }
    }
}
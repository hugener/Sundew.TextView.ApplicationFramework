// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderingContextFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    using Sundew.Base.Computation;
    using Sundew.TextView.ApplicationFramework.DeviceInterface;

    /// <summary>
    /// Rendering context factory for an <see cref="ITextDisplayDevice"/> display.
    /// </summary>
    /// <seealso cref="IRenderingContextFactory" />
    public class RenderingContextFactory : IRenderingContextFactory
    {
        private readonly ITextDisplayDevice textDisplayDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingContextFactory" /> class.
        /// </summary>
        /// <param name="textDisplayDevice">The text display device.</param>
        public RenderingContextFactory(ITextDisplayDevice textDisplayDevice)
        {
            this.textDisplayDevice = textDisplayDevice;
        }

        /// <summary>
        /// Creates the render context.
        /// </summary>
        /// <returns>
        /// A <see cref="RenderingContext" />.
        /// </returns>
        public IRenderingContext CreateRenderingContext()
        {
            return new RenderingContext(this.textDisplayDevice);
        }

        /// <summary>
        /// Tries the create custom character builder.
        /// </summary>
        /// <returns>
        ///   <c>true</c>, if custom characters are supported, otherwise <c>false</c>.
        /// </returns>
        public Result<ICharacterContext> TryCreateCustomCharacterBuilder()
        {
            return this.textDisplayDevice.TryCreateCharacterContext();
        }
    }
}
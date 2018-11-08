// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRenderingContextFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    using Sundew.Base.Computation;

    /// <summary>
    /// Interface for creating render contexts.
    /// </summary>
    public interface IRenderingContextFactory
    {
        /// <summary>
        /// Creates the render context.
        /// </summary>
        /// <returns>A <see cref="IRenderingContext"/>.</returns>
        IRenderingContext CreateRenderingContext();

        /// <summary>
        /// Tries the create custom character builder.
        /// </summary>
        /// <returns>
        ///   <c>true</c>, if custom characters are supported, otherwise <c>false</c>.
        /// </returns>
        Result<ICharacterContext> TryCreateCustomCharacterBuilder();
    }
}
﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRenderingContext.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework.TextViewRendering
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for implementing a rendering context.
    /// </summary>
    public interface IRenderingContext : IRenderContext
    {
        /// <summary>Gets the instruction count.</summary>
        /// <value>The instruction count.</value>
        int InstructionCount { get; }

        /// <summary>Gets the instructions.</summary>
        /// <value>The instructions.</value>
        IEnumerable<Action> Instructions { get; }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        void Reset();
    }
}
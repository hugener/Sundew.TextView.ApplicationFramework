// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationExit.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.TextView.ApplicationFramework
{
    /// <summary>
    /// Interface that combines the <see cref="IApplicationExitEvents"/> and <see cref="IApplicationExitRequester"/>.
    /// </summary>
    /// <seealso cref="IApplicationExitEvents" />
    /// <seealso cref="IApplicationExitRequester" />
    public interface IApplicationExit : IApplicationExitEvents, IApplicationExitRequester
    {
    }
}
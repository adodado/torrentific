// ***********************************************************************
// Assembly         : Torrentific.Framework
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="IAppSettingsService.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using Torrentific.Core.Models;

namespace Torrentific.Framework.Services
{
    /// <summary>
    /// Interface IAppSettingsService
    /// </summary>
    public interface IAppSettingsService
    {
        /// <summary>
        /// Gets the application settings.
        /// </summary>
        /// <value>The application settings.</value>
        ApplicationSettings ApplicationSettings { get; }
        /// <summary>
        /// Applies the new values.
        /// </summary>
        /// <param name="appSettings">The application settings.</param>
        void ApplyNewValues(ApplicationSettings appSettings);
        /// <summary>
        /// Saves the changes.
        /// </summary>
        void SaveChanges();
    }
}
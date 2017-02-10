// ***********************************************************************
// Assembly         : Torrentific.Framework
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="AppSettingsService.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using System.Linq;
using Torrentific.Core.Data;
using Torrentific.Core.Models;

namespace Torrentific.Framework.Services
{
    /// <summary>
    /// Class AppSettingsService.
    /// </summary>
    /// <seealso cref="Torrentific.Framework.Services.IAppSettingsService" />
    public class AppSettingsService : IAppSettingsService
    {
        /// <summary>
        /// The application settings repository
        /// </summary>
        private readonly IDataRepository<ApplicationSettings> _appSettingsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettingsService"/> class.
        /// </summary>
        /// <param name="appSettingsRepository">The application settings repository.</param>
        public AppSettingsService(IDataRepository<ApplicationSettings> appSettingsRepository)
        {
            _appSettingsRepository = appSettingsRepository;
            ApplicationSettings = new ApplicationSettings();
            LoadAppSettings();
        }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        /// <value>The application settings.</value>
        public ApplicationSettings ApplicationSettings { get; private set; }

        /// <summary>
        /// Applies the new values.
        /// </summary>
        /// <param name="appSettings">The application settings.</param>
        public void ApplyNewValues(ApplicationSettings appSettings)
        {
            ApplicationSettings = appSettings;
            _appSettingsRepository.InsertOrUpdate(ApplicationSettings);
            SaveChanges();
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public void SaveChanges()
        {
            _appSettingsRepository.SaveChanges(ApplicationSettings.AppSettingsFile, ApplicationSettings.AppDataFolder);
        }

        /// <summary>
        /// Loads the application settings.
        /// </summary>
        private void LoadAppSettings()
        {
            _appSettingsRepository.Initialize(ApplicationSettings.AppSettingsFile);
            ApplicationSettings = _appSettingsRepository.GetAll().SingleOrDefault();

            if (ApplicationSettings == null)
            {
                LoadDefaultAppSettings();
            }
        }

        /// <summary>
        /// Loads the default application settings.
        /// </summary>
        private void LoadDefaultAppSettings()
        {
            var downloadFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads");

            ApplicationSettings = new ApplicationSettings
            {
                DownloadFolderPath = downloadFolderPath,
                DownloadLimit = 0,
                UploadLimit = 0,
                TurtleModeDownloadLimit = 0,
                TurtleModeUploadLimit = 0
            };

            _appSettingsRepository.InsertOrUpdate(ApplicationSettings);
            SaveChanges();
        }
    }
}
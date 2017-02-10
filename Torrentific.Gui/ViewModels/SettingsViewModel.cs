// ***********************************************************************
// Assembly         : Torrentific
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="SettingsViewModel.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Torrentific.Core.Data;
using Torrentific.Core.Models;
using Torrentific.Framework.Services;
using Torrentific.Framework.Utilities;
using Torrentific.Infrastructure;

namespace Torrentific.ViewModels
{
    /// <summary>
    /// Class SettingsViewModel. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Torrentific.Infrastructure.ViewModelBase" />
    public sealed class SettingsViewModel : ViewModelBase
    {
        /// <summary>
        /// The application settings service
        /// </summary>
        private readonly IAppSettingsService _appSettingsService;
        /// <summary>
        /// The dialog service
        /// </summary>
        private readonly IDialogService _dialogService;
        /// <summary>
        /// The download folder path
        /// </summary>
        private string _downloadFolderPath;
        /// <summary>
        /// The download limit
        /// </summary>
        private string _downloadLimit;
        /// <summary>
        /// The stop torrents when finished
        /// </summary>
        private bool _stopTorrentsWhenFinished;
        /// <summary>
        /// The upload limit
        /// </summary>
        private string _uploadLimit;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="appSettingsService">The application settings service.</param>
        public SettingsViewModel(IDialogService dialogService, IAppSettingsService appSettingsService)
        {
            DisplayName = "Options";

            _dialogService = dialogService;
            _appSettingsService = appSettingsService;
            
            DownloadFolderPath = appSettingsService.ApplicationSettings.DownloadFolderPath;
            StopTorrentsWhenFinished = appSettingsService.ApplicationSettings.StopTorrentsWhenFinished;

            DownloadLimit = (appSettingsService.ApplicationSettings.TurtleModeDownloadLimit/1000).ToString();
            if (appSettingsService.ApplicationSettings.TurtleModeDownloadLimit == 0)
            {
                DownloadLimit = "Unlimited";
            }

            UploadLimit = (appSettingsService.ApplicationSettings.TurtleModeUploadLimit/1000).ToString();
            if (appSettingsService.ApplicationSettings.TurtleModeUploadLimit == 0)
            {
                UploadLimit = "Unlimited";
            }
        }

        /// <summary>
        /// Gets the speed list.
        /// </summary>
        /// <value>The speed list.</value>
        public IEnumerable<string> SpeedList => Res.SpeedList;

        /// <summary>
        /// Gets the subtitle languages.
        /// </summary>
        /// <value>The subtitle languages.</value>
        public IEnumerable<string> SubtitleLanguages => Res.Languages.Values;


        /// <summary>
        /// Gets or sets the download folder path.
        /// </summary>
        /// <value>The download folder path.</value>
        public string DownloadFolderPath
        {
            get { return _downloadFolderPath; }
            set
            {
                _downloadFolderPath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the download limit.
        /// </summary>
        /// <value>The download limit.</value>
        public string DownloadLimit
        {
            get { return _downloadLimit; }
            set
            {
                _downloadLimit = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the upload limit.
        /// </summary>
        /// <value>The upload limit.</value>
        public string UploadLimit
        {
            get { return _uploadLimit; }
            set
            {
                _uploadLimit = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [stop torrents when finished].
        /// </summary>
        /// <value><c>true</c> if [stop torrents when finished]; otherwise, <c>false</c>.</value>
        public bool StopTorrentsWhenFinished
        {
            get { return _stopTorrentsWhenFinished; }
            set
            {
                _stopTorrentsWhenFinished = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Enables selection of a save path for downloads by browsing
        /// </summary>
        public void BrowseDirectory()
        {
            try
            {
                var path = _dialogService.ShowFolderBrowserDialog();
                if (!string.IsNullOrEmpty(path))
                {
                    DownloadFolderPath = path;
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Changes value for opening magnet links in the Windows registry
        /// </summary>
        public void AssociateWithMagnetLinks()
        {
            try
            {
                Utilities.SetMagnetLinkAssociation();
                _dialogService.ShowMessageBox("Done.", Res.ApplicationTitle);
            }

            catch (UnauthorizedAccessException)
            {
                _dialogService.ShowMessageBox(
                    "Please restart the application with administrator rights and set magnet link association again.");
            }

            catch (Exception ex)
            {
                _dialogService.ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Saves the and close.
        /// </summary>
        public void SaveAndClose()
        {
            var settings = new ApplicationSettings
            {
                DownloadFolderPath = DownloadFolderPath,
                StopTorrentsWhenFinished = StopTorrentsWhenFinished,
                TurtleModeUploadLimit = UploadLimit.Equals("Unlimited")
                    ? 0
                    : int.Parse(UploadLimit)*1000,
                TurtleModeDownloadLimit = DownloadLimit.Equals("Unlimited")
                    ? 0
                    : int.Parse(DownloadLimit)*1000
            };

            _appSettingsService.ApplyNewValues(settings);
            _appSettingsService.SaveChanges();
            _dialogService.Close(this);
        }

        /// <summary>
        /// Opens the about.
        /// </summary>
        public void OpenAbout()
        {
            _dialogService.ShowDialog(new AboutViewModel());
        }
    }
}
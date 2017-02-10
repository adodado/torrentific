// ***********************************************************************
// Assembly         : Torrentific.Core
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="ApplicationSettings.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Torrentific.Core.Annotations;
using Torrentific.Core.Data;

namespace Torrentific.Core.Models
{
    /// <summary>
    /// Class ApplicationSettings.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// <seealso cref="Torrentific.Core.Data.IEntity" />
    public class ApplicationSettings : INotifyPropertyChanged, IEntity
    {
        /// <summary>
        /// The download folder path
        /// </summary>
        private string _downloadFolderPath;
        /// <summary>
        /// The download limit
        /// </summary>
        private int _downloadLimit;
        /// <summary>
        /// The turtle mode download limit
        /// </summary>
        private int _turtleModeDownloadLimit;
        /// <summary>
        /// The turtle mode upload limit
        /// </summary>
        private int _turtleModeUploadLimit;
        /// <summary>
        /// The upload limit
        /// </summary>
        private int _uploadLimit;

        /// <summary>
        /// Gets the application data folder.
        /// </summary>
        /// <value>The application data folder.</value>
        public string AppDataFolder
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Torrentific");

        /// <summary>
        /// Gets the application data file.
        /// </summary>
        /// <value>The application data file.</value>
        public string AppDataFile => Path.Combine(AppDataFolder, "TorrentificData.xml");

        /// <summary>
        /// Gets the application session file.
        /// </summary>
        /// <value>The application session file.</value>
        public string AppSessionFile => Path.Combine(AppDataFolder, "TorrentificData.dat");

        /// <summary>
        /// Gets the application settings file.
        /// </summary>
        /// <value>The application settings file.</value>
        public string AppSettingsFile => Path.Combine(AppDataFolder, "TorrentificSettings.xml");

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
        public int DownloadLimit
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
        public int UploadLimit
        {
            get { return _uploadLimit; }
            set
            {
                _uploadLimit = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the turtle mode download limit.
        /// </summary>
        /// <value>The turtle mode download limit.</value>
        public int TurtleModeDownloadLimit
        {
            get { return _turtleModeDownloadLimit; }
            set
            {
                _turtleModeDownloadLimit = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the turtle mode upload limit.
        /// </summary>
        /// <value>The turtle mode upload limit.</value>
        public int TurtleModeUploadLimit
        {
            get { return _turtleModeUploadLimit; }
            set
            {
                _turtleModeUploadLimit = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether [stop torrents when finished].
        /// </summary>
        /// <value><c>true</c> if [stop torrents when finished]; otherwise, <c>false</c>.</value>
        public bool StopTorrentsWhenFinished { get; set; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }

        #region INotifyPropertyChanged Support

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
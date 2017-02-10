// ***********************************************************************
// Assembly         : Torrentific.Framework
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="TorrentService.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Ragnar;
using Torrentific.Core.Data;
using Torrentific.Core.Enums;
using Torrentific.Core.Models;
using Torrentific.Framework.Utilities;

namespace Torrentific.Framework.Services
{
    /// <summary>
    /// Class TorrentService.
    /// </summary>
    /// <seealso cref="Torrentific.Framework.Services.ITorrentService" />
    public class TorrentService : ITorrentService
    {
        /// <summary>
        /// The application settings service
        /// </summary>
        private readonly IAppSettingsService _appSettingsService;
        /// <summary>
        /// The metadata service
        /// </summary>
        private readonly IMetadataService _metadataService;
        /// <summary>
        /// The session
        /// </summary>
        private readonly ISession _session;
        /// <summary>
        /// The torrent repository
        /// </summary>
        private readonly IDataRepository<TorrentEntity> _torrentRepository;
        /// <summary>
        /// The is speed limit mode actived
        /// </summary>
        private bool _isSpeedLimitModeActived;
        /// <summary>
        /// The torrent entities
        /// </summary>
        private ObservableCollection<TorrentEntity> _torrentEntities;

        /// <summary>
        /// Initializes a new instance of the <see cref="TorrentService"/> class.
        /// </summary>
        /// <param name="torrentRepository">The torrent repository.</param>
        /// <param name="appSettingsService">The application settings service.</param>
        /// <param name="session">The session.</param>
        /// <param name="metadataService">The metadata service.</param>
        /// <param name="subtitleService">The subtitle service.</param>
        public TorrentService(IDataRepository<TorrentEntity> torrentRepository, IAppSettingsService appSettingsService,
            ISession session, IMetadataService metadataService)
        {
            _torrentRepository = torrentRepository;
            _appSettingsService = appSettingsService;
            _session = session;
            _metadataService = metadataService;

            StartTorrentEngine();
            StartTorrentStatsUpdater();
        }

        /// <summary>
        /// Gets the torrents.
        /// </summary>
        /// <value>The torrents.</value>
        public ObservableCollection<TorrentEntity> Torrents
            => _torrentEntities ?? (_torrentEntities = new ObservableCollection<TorrentEntity>());

        /// <summary>
        /// Updates the progress of an individual file in a torrent
        /// </summary>
        /// <param name="torrent">The torrent.</param>
        public void UpdateTorrentFilesStats(TorrentEntity torrent)
        {
            if (torrent.TorrentFiles.Count > 0)
            {
                var index = 0;
                var fileProgresses = torrent.TorrentHandle.GetFileProgresses();

                foreach (var file in torrent.TorrentFiles)
                {
                    if (file.Priority != TorrentPriority.Skip && fileProgresses[index] > 0)
                    {
                        file.Progress = (float) fileProgresses[index]/file.Length*100;
                        file.ProgressText = string.Format(NumberFormatInfo.InvariantInfo, "{0:0} %", file.Progress);
                    }
                    index++;
                }
            }
        }

        /// <summary>
        /// Starts the torrent engine.
        /// </summary>
        public void StartTorrentEngine()
        {
            _session.ListenOn(8000, 8002);
            _session.StartDht();
            _session.StartLsd();
            _session.StartNatPmp();
            _session.StartUpnp();
        }

        /// <summary>
        /// Stops the torrent engine.
        /// </summary>
        public void StopTorrentEngine()
        {
            _session.StopDht();
            _session.StopLsd();
            _session.StopNatPmp();
            _session.StopUpnp();
            _session.Pause();
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            try
            {
                // Load saved session state from file if any exists
                if (File.Exists(_appSettingsService.ApplicationSettings.AppSessionFile))
                {
                    _session.LoadState(File.ReadAllBytes(_appSettingsService.ApplicationSettings.AppSessionFile));
                }
            }
            catch (Exception)
            {
                // File is most likely corrupted so delete it
                File.Delete(_appSettingsService.ApplicationSettings.AppSessionFile);
            }

            _torrentRepository.Initialize(_appSettingsService.ApplicationSettings.AppDataFile);

            Torrents.Clear();
            var loadedTorrents = _torrentRepository.GetAll();
            loadedTorrents.ForEach(x => Torrents.Add(x));
        }

        /// <summary>
        /// Removes the torrents asynchronous.
        /// </summary>
        /// <param name="torrents">The torrents.</param>
        /// <param name="removeData">if set to <c>true</c> [remove data].</param>
        public void RemoveTorrentsAsync(List<TorrentEntity> torrents, bool removeData)
        {
            foreach (var torrent in torrents)
            {
                if (torrent.TorrentHandle != null) _session.RemoveTorrent(torrent.TorrentHandle);

                Torrents.Remove(torrent);
                _torrentRepository.Remove(torrent);
            }
        }

        /// <summary>
        /// Sets the torrent priority.
        /// </summary>
        /// <param name="torrents">The torrents.</param>
        /// <param name="priority">The priority.</param>
        public void SetTorrentPriority(List<TorrentEntity> torrents, TorrentPriority priority)
        {
            foreach (var torrent in torrents)
            {
                foreach (var file in torrent.TorrentFiles.Where(t => t.IsSelected))
                {
                    SetIndividualFilePriority(torrent, file, priority);
                }
            }
        }

        /// <summary>
        /// Deactivates this instance.
        /// </summary>
        public void Deactivate()
        {
            StopTorrentEngine();

            StopTorrents(Torrents.Where(x => x.BasicTorrentState == TorrentBaseState.Started).ToList());

            _torrentRepository.SaveChanges(_appSettingsService.ApplicationSettings.AppDataFile,
                _appSettingsService.ApplicationSettings.AppDataFolder);
        }

        /// <summary>
        /// Resolves the torrent from command line arguments.
        /// </summary>
        /// <param name="argumnets">The argumnets.</param>
        /// <returns>Task&lt;TorrentEntity&gt;.</returns>
        public async Task<TorrentEntity> ResolveTorrentFromCommandLineArgs(string[] argumnets)
        {
            if (argumnets != null && argumnets.Length > 0)
            {
                foreach (var argument in argumnets.Where(x => !string.IsNullOrEmpty(x)))
                {
                    var valid = false;
                    var isMagnet = false;

                    // Check if we have a magnet link
                    if (argument.Length > 6 && argument.Substring(0, 6).Equals("magnet"))
                    {
                        valid = true;
                        isMagnet = true;
                    }

                    // Check if we have a torrent file
                    else if (Path.GetExtension(argument).Equals(".torrent"))
                    {
                        valid = true;
                    }

                    if (valid)
                    {
                        return await InitializeNewTorrentFromMagnetOrFileAsync(argument, isMagnet);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Sets the individual file priority.
        /// </summary>
        /// <param name="torrent">The torrent.</param>
        /// <param name="file">The file.</param>
        /// <param name="priority">The priority.</param>
        public void SetIndividualFilePriority(TorrentEntity torrent, TorrentFileEntity file, TorrentPriority priority)
        {
            var index = torrent.TorrentFiles.IndexOf(file);
            torrent.TorrentHandle.SetFilePriority(index, (int) priority);
            file.Priority = priority;
            file.PriorityText = priority.ToString();
        }

        /// <summary>
        /// Initializes the new torrent from magnet or file asynchronous.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="isMagnet">if set to <c>true</c> [is magnet].</param>
        /// <returns>Task&lt;TorrentEntity&gt;.</returns>
        public Task<TorrentEntity> InitializeNewTorrentFromMagnetOrFileAsync(string uri, bool isMagnet)
        {
            if (string.IsNullOrEmpty(uri)) return null;

            return Task.Run(async () =>
            {
                var result = await _metadataService.RetrieveMetadataFromMagnetOrFile(uri, isMagnet);
                return result.FoundMetadata ? LoadNewTorrent(result.Metadata, uri) : null;
            });
        }
        
        /// <summary>
        /// Adds the torrent to session.
        /// </summary>
        /// <param name="torrent">The torrent.</param>
        public void AddTorrentToSession(TorrentEntity torrent)
        {
            using (var parameters = new AddTorrentParams())
            {
                parameters.SavePath = torrent.SavePath;
                parameters.TorrentInfo = new TorrentInfo(torrent.TorrentMetadata);
                torrent.TorrentHandle = _session.AddTorrent(parameters);
            }

            if (!Torrents.Contains(torrent))
            {
                for (var i = 0; i < torrent.TorrentFiles.Count; i++)
                {
                    if (!torrent.TorrentFiles[i].IsSelected || torrent.TorrentFiles[i].Priority == TorrentPriority.Skip)
                    {
                        torrent.TorrentHandle.SetFilePriority(i, 0);
                        torrent.TorrentFiles[i].Priority = TorrentPriority.Skip;
                    }
                }

                Torrents.Add(torrent);
                _torrentRepository.InsertOrUpdate(torrent);
            }
        }

        /// <summary>
        /// Starts the torrents.
        /// </summary>
        /// <param name="torrents">The torrents.</param>
        public void StartTorrents(List<TorrentEntity> torrents)
        {
            if (torrents.Any())
            {
                torrents.ForEach(StartTorrent);
            }
        }

        /// <summary>
        /// Starts the torrent.
        /// </summary>
        /// <param name="torrent">The torrent.</param>
        public void StartTorrent(TorrentEntity torrent)
        {
            if (torrent.TorrentHandle == null) AddTorrentToSession(torrent);

            if (torrent.TorrentHandle != null)
            {
                torrent.TorrentHandle.AutoManaged = true;
                torrent.TorrentHandle.Resume();
                torrent.BasicTorrentState = TorrentBaseState.Started;

                if (_isSpeedLimitModeActived)
                {
                    torrent.TorrentHandle.DownloadLimit =
                        _appSettingsService.ApplicationSettings.TurtleModeDownloadLimit;
                    torrent.TorrentHandle.UploadLimit = _appSettingsService.ApplicationSettings.UploadLimit;
                }
            }
        }

        /// <summary>
        /// Stops the torrents.
        /// </summary>
        /// <param name="torrents">The torrents.</param>
        public void StopTorrents(List<TorrentEntity> torrents)
        {
            if (torrents.Any())
            {
                torrents.ForEach(StopTorrent);
            }
        }

        /// <summary>
        /// Stops the torrent.
        /// </summary>
        /// <param name="torrent">The torrent.</param>
        public void StopTorrent(TorrentEntity torrent)
        {
            if (torrent.TorrentHandle != null)
            {
                torrent.TorrentHandle.AutoManaged = false;
                torrent.TorrentHandle.Pause();
            }

            torrent.BasicTorrentState = TorrentBaseState.Stopped;
            torrent.Eta = string.Empty;
            torrent.UpSpeed = string.Empty;
            torrent.DownSpeed = string.Empty;
            torrent.State = TorrentBaseState.Stopped.ToString();
        }

        /// <summary>
        /// Toggles the speed limit mode.
        /// </summary>
        /// <param name="activate">if set to <c>true</c> [activate].</param>
        public void ToggleSpeedLimitMode(bool activate)
        {
            _isSpeedLimitModeActived = activate;

            var downloadLimit = 0;
            var uploadLimit = 0;

            if (_isSpeedLimitModeActived)
            {
                downloadLimit = _appSettingsService.ApplicationSettings.TurtleModeDownloadLimit;
                uploadLimit = _appSettingsService.ApplicationSettings.TurtleModeUploadLimit;
            }

            foreach (var torrent in Torrents)
            {
                if (torrent.TorrentHandle != null)
                {
                    torrent.TorrentHandle.DownloadLimit = downloadLimit;
                    torrent.TorrentHandle.UploadLimit = uploadLimit;
                }
            }
        }

        /// <summary>
        /// Continuously updates torrent stats once every second
        /// </summary>
        private void StartTorrentStatsUpdater()
        {
            var updateTimer = new Timer {Interval = 1000};
            updateTimer.Elapsed += (sender, args) =>
            {
                foreach (var torrent in Torrents.Where(torrent => torrent.BasicTorrentState == TorrentBaseState.Started)
                    )
                {
                    using (var status = torrent.TorrentHandle.QueryStatus())
                    {
                        UpdateTorrentStats(torrent, status);
                        UpdateTorrentFilesStats(torrent);
                    }
                }
            };

            updateTimer.Start();
        }

        /// <summary>
        /// Loads the new torrent.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="uri">The URI.</param>
        /// <returns>TorrentEntity.</returns>
        private TorrentEntity LoadNewTorrent(byte[] metadata, string uri)
        {
            using (var torrentInfo = new TorrentInfo(metadata))
            {
                var torrent = new TorrentEntity
                {
                    Id = Guid.NewGuid(),
                    BasicTorrentState = TorrentBaseState.Started,
                    DateAdded = DateTime.Now.ToShortDateString(),
                    TorrentMetadata = metadata,
                    TorrentFiles = new ObservableCollection<TorrentFileEntity>(),
                    TorrentUri = uri,
                    SavePath = _appSettingsService.ApplicationSettings.DownloadFolderPath,
                    Size = GeneralMethods.NumberToFileSize(torrentInfo.TotalSize),
                    Name = torrentInfo.Name
                };

                for (var i = 0; i < torrentInfo.NumFiles; i++)
                {
                    using (var entry = torrentInfo.FileAt(i))
                    {
                        torrent.TorrentFiles.Add(new TorrentFileEntity
                        {
                            Id = Guid.NewGuid(),
                            Name = Path.GetFileName(entry.Path),
                            Path = Path.Combine(_appSettingsService.ApplicationSettings.DownloadFolderPath, entry.Path),
                            Size = GeneralMethods.NumberToFileSize(entry.Size),
                            Length = entry.Size,
                            IsSelected = true,
                            Priority = TorrentPriority.Normal,
                            PriorityText = TorrentPriority.Normal.ToString()
                        });
                    }
                }
                return torrent;
            }
        }

        /// <summary>
        /// Updates the torrent stats.
        /// </summary>
        /// <param name="torrent">The torrent.</param>
        /// <param name="status">The status.</param>
        private void UpdateTorrentStats(TorrentEntity torrent, TorrentStatus status)
        {
            torrent.DownSpeed = GeneralMethods.NumberToSpeed(status.DownloadRate);
            torrent.UpSpeed = GeneralMethods.NumberToSpeed(status.UploadRate);
            torrent.Progress = status.Progress*100;
            torrent.Seeds = status.NumSeeds;
            torrent.Peers = status.NumPeers;

            switch (status.State)
            {
                case TorrentState.Downloading:
                    torrent.State = string.Format(NumberFormatInfo.InvariantInfo, " {0} {1:0.0} %",
                        "Downloading", torrent.Progress);
                    break;
                case TorrentState.CheckingFiles:
                    torrent.State = "Checking files";
                    break;
                case TorrentState.CheckingResumeData:
                    torrent.State = "Checking resume data";
                    break;
                case TorrentState.DownloadingMetadata:
                    torrent.State = "Downloading metadata";
                    break;
                case TorrentState.QueuedForChecking:
                    torrent.State = "Queued for checking";
                    break;
                default:
                    torrent.State = status.State.ToString();
                    break;
            }

            if (status.DownloadRate > 0)
            {
                var timeSpan = new TimeSpan(0, 0,
                    (int) ((status.TotalWanted - status.TotalDone)/status.DownloadRate));
                if (timeSpan.TotalDays > 1)
                    torrent.Eta = $"{timeSpan.TotalDays:0} days {timeSpan.Hours:0} hours";
                else if (timeSpan.TotalHours > 1)
                    torrent.Eta = $"{timeSpan.TotalHours:0} hours {timeSpan.Minutes:0} minutes";
                else if (timeSpan.TotalMinutes > 1)
                    torrent.Eta = $"{timeSpan.TotalMinutes:0} minutes {timeSpan.Seconds:0} seconds";
                else if (timeSpan.TotalSeconds > 1)
                    torrent.Eta = $"{timeSpan.TotalSeconds:0} seconds";
            }

            if (status.IsFinished && !torrent.HasFinishedOnce)
            {
                PerformTorrentFinishedActivities(torrent);
            }
        }

        /// <summary>
        /// Performs the torrent finished activities.
        /// </summary>
        /// <param name="torrent">The torrent.</param>
        private void PerformTorrentFinishedActivities(TorrentEntity torrent)
        {
            torrent.HasFinishedOnce = true;

            if (_appSettingsService.ApplicationSettings.StopTorrentsWhenFinished)
            {
                StopTorrent(torrent);
            }
        }
    }
}
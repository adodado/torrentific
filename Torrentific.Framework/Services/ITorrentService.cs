// ***********************************************************************
// Assembly         : Torrentific.Framework
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="ITorrentService.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Torrentific.Core.Enums;
using Torrentific.Core.Models;

namespace Torrentific.Framework.Services
{
    /// <summary>
    /// Interface ITorrentService
    /// </summary>
    public interface ITorrentService
    {
        /// <summary>
        /// Gets the torrents.
        /// </summary>
        /// <value>The torrents.</value>
        ObservableCollection<TorrentEntity> Torrents { get; }
        /// <summary>
        /// Updates the torrent files stats.
        /// </summary>
        /// <param name="torrent">The torrent.</param>
        void UpdateTorrentFilesStats(TorrentEntity torrent);
        /// <summary>
        /// Starts the torrent engine.
        /// </summary>
        void StartTorrentEngine();
        /// <summary>
        /// Stops the torrent engine.
        /// </summary>
        void StopTorrentEngine();
        /// <summary>
        /// Initializes the new torrent from magnet or file asynchronous.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="isMagnet">if set to <c>true</c> [is magnet].</param>
        /// <returns>Task&lt;TorrentEntity&gt;.</returns>
        Task<TorrentEntity> InitializeNewTorrentFromMagnetOrFileAsync(string uri, bool isMagnet);
        /// <summary>
        /// Adds the torrent to session.
        /// </summary>
        /// <param name="torrent">The torrent.</param>
        void AddTorrentToSession(TorrentEntity torrent);
        /// <summary>
        /// Starts the torrents.
        /// </summary>
        /// <param name="torrents">The torrents.</param>
        void StartTorrents(List<TorrentEntity> torrents);
        /// <summary>
        /// Starts the torrent.
        /// </summary>
        /// <param name="torrent">The torrent.</param>
        void StartTorrent(TorrentEntity torrent);
        /// <summary>
        /// Stops the torrents.
        /// </summary>
        /// <param name="torrents">The torrents.</param>
        void StopTorrents(List<TorrentEntity> torrents);
        /// <summary>
        /// Stops the torrent.
        /// </summary>
        /// <param name="torrent">The torrent.</param>
        void StopTorrent(TorrentEntity torrent);
        /// <summary>
        /// Toggles the speed limit mode.
        /// </summary>
        /// <param name="isTurtleModeActive">if set to <c>true</c> [is turtle mode active].</param>
        void ToggleSpeedLimitMode(bool isTurtleModeActive);
        /// <summary>
        /// Loads this instance.
        /// </summary>
        void Load();
        /// <summary>
        /// Removes the torrents asynchronous.
        /// </summary>
        /// <param name="torrents">The torrents.</param>
        /// <param name="removeData">if set to <c>true</c> [remove data].</param>
        void RemoveTorrentsAsync(List<TorrentEntity> torrents, bool removeData);
        /// <summary>
        /// Sets the individual file priority.
        /// </summary>
        /// <param name="torrent">The torrent.</param>
        /// <param name="file">The file.</param>
        /// <param name="priority">The priority.</param>
        void SetIndividualFilePriority(TorrentEntity torrent, TorrentFileEntity file, TorrentPriority priority);
        /// <summary>
        /// Sets the torrent priority.
        /// </summary>
        /// <param name="torrents">The torrents.</param>
        /// <param name="priority">The priority.</param>
        void SetTorrentPriority(List<TorrentEntity> torrents, TorrentPriority priority);
        /// <summary>
        /// Deactivates this instance.
        /// </summary>
        void Deactivate();
        /// <summary>
        /// Resolves the torrent from command line arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>Task&lt;TorrentEntity&gt;.</returns>
        Task<TorrentEntity> ResolveTorrentFromCommandLineArgs(string[] args);
    }
}
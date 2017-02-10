// ***********************************************************************
// Assembly         : Torrentific.Core
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="Res.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Torrentific.Core.Data
{
    /// <summary>
    /// Class Res.
    /// </summary>
    public static class Res
    {
        /// <summary>
        /// The application title
        /// </summary>
        public const string ApplicationTitle = "Torrentific - the lightweight torrent client that could ;)";

        /// <summary>
        /// The torrent bad health
        /// </summary>
        public const string TorrentBadHealth = "This torrent may have poor health or be dead.";

        /// <summary>
        /// The confirm torrent bad health
        /// </summary>
        public const string ConfirmTorrentBadHealth =
            "Torrent metadata could not be retrieved. This torrent may have poor health or be dead. Do you still wish to add the torrent?";

        /// <summary>
        /// The torrent already exist
        /// </summary>
        public const string TorrentAlreadyExist = "Torrent already exists and cannot be added!";

        /// <summary>
        /// The torrent already exist or is corrupted
        /// </summary>
        public const string TorrentAlreadyExistOrIsCorrupted =
            "The torrent already exists or is corrupted and cannot be added!";

        /// <summary>
        /// The confirm torrent removal
        /// </summary>
        public const string ConfirmTorrentRemoval = "Are you sure you want to remove selected torrents?";

        /// <summary>
        /// The no files are selected
        /// </summary>
        public const string NoFilesAreSelected = "You have not selected any files to download!";

        /// <summary>
        /// The could not generate torrent from magnet link
        /// </summary>
        public const string CouldNotGenerateTorrentFromMagnetLink = "Failed to use magnet link.";

        /// <summary>
        /// The invalid magnet URI
        /// </summary>
        public const string InvalidMagnetUri = "Magnet uri is invalid.";

        /// <summary>
        /// The confirm torrent removal with data
        /// </summary>
        public const string ConfirmTorrentRemovalWithData =
            "Are you sure you want to remove (with data) selected torrents?";

        /// <summary>
        /// The loading torrent metadata
        /// </summary>
        public const string LoadingTorrentMetadata = "Loading torrent metadata...";
        /// <summary>
        /// Something went wrong
        /// </summary>
        public const string SomethingWentWrong = "Oops, something went wrong.";

        /// <summary>
        /// The downloaded torrent removed or moved
        /// </summary>
        public const string DownloadedTorrentRemovedOrMoved =
            "Downloaded file(s) has been removed or moved to another location. Torrent will be removed from the list.";

        /// <summary>
        /// The invalid torrent
        /// </summary>
        public static string InvalidTorrent =
            "Common torrent load failure encountered. This is a general error indicating any of the following problems:" +
            Environment.NewLine + Environment.NewLine +
            "* Torrent is invalid due to poor health or being corrupt." +
            Environment.NewLine + Environment.NewLine +
            "* If the torrent was loaded from a magnet link, Torrentific could have problems reading it. Try loading the torrent from a .torrent file instead." +
            Environment.NewLine + Environment.NewLine +
            "* Torrent caching services are offline. Try loading the torrent again later." +
            Environment.NewLine + Environment.NewLine +
            "* Torrentific does not support this torrent. Try loading the torrent from another service/site if possible.";

        /// <summary>
        /// The speed list
        /// </summary>
        public static readonly List<string> SpeedList = new List<string>
        {
            "Unlimited",
            "10",
            "25",
            "50",
            "100",
            "150",
            "200",
            "250",
            "300",
            "400",
            "500",
            "600",
            "700",
            "800",
            "1000",
            "2000"
        };

        /// <summary>
        /// The languages
        /// </summary>
        public static readonly Dictionary<string, string> Languages = new Dictionary<string, string>
        {
            {"eng", "English"}
        };

        /// <summary>
        /// The torrent categories
        /// </summary>
        public static List<string> TorrentCategories = new List<string>
        {
            "All",
            "Applications",
            "Games",
            "Movies",
            "Music",
            "Other",
            "TV",
            "XXX"
        };

        /// <summary>
        /// The supported video extensions
        /// </summary>
        public static string[] SupportedVideoExtensions = {".wmv", ".avi", ".mpeg", ".ogg", ".mp4", ".ogg", ".mkv"};
    }
}
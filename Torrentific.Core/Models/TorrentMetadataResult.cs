// ***********************************************************************
// Assembly         : Torrentific.Core
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="TorrentMetadataResult.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Torrentific.Core.Models
{
    /// <summary>
    /// Class TorrentMetadataResult.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class TorrentMetadataResult : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TorrentMetadataResult"/> class.
        /// </summary>
        /// <param name="foundMetadata">if set to <c>true</c> [found metadata].</param>
        /// <param name="metadata">The metadata.</param>
        public TorrentMetadataResult(bool foundMetadata, byte[] metadata)
        {
            FoundMetadata = foundMetadata;
            Metadata = metadata;
        }

        /// <summary>
        /// Gets a value indicating whether [found metadata].
        /// </summary>
        /// <value><c>true</c> if [found metadata]; otherwise, <c>false</c>.</value>
        public bool FoundMetadata { get; private set; }
        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        public byte[] Metadata { get; private set; }
    }
}
// ***********************************************************************
// Assembly         : Torrentific.Framework
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="IMetadataService.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Threading.Tasks;
using Torrentific.Core.Models;

namespace Torrentific.Framework.Services
{
    /// <summary>
    /// Interface IMetadataService
    /// </summary>
    public interface IMetadataService
    {
        /// <summary>
        /// Retrieves the metadata from magnet or file.
        /// </summary>
        /// <param name="magnetUri">The magnet URI.</param>
        /// <param name="isMagnet">if set to <c>true</c> [is magnet].</param>
        /// <returns>Task&lt;TorrentMetadataResult&gt;.</returns>
        Task<TorrentMetadataResult> RetrieveMetadataFromMagnetOrFile(string magnetUri, bool isMagnet);
    }
}
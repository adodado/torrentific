// ***********************************************************************
// Assembly         : Torrentific.Framework
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="MetadataService.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Torrentific.Core.Models;

namespace Torrentific.Framework.Services
{
    /// <summary>
    /// Class used for retrieval of torrent metadata, either from internet services or from a .torrent file.
    /// </summary>
    /// <seealso cref="Torrentific.Framework.Services.IMetadataService" />
    public class MetadataService : IMetadataService
    {

        /// <summary>
        /// Retrieves the metadata from magnet or file.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="isMagnet">if set to <c>true</c> [is magnet].</param>
        /// <returns>Task&lt;TorrentMetadataResult&gt;.</returns>
        public async Task<TorrentMetadataResult> RetrieveMetadataFromMagnetOrFile(string uri, bool isMagnet)
        {
            var metadata = isMagnet ? RetrieveMetadataFromInternetServices(uri) : GetMetadataFromFile(uri);

            return new TorrentMetadataResult(metadata != null && metadata.Length > 50, metadata);
        }

        /// <summary>
        /// Retrieves the metadata from internet services.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        private static byte[] RetrieveMetadataFromInternetServices(string uri)
        {
            try
            {
                using (var client = new WebClient())
                {

                    return client.UploadValues("http://magnet2torrent.com/upload/", new NameValueCollection()
                    {
                        {"magnet", uri}
                    });

                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the metadata from file.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>System.Byte[].</returns>
        private byte[] GetMetadataFromFile(string uri)
        {
            try
            {
                return File.ReadAllBytes(uri);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
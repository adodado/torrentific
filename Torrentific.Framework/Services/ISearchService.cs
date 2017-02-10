// ***********************************************************************
// Assembly         : Torrentific.Framework
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="ISearchService.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using ThePirateBay;

namespace Torrentific.Framework.Services
{
    /// <summary>
    /// Interface ISearchService
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Finds the torrents.
        /// </summary>
        /// <param name="searchQuery">The search query.</param>
        /// <param name="category">The category.</param>
        /// <returns>Task&lt;ResponseContainer&lt;SearchResult&gt;&gt;.</returns>
        IEnumerable<Torrent> FindTorrents(string searchQuery, int page, string category);
    }
}
// ***********************************************************************
// Assembly         : Torrentific.Framework
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-09-2017
// ***********************************************************************
// <copyright file="SearchService.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using ThePirateBay;

namespace Torrentific.Framework.Services
{
    /// <summary>
    /// Class SearchService.
    /// </summary>
    /// <seealso cref="Torrentific.Framework.Services.ISearchService" />
    public class SearchService : ISearchService
    {
        /// <summary>
        /// Finds the torrents.
        /// </summary>
        /// <param name="searchQuery">The search query.</param>
        /// <param name="page">The page.</param>
        /// <param name="category">The category.</param>
        /// <returns>Task&lt;ResponseContainer&lt;SearchResult&gt;&gt;.</returns>
        public IEnumerable<Torrent> FindTorrents(string searchQuery,int page, string category)
        {
            var cat = 0;
            switch (category)
            {
                case "Applications":
                    cat = TorrentCategory.AllApplication;
                    break;
                case "Music":
                    cat = TorrentCategory.Audio.Music;
                    break;
                case "Movies":
                    cat = TorrentCategory.Video.Movies;
                    break;
                case "Games":
                    cat = TorrentCategory.AllGames;
                    break;
                case "Other":
                    cat = TorrentCategory.AllOther;
                    break;
                case "TV":
                    cat = TorrentCategory.Video.TVshows;
                    break;
                case "XXX":
                    cat = TorrentCategory.AllPorn;
                    break;
                default:
                    cat = TorrentCategory.All;
                    break;
            }
            //TODO!! Fix setting for queryorder
            var results = Tpb.Search(new Query(searchQuery, page, cat, QueryOrder.BySeeds));
            return results;
        }
    }
}
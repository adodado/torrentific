// ***********************************************************************
// Assembly         : Torrentific
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-09-2017
// ***********************************************************************
// <copyright file="SearchViewModel.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using Castle.Core.Internal;
using ThePirateBay;
using Torrentific.Core.Data;
using Torrentific.Framework.Services;
using Torrentific.Infrastructure;

namespace Torrentific.ViewModels
{
    /// <summary>
    /// Class SearchViewModel.
    /// </summary>
    /// <seealso cref="Torrentific.Infrastructure.ViewModelBase" />
    public class SearchViewModel : ViewModelBase
    {
        /// <summary>
        /// The already sorted properties
        /// </summary>
        private readonly List<string> _alreadySortedProperties = new List<string>();
        /// <summary>
        /// The default list
        /// </summary>
        private readonly List<Torrent> _defaultList = new List<Torrent>();
        /// <summary>
        /// The dialog service
        /// </summary>
        private readonly IDialogService _dialogService;
        /// <summary>
        /// The message service
        /// </summary>
        private readonly IMessageService _messageService;
        /// <summary>
        /// The search service
        /// </summary>
        private readonly ISearchService _searchService;
        /// <summary>
        /// The download torrent command
        /// </summary>
        private ICommand _downloadTorrentCommand;
        /// <summary>
        /// The search command
        /// </summary>
        private ICommand _searchCommand;
        /// <summary>
        /// The search query
        /// </summary>
        private string _searchQuery;
        /// <summary>
        /// The selected torrent category
        /// </summary>
        private string _selectedTorrentCategory;
        /// <summary>
        /// The sort command
        /// </summary>
        private ICommand _sortCommand;
        /// <summary>
        /// The next page search command
        /// </summary>
        private ICommand _nextPageSearchCommand;
        /// <summary>
        /// The search page
        /// </summary>
        private int _searchPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchViewModel"/> class.
        /// </summary>
        /// <param name="searchService">The search service.</param>
        /// <param name="messageService">The message service.</param>
        /// <param name="dialogService">The dialog service.</param>
        public SearchViewModel(ISearchService searchService, IMessageService messageService,
            IDialogService dialogService)
        {
            DisplayName = "Find torrents";

            _searchService = searchService;
            _messageService = messageService;
            _dialogService = dialogService;

            var categories = Res.TorrentCategories;

            TorrentSearchResults = new ObservableCollection<Torrent>();
            TorrentCategories = new ObservableCollection<string>(categories);

            SelectedTorrentCategory = TorrentCategories.First();
        }

        /// <summary>
        /// Gets the torrent search results.
        /// </summary>
        /// <value>The torrent search results.</value>
        public ObservableCollection<Torrent> TorrentSearchResults { get; }

        /// <summary>
        /// Gets or sets the search query.
        /// </summary>
        /// <value>The search query.</value>
        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected torrent category.
        /// </summary>
        /// <value>The selected torrent category.</value>
        public string SelectedTorrentCategory
        {
            get { return _selectedTorrentCategory; }
            set
            {
                _selectedTorrentCategory = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected torrent search result.
        /// </summary>
        /// <value>The selected torrent search result.</value>
        public Torrent SelectedTorrentSearchResult { get; set; }

        /// <summary>
        /// Gets the download torrent command.
        /// </summary>
        /// <value>The download torrent command.</value>
        public ICommand DownloadTorrentCommand
        {
            get
            {
                return _downloadTorrentCommand ??
                       (_downloadTorrentCommand =
                           new RelayCommand(DownloadTorrent, () => SelectedTorrentSearchResult != null));
            }
        }

        /// <summary>
        /// Gets the search command.
        /// </summary>
        /// <value>The search command.</value>
        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ??
                       (_searchCommand = new RelayCommand(SearchTorrents, () => !string.IsNullOrEmpty(SearchQuery)));
            }
        }

        /// <summary>
        /// Gets the next page search command.
        /// </summary>
        /// <value>The next page search command.</value>
        public ICommand NextPageSearchCommand
        {
            get
            {
                return _nextPageSearchCommand ??
                       (_nextPageSearchCommand = new RelayCommand(NextPage, () => !string.IsNullOrEmpty(SearchQuery)));
            }
        }

        /// <summary>
        /// Gets the sort command.
        /// </summary>
        /// <value>The sort command.</value>
        public ICommand SortCommand
            => _sortCommand ?? (_sortCommand = new RelayCommand<string>(SortTorrentSearchResultsCollection));

        /// <summary>
        /// Gets or sets the torrent categories.
        /// </summary>
        /// <value>The torrent categories.</value>
        public ObservableCollection<string> TorrentCategories { get; set; }

        /// <summary>
        /// Sorts the torrent search results collection.
        /// </summary>
        /// <param name="property">The property.</param>
        private void SortTorrentSearchResultsCollection(string property)
        {
            List<Torrent> sortedList = null;

            if (_alreadySortedProperties.Count(x => x == property) == 0)
            {
                switch (property)
                {
                    case "torrent_trusted":
                        sortedList = TorrentSearchResults.OrderBy(x => x.IsTrusted).ToList();
                        break;
                    case "torrent_title":
                        sortedList = TorrentSearchResults.OrderBy(x => x.Name).ToList();
                        break;
                    case "seeds":
                        sortedList = TorrentSearchResults.OrderBy(x => x.Seeds).ToList();
                        break;
                    case "leeches":
                        sortedList = TorrentSearchResults.OrderBy(x => x.Leechers).ToList();
                        break;
                    case "size":
                        sortedList = TorrentSearchResults.OrderBy(x => x.Size).ToList();
                        break;
                    case "upload_date":
                        sortedList = TorrentSearchResults.OrderBy(x => x.Uploaded).ToList();
                        break;
                }

                _alreadySortedProperties.Add(property);
            }
            else if (_alreadySortedProperties.Count(x => x == property) == 1)
            {
                switch (property)
                {
                    case "torrent_category":
                        sortedList = TorrentSearchResults.OrderByDescending(x => x.Category).ToList();
                        break;
                    case "torrent_title":
                        sortedList = TorrentSearchResults.OrderByDescending(x => x.Name).ToList();
                        break;
                    case "seeds":
                        sortedList = TorrentSearchResults.OrderByDescending(x => x.Seeds).ToList();
                        break;
                    case "leeches":
                        sortedList = TorrentSearchResults.OrderByDescending(x => x.Leechers).ToList();
                        break;
                    case "size":
                        sortedList = TorrentSearchResults.OrderByDescending(x => x.Size).ToList();
                        break;
                    case "upload_date":
                        sortedList = TorrentSearchResults.OrderByDescending(x => x.Uploaded).ToList();
                        break;
                }
                _alreadySortedProperties.Add(property);
            }
            else
            {
                _alreadySortedProperties.RemoveAll(x => x == property);
                sortedList = _defaultList;
            }

            if (sortedList != null)
            {
                TorrentSearchResults.Clear();
                sortedList.ForEach(x => TorrentSearchResults.Add(x));
            }
        }

        /// <summary>
        /// Searches the torrents.
        /// </summary>
        public void SearchTorrents()
        {
            try
            {
                IsWorking = true;
                TorrentSearchResults.Clear();
                _defaultList.Clear();

                var searchResponse = _searchService.FindTorrents(SearchQuery.Trim(), 0, SelectedTorrentCategory);
                if (searchResponse != null)
                {
                    var collection = searchResponse as IList<Torrent> ?? searchResponse.ToList();
                    _defaultList.AddRange(collection);
                    collection.ForEach(x => TorrentSearchResults.Add(x));
                }

                IsWorking = false;
                _searchPage++;
            }
            catch (HttpRequestException)
            {
                IsWorking = false;
                TorrentSearchResults.Clear();
                _defaultList.Clear();
            }
            catch (Exception exception)
            {
                IsWorking = false;
                _dialogService.ShowMessageBox(exception.Message, messageBoxImage: MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Nexts the page.
        /// </summary>
        public void NextPage()
        {
            try
            {
                IsWorking = true;
                TorrentSearchResults.Clear();
                _defaultList.Clear();

                var searchResponse = _searchService.FindTorrents(SearchQuery.Trim(), _searchPage, SelectedTorrentCategory);
                if (searchResponse != null)
                {
                    var collection = searchResponse as IList<Torrent> ?? searchResponse.ToList();
                    _defaultList.AddRange(collection);
                    collection.ForEach(x => TorrentSearchResults.Add(x));
                }

                IsWorking = false;
                _searchPage++;
            }
            catch (HttpRequestException)
            {
                IsWorking = false;
                TorrentSearchResults.Clear();
                _defaultList.Clear();
            }
            catch (Exception exception)
            {
                IsWorking = false;
                _dialogService.ShowMessageBox(exception.Message, messageBoxImage: MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Downloads the torrent.
        /// </summary>
        private void DownloadTorrent()
        {
            string[] args = {SelectedTorrentSearchResult.Magnet};

            _messageService.Send(new ApplicationMessage
            {
                Arguments = args,
                MessageType = MessageType.AddNewTorrentFromSearchView
            });
        }
    }
}
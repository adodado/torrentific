// ***********************************************************************
// Assembly         : Torrentific
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="AddNewTorrentViewModel.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Torrentific.Core.Data;
using Torrentific.Core.Models;
using Torrentific.Framework.Services;
using Torrentific.Infrastructure;

namespace Torrentific.ViewModels
{
    /// <summary>
    /// Class AddNewTorrentViewModel.
    /// </summary>
    /// <seealso cref="Torrentific.Infrastructure.ViewModelBase" />
    public class AddNewTorrentViewModel : ViewModelBase
    {
        /// <summary>
        /// The dialog service
        /// </summary>
        private readonly IDialogService _dialogService;
        /// <summary>
        /// The torrent service
        /// </summary>
        private readonly ITorrentService _torrentService;
        /// <summary>
        /// The information text
        /// </summary>
        private string _infoText;
        /// <summary>
        /// The torrent
        /// </summary>
        private TorrentEntity _torrent;
        /// <summary>
        /// The torrent task
        /// </summary>
        private Task<TorrentEntity> _torrentTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddNewTorrentViewModel"/> class.
        /// </summary>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="torrentService">The torrent service.</param>
        public AddNewTorrentViewModel(IDialogService dialogService, ITorrentService torrentService)
        {
            DisplayName = "Add new torrent";

            _dialogService = dialogService;
            _torrentService = torrentService;
        }

        /// <summary>
        /// Gets or sets the information text.
        /// </summary>
        /// <value>The information text.</value>
        public string InfoText
        {
            get { return _infoText; }
            set
            {
                _infoText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the torrent.
        /// </summary>
        /// <value>The torrent.</value>
        public TorrentEntity Torrent
        {
            get { return _torrent; }
            private set
            {
                _torrent = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initializes the specified torrent entity task.
        /// </summary>
        /// <param name="torrentEntityTask">The torrent entity task.</param>
        /// <param name="torrents">The torrents.</param>
        public async void Initialize(Task<TorrentEntity> torrentEntityTask, ObservableCollection<TorrentEntity> torrents)
        {
            try
            {
                _torrentTask = torrentEntityTask;
                Torrent = null;
                IsWorking = true;
                InfoText = Res.LoadingTorrentMetadata;

                Torrent = await torrentEntityTask;

                IsWorking = false;
                InfoText = string.Empty;

                if (Torrent == null)
                {
                    Torrent = null;
                    _dialogService.ShowMessageBox(Res.InvalidTorrent, messageBoxImage: MessageBoxImage.Error);
                }
                else if (torrents.Any(t => t.TorrentUri.Equals(_torrent.TorrentUri)))
                {
                    Torrent = null;
                    _dialogService.ShowMessageBox(Res.TorrentAlreadyExist, messageBoxImage: MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                Torrent = null;
                InfoText = string.Empty;
                IsWorking = false;
                _dialogService.ShowMessageBox(Res.InvalidTorrent, messageBoxImage: MessageBoxImage.Error);
                _dialogService.Close(this);
            }
        }

        /// <summary>
        /// Adds the torrent to session.
        /// </summary>
        public void AddTorrentToSession()
        {
            _torrentTask.Wait(3000);

            if (_torrentTask.IsCompleted)
            {
                _torrentService.AddTorrentToSession(Torrent);
                _dialogService.Close(this);
            }
            else
            {
                Torrent = null;
                InfoText = string.Empty;
                IsWorking = false;
                _dialogService.ShowMessageBox(Res.InvalidTorrent, messageBoxImage: MessageBoxImage.Error);
                _dialogService.Close(this);
            }
        }

        /// <summary>
        /// Selects all.
        /// </summary>
        public void SelectAll()
        {
            if (_torrent != null && _torrent.TorrentFiles.Any())
            {
                foreach (var file in Torrent.TorrentFiles)
                {
                    file.IsSelected = true;
                }
            }
        }

        /// <summary>
        /// Selects the none.
        /// </summary>
        public void SelectNone()
        {
            if (_torrent != null && _torrent.TorrentFiles.Any())
            {
                foreach (var file in Torrent.TorrentFiles)
                {
                    file.IsSelected = false;
                }
            }
        }
    }
}
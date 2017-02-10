// ***********************************************************************
// Assembly         : Torrentific
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="MainViewModel.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Torrentific.Core.Data;
using Torrentific.Core.Enums;
using Torrentific.Core.Models;
using Torrentific.Framework.Services;
using Torrentific.Infrastructure;

namespace Torrentific.ViewModels
{
    /// <summary>
    /// Class MainViewModel. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Torrentific.Infrastructure.ViewModelBase" />
    public sealed class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// The dialog service
        /// </summary>
        private readonly IDialogService _dialogService;
        /// <summary>
        /// The messenger
        /// </summary>
        private readonly IMessageService _messenger;
        /// <summary>
        /// The service manager
        /// </summary>
        private readonly IServiceManager _serviceManager;
        /// <summary>
        /// The torrent service
        /// </summary>
        private readonly ITorrentService _torrentService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="torrentService">The torrent service.</param>
        /// <param name="messenger">The messenger.</param>
        /// <param name="serviceManager">The service manager.</param>
        public MainViewModel(IDialogService dialogService, ITorrentService torrentService, IMessageService messenger,
            IServiceManager serviceManager)
        {
            DisplayName = "Torrentific.Gui";

            _dialogService = dialogService;
            _torrentService = torrentService;
            _messenger = messenger;
            _serviceManager = serviceManager;
        }

        //<summary>
        //    Runs upon application activation
        //</summary>
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            _messenger.Register<ApplicationMessage>(HandleApplicationMessages);

            _torrentService.Load();

            // Check if a torrent file or magnet link was opened outside of this application
            var args = Environment.GetCommandLineArgs().Where(x => !x.EndsWith(".exe")).ToArray();
            ProcessCommandLineArguments(args);
        }

        /// <summary>
        /// Handles the application messages.
        /// </summary>
        /// <param name="message">The message.</param>
        private void HandleApplicationMessages(ApplicationMessage message)
        {
            try
            {
                switch (message.MessageType)
                {
                    case MessageType.CommandLineMessage:
                        ProcessCommandLineArguments(message.Arguments.ToArray());
                        break;
                    case MessageType.Shutdown:
                        Deactivate();
                        break;
                    case MessageType.AddNewTorrentFromSearchView:
                        var torrent =
                            _torrentService.InitializeNewTorrentFromMagnetOrFileAsync(message.Arguments.First(), true);
                        if (torrent != null)
                        {
                            AddNewTorrent(torrent);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessageBox(ex.ToString());
            }
        }

        /// <summary>
        /// Opens the containing folder.
        /// </summary>
        private void OpenContainingFolder()
        {
            if (SelectedTorrent.TorrentFiles.Any())
            {
                var path = SelectedTorrent.TorrentFiles.First().Path;

                if (File.Exists(path))
                {
                    _dialogService.ShowFileInExplorer(path);
                }
                else
                {
                    _dialogService.ShowMessageBox(Res.DownloadedTorrentRemovedOrMoved);
                    _torrentService.RemoveTorrentsAsync(new List<TorrentEntity> {SelectedTorrent}, false);
                }
            }
        }

        /// <summary>
        /// Torrentses the grid double clicked.
        /// </summary>
        public void TorrentsGridDoubleClicked()
        {
            if (SelectedTorrent != null)
            {
                if (SelectedTorrent.HasFinishedOnce)
                {
                    OpenContainingFolder();
                }

                else if (SelectedTorrent.BasicTorrentState == TorrentBaseState.Started)
                {
                    _torrentService.StopTorrent(SelectedTorrent);
                }

                else if (SelectedTorrent.BasicTorrentState == TorrentBaseState.Stopped)
                {
                    _torrentService.StartTorrent(SelectedTorrent);
                }
            }
        }

        /// <summary>
        /// Remove selected torrents
        /// </summary>
        private void RemoveTorrent()
        {
            try
            {
                var result = _dialogService.ShowMessageBox(Res.ConfirmTorrentRemoval, Res.ApplicationTitle,
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    _torrentService.RemoveTorrentsAsync(SelectedTorrents.Cast<TorrentEntity>().ToList(), false);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessageBox(ex.ToString());
            }
        }

        /// <summary>
        /// Remove selected torrents and downloaded files
        /// </summary>
        private void RemoveTorrentWithData()
        {
            try
            {
                var result = _dialogService.ShowMessageBox(Res.ConfirmTorrentRemovalWithData,
                    Res.ApplicationTitle, MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    _torrentService.RemoveTorrentsAsync(SelectedTorrents.Cast<TorrentEntity>().ToList(), true);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessageBox(ex.ToString());
            }
        }

        /// <summary>
        /// Set the priority of a single file in a torrent
        /// </summary>
        /// <param name="param">The parameter.</param>
        private void SetFilePriority(string param)
        {
            TorrentPriority priority;

            if (param.Equals(TorrentPriority.Low.ToString()))
                priority = TorrentPriority.Low;
            else if (param.Equals(TorrentPriority.High.ToString()))
                priority = TorrentPriority.High;
            else if (param.Equals(TorrentPriority.Skip.ToString()))
                priority = TorrentPriority.Skip;
            else
                priority = TorrentPriority.Normal;

            _torrentService.SetIndividualFilePriority(SelectedTorrent, SelectedFileEntity, priority);
        }

        /// <summary>
        /// Set the priority of all files in a torrent
        /// </summary>
        /// <param name="param">The parameter.</param>
        private void SetTorrentPriority(string param)
        {
            TorrentPriority priority;

            if (param.Equals(TorrentPriority.Low.ToString()))
                priority = TorrentPriority.Low;
            else if (param.Equals(TorrentPriority.High.ToString()))
                priority = TorrentPriority.High;
            else
                priority = TorrentPriority.Normal;

            _torrentService.SetTorrentPriority(SelectedTorrents.Cast<TorrentEntity>().ToList(), priority);
        }

        /// <summary>
        /// Open dialog to let user select a torrent file to add
        /// </summary>
        public void AddTorrentFile()
        {
            try
            {
                var path = _dialogService.ShowOpenFileDialog();
                if (path != null)
                {
                    AddNewTorrent(_torrentService.InitializeNewTorrentFromMagnetOrFileAsync(path, false));
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Open dialog to let user type in the magnet link uri of a torrent
        /// </summary>
        public void AddTorrentFromUri()
        {
            try
            {
                var win = _serviceManager.Get<ManualDownloadViewModel>();
                if (_dialogService.ShowDialog(win) == true)
                {
                    if (!string.IsNullOrEmpty(win.MagnetLinkText) && win.MagnetLinkText.Length > 6 &&
                        win.MagnetLinkText.Substring(0, 6).Equals("magnet"))
                    {
                        var torrent = _torrentService.InitializeNewTorrentFromMagnetOrFileAsync(win.MagnetLinkText, true);
                        if (torrent != null)
                        {
                            AddNewTorrent(torrent);
                        }
                        else
                        {
                            _dialogService.ShowMessageBox(Res.InvalidTorrent);
                        }
                    }
                    else
                    {
                        _dialogService.ShowMessageBox(Res.InvalidMagnetUri);
                    }
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Open dialog to let user select files to download or cancel
        /// </summary>
        /// <param name="torrent">The torrent.</param>
        private void AddNewTorrent(Task<TorrentEntity> torrent)
        {
            try
            {
                // MainWindow must've been shown before it can be used as an owner for dialog windows
                // This check with possible action is needed since this method could run on application startup before the MainWindow has been shown
                if (!Application.Current.MainWindow.IsLoaded)
                {
                    Application.Current.MainWindow.Show();
                }

                var win = _serviceManager.Get<AddNewTorrentViewModel>();
                win.Initialize(torrent, Torrents);

                if (_dialogService.ShowDialog(win) == true)
                {
                    _torrentService.AddTorrentToSession(win.Torrent);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Open dialog with application options
        /// </summary>
        public void OpenOptions()
        {
            try
            {
                _dialogService.ShowDialog(_serviceManager.Get<SettingsViewModel>());
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Opens the search view.
        /// </summary>
        public void OpenSearchView()
        {
            _dialogService.ShowWindow(_serviceManager.Get<SearchViewModel>());
        }

        /// <summary>
        /// Turn on or off 'turtle mode'
        /// </summary>
        public void ToggleTurtleMode()
        {
            try
            {
                _torrentService.ToggleSpeedLimitMode(IsTurtleModeActive);
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Handle incoming command line arguments
        /// </summary>
        /// <param name="args">The arguments.</param>
        private void ProcessCommandLineArguments(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    AddNewTorrent(_torrentService.ResolveTorrentFromCommandLineArgs(args));
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessageBox(ex.Message);
            }
        }

        /// <summary>
        /// Runs upon application shutdown
        /// </summary>
        public void Deactivate()
        {
            _torrentService.Deactivate();
            _dialogService.CloseOpenWindows();
        }

        #region Properties

        /// <summary>
        /// The is turtle mode active
        /// </summary>
        private bool _isTurtleModeActive;
        /// <summary>
        /// The open containing folder command
        /// </summary>
        private ICommand _openContainingFolderCommand;
        /// <summary>
        /// The open file command
        /// </summary>
        private ICommand _openFileCommand;
        /// <summary>
        /// The remove torrent command
        /// </summary>
        private ICommand _removeTorrentCommand;
        /// <summary>
        /// The remove torrent with data command
        /// </summary>
        private ICommand _removeTorrentWithDataCommand;
        /// <summary>
        /// The selected file entity
        /// </summary>
        private TorrentFileEntity _selectedFileEntity;
        /// <summary>
        /// The selected torrent
        /// </summary>
        private TorrentEntity _selectedTorrent;
        /// <summary>
        /// The selected torrents
        /// </summary>
        private IList _selectedTorrents;
        /// <summary>
        /// The set file command
        /// </summary>
        private ICommand _setFileCommand;
        /// <summary>
        /// The set torrent priority command
        /// </summary>
        private ICommand _setTorrentPriorityCommand;
        /// <summary>
        /// The start command
        /// </summary>
        private ICommand _startCommand;
        /// <summary>
        /// The stop command
        /// </summary>
        private ICommand _stopCommand;

        /// <summary>
        /// Gets the torrents.
        /// </summary>
        /// <value>The torrents.</value>
        public ObservableCollection<TorrentEntity> Torrents => _torrentService.Torrents;

        /// <summary>
        /// Gets the start command.
        /// </summary>
        /// <value>The start command.</value>
        public ICommand StartCommand
        {
            get
            {
                if (_startCommand != null)
                    return _startCommand;
                else
                {
                    _startCommand =
                        new RelayCommand(
                            () => _torrentService.StartTorrents(SelectedTorrents.Cast<TorrentEntity>().ToList()),
                            () =>
                                SelectedTorrent != null &&
                                SelectedTorrent.BasicTorrentState != TorrentBaseState.Started);
                    return _startCommand;
                }
            }
        }

        /// <summary>
        /// Gets the stop command.
        /// </summary>
        /// <value>The stop command.</value>
        public ICommand StopCommand
        {
            get
            {
                return _stopCommand ??
                       (_stopCommand =
                           new RelayCommand(
                               () => _torrentService.StopTorrents(SelectedTorrents.Cast<TorrentEntity>().ToList()),
                               () =>
                                   SelectedTorrent != null &&
                                   SelectedTorrent.BasicTorrentState != TorrentBaseState.Stopped));
            }
        }

        /// <summary>
        /// Gets the open file command.
        /// </summary>
        /// <value>The open file command.</value>
        public ICommand OpenFileCommand
        {
            get
            {
                return _openFileCommand ?? (_openFileCommand = new RelayCommand(() =>
                    _dialogService.OpenFile(SelectedFileEntity.Path), () => SelectedFileEntity != null));
            }
        }

        /// <summary>
        /// Gets the open containing folder command.
        /// </summary>
        /// <value>The open containing folder command.</value>
        public ICommand OpenContainingFolderCommand
        {
            get
            {
                return _openContainingFolderCommand ??
                       (_openContainingFolderCommand = new RelayCommand(OpenContainingFolder,
                           () => SelectedTorrent != null));
            }
        }

        /// <summary>
        /// Gets the set priority command.
        /// </summary>
        /// <value>The set priority command.</value>
        public ICommand SetPriorityCommand
        {
            get
            {
                return _setFileCommand ?? (_setFileCommand = new RelayCommand<string>(SetFilePriority,
                    s => SelectedFileEntity != null));
            }
        }

        /// <summary>
        /// Gets the set torrent priority command.
        /// </summary>
        /// <value>The set torrent priority command.</value>
        public ICommand SetTorrentPriorityCommand
        {
            get
            {
                return _setTorrentPriorityCommand ??
                       (_setTorrentPriorityCommand = new RelayCommand<string>(SetTorrentPriority,
                           s => SelectedTorrent != null));
            }
        }

        /// <summary>
        /// Gets the remove torrent command.
        /// </summary>
        /// <value>The remove torrent command.</value>
        public ICommand RemoveTorrentCommand
        {
            get
            {
                return _removeTorrentCommand ??
                       (_removeTorrentCommand = new RelayCommand(RemoveTorrent, () => SelectedTorrent != null));
            }
        }

        /// <summary>
        /// Gets the remove torrent with data command.
        /// </summary>
        /// <value>The remove torrent with data command.</value>
        public ICommand RemoveTorrentWithDataCommand
        {
            get
            {
                return _removeTorrentWithDataCommand ??
                       (_removeTorrentWithDataCommand =
                           new RelayCommand(RemoveTorrentWithData, () => SelectedTorrent != null));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is turtle mode active.
        /// </summary>
        /// <value><c>true</c> if this instance is turtle mode active; otherwise, <c>false</c>.</value>
        public bool IsTurtleModeActive
        {
            get { return _isTurtleModeActive; }
            set
            {
                _isTurtleModeActive = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected torrent.
        /// </summary>
        /// <value>The selected torrent.</value>
        public TorrentEntity SelectedTorrent
        {
            get { return _selectedTorrent; }
            set
            {
                _selectedTorrent = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected torrents.
        /// </summary>
        /// <value>The selected torrents.</value>
        public IList SelectedTorrents
        {
            get { return _selectedTorrents; }
            set
            {
                _selectedTorrents = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected file entity.
        /// </summary>
        /// <value>The selected file entity.</value>
        public TorrentFileEntity SelectedFileEntity
        {
            get { return _selectedFileEntity; }
            set
            {
                _selectedFileEntity = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
// ***********************************************************************
// Assembly         : Torrentific
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="ManualDownloadViewModel.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Windows;
using System.Windows.Input;
using Torrentific.Infrastructure;

namespace Torrentific.ViewModels
{
    /// <summary>
    /// Class ManualDownloadViewModel. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Torrentific.Infrastructure.ViewModelBase" />
    public sealed class ManualDownloadViewModel : ViewModelBase
    {
        /// <summary>
        /// The window manager
        /// </summary>
        private readonly IDialogService _windowManager;
        /// <summary>
        /// The magnet link text
        /// </summary>
        private string _magnetLinkText;
        /// <summary>
        /// The ok command
        /// </summary>
        private ICommand _okCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualDownloadViewModel"/> class.
        /// </summary>
        /// <param name="windowManager">The window manager.</param>
        public ManualDownloadViewModel(IDialogService windowManager)
        {
            DisplayName = "Add new torrent";
            _windowManager = windowManager;
        }

        /// <summary>
        /// Gets the ok command.
        /// </summary>
        /// <value>The ok command.</value>
        public ICommand OkCommand
        {
            get
            {
                return _okCommand ?? (_okCommand = new RelayCommand(() => _windowManager.Close(this, true),
                    () => !string.IsNullOrWhiteSpace(MagnetLinkText)));
            }
        }

        /// <summary>
        /// Gets or sets the magnet link text.
        /// </summary>
        /// <value>The magnet link text.</value>
        public string MagnetLinkText
        {
            get { return _magnetLinkText; }
            set
            {
                _magnetLinkText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pastes the text.
        /// </summary>
        public void PasteText()
        {
            MagnetLinkText = Clipboard.GetText();
        }
    }
}
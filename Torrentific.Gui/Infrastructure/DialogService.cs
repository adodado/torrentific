// ***********************************************************************
// Assembly         : Torrentific
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="DialogService.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Torrentific.Windows;

namespace Torrentific.Infrastructure
{
    /// <summary>
    /// Service for managing windows and showing common dialogs
    /// </summary>
    /// <seealso cref="Torrentific.Infrastructure.IDialogService" />
    public class DialogService : IDialogService
    {
        /// <summary>
        /// The lock object
        /// </summary>
        private static readonly object LockObject = new object();
        /// <summary>
        /// The opened windows
        /// </summary>
        private readonly List<Window> _openedWindows = new List<Window>();
        /// <summary>
        /// Occurs when [closed].
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Occurs when [closing].
        /// </summary>
        public event CancelEventHandler Closing;

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool? ShowDialog(ViewModelBase viewModel)
        {
            lock (LockObject)
            {
                var window = GetWindow(viewModel, true);
                window.DataContext = viewModel;
                return window.ShowDialog();
            }
        }

        /// <summary>
        /// Shows the window.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public void ShowWindow(ViewModelBase viewModel)
        {
            lock (LockObject)
            {
                var window = GetWindow(viewModel, false);
                window.DataContext = viewModel;
                window.Show();
            }
        }

        /// <summary>
        /// Closes the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="dialogResult">if set to <c>true</c> [dialog result].</param>
        public void Close(ViewModelBase viewModel, bool dialogResult = false)
        {
            lock (LockObject)
            {
                var window = _openedWindows.SingleOrDefault(w => w.DataContext == viewModel);

                if (window != null)
                {
                    if (dialogResult) window.DialogResult = true;

                    _openedWindows.Remove(window);
                    window.Close();
                }
            }
        }

        /// <summary>
        /// Closes the open windows.
        /// </summary>
        public void CloseOpenWindows()
        {
            lock (LockObject)
            {
                if (_openedWindows.Any())
                {
                    var windowsToClose = _openedWindows.ToList();
                    windowsToClose.ForEach(x => x.Close());
                    _openedWindows.Clear();
                }
            }
        }

        /// <summary>
        /// Shows a message box with a message
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="messageBoxImage">The message box image.</param>
        /// <returns>MessageBoxResult.</returns>
        public MessageBoxResult ShowMessageBox(string message, string title = null,
            MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage messageBoxImage = MessageBoxImage.None)
        {
            return MessageBox.Show(message, title, buttons, messageBoxImage);
        }

        /// <summary>
        /// Shows the open file dialog
        /// </summary>
        /// <param name="initialDirectory">The initial directory.</param>
        /// <returns>System.String.</returns>
        public string ShowOpenFileDialog(string initialDirectory = null)
        {
            var dialog = new OpenFileDialog
            {
                InitialDirectory = initialDirectory,
                Multiselect = false,
                CheckFileExists = true,
                Filter = "Torrent files|*.torrent;"
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        /// <summary>
        /// Shows the open folder dialog
        /// </summary>
        /// <returns>Returns the path of the folder or null if canceled</returns>
        public string ShowFolderBrowserDialog()
        {
            using (var dialog = new CommonOpenFileDialog {IsFolderPicker = true})
            {
                return dialog.ShowDialog() == CommonFileDialogResult.Ok ? dialog.FileName : null;
            }
        }

        /// <summary>
        /// Attempts to show a particular file in Windows Explorer
        /// </summary>
        /// <param name="path">The full path of the file including its name</param>
        public void ShowFileInExplorer(string path)
        {
            Process.Start("explorer.exe", "/select," + path);
        }

        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="file">The file.</param>
        public void OpenFile(string file)
        {
            Process.Start(file);
        }

        /// <summary>
        /// Gets the window.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="isDialogWindow">if set to <c>true</c> [is dialog window].</param>
        /// <returns>Window.</returns>
        private Window GetWindow(ViewModelBase viewModel, bool isDialogWindow)
        {
            lock (LockObject)
            {
                var window = _openedWindows.SingleOrDefault(w => w.DataContext == viewModel);

                if (window != null)
                {
                    window.WindowState = WindowState.Normal;
                    window.Activate();
                    return window;
                }

                if (isDialogWindow)
                {
                    // Set window owner in order to prevent that it becomes hidden when minimizing the application
                    window = new DialogWindow
                    {
                        Owner = _openedWindows.Any() ? _openedWindows.Last() : Application.Current.MainWindow
                    };
                }
                else
                {
                    // Doesn't need a window owner since it's shown in the taskbar
                    window = new StandardWindow();
                }

                _openedWindows.Add(window);

                window.Closed += (sender, e) =>
                {
                    Closed?.Invoke(sender, e);

                    _openedWindows.Remove(window);
                    window = null;

                    if (!_openedWindows.Any() && Application.Current.MainWindow != null)
                    {
                        Application.Current.MainWindow.Activate();
                    }
                };

                window.Closing += (sender, e) =>
                {
                    Closing?.Invoke(sender, e);
                };
                return window;
            }
        }
    }
}
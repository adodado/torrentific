// ***********************************************************************
// Assembly         : Torrentific
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="IDialogService.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.ComponentModel;
using System.Windows;

namespace Torrentific.Infrastructure
{
    /// <summary>
    /// Interface IDialogService
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Shows a message box with a message
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="messageBoxImage">The message box image.</param>
        /// <returns>MessageBoxResult.</returns>
        MessageBoxResult ShowMessageBox(string message, string title = null,
            MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage messageBoxImage = MessageBoxImage.None);

        /// <summary>
        /// Shows the open file dialog
        /// </summary>
        /// <param name="initialDirectory">The initial directory.</param>
        /// <returns>System.String.</returns>
        string ShowOpenFileDialog(string initialDirectory = null);

        /// <summary>
        /// Shows the open folder dialog
        /// </summary>
        /// <returns>Returns the path of the folder or null if canceled</returns>
        string ShowFolderBrowserDialog();

        /// <summary>
        /// Attempts to show a particular file in Windows Explorer
        /// </summary>
        /// <param name="path">The full path of the file including its name</param>
        void ShowFileInExplorer(string path);

        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="selectedFileEntity">The selected file entity.</param>
        void OpenFile(string selectedFileEntity);

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool? ShowDialog(ViewModelBase viewModel);
        /// <summary>
        /// Shows the window.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        void ShowWindow(ViewModelBase viewModel);
        /// <summary>
        /// Closes the specified view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="dialogResult">if set to <c>true</c> [dialog result].</param>
        void Close(ViewModelBase viewModel, bool dialogResult = false);
        /// <summary>
        /// Occurs when [closing].
        /// </summary>
        event CancelEventHandler Closing;
        /// <summary>
        /// Occurs when [closed].
        /// </summary>
        event EventHandler Closed;
        /// <summary>
        /// Closes the open windows.
        /// </summary>
        void CloseOpenWindows();
    }
}
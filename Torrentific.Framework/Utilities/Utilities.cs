// ***********************************************************************
// Assembly         : Torrentific.Framework
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="Utilities.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Microsoft.Win32;

namespace Torrentific.Framework.Utilities
{
    /// <summary>
    /// Class Utilities.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Associates magnet links with this application
        /// </summary>
        public static void SetMagnetLinkAssociation()
        {
            var command = "\"" + Environment.CurrentDirectory + "\\Torrentific.exe\"" + " " + "\"%1\"";

            var rootKey = Registry.ClassesRoot;

            try
            {
                rootKey = rootKey.CreateSubKey("magnet");
                rootKey.SetValue("", "URL:Magnet link");
                rootKey.SetValue("Content Type", "application/x-magnet");
                rootKey.SetValue("URL Protocol", "");
                rootKey.CreateSubKey("DefaultIcon");
                rootKey = rootKey.CreateSubKey("shell");
                rootKey.SetValue("", "open");
                rootKey = rootKey.CreateSubKey("open");
                rootKey = rootKey.CreateSubKey("command");
                rootKey.SetValue("", command);
            }

            finally
            {
                rootKey.Close();
            }
        }
    }
}
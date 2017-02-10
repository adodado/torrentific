// ***********************************************************************
// Assembly         : Torrentific
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="ApplicationMessage.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;

namespace Torrentific.Infrastructure
{
    /// <summary>
    /// Enum MessageType
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// The command line message
        /// </summary>
        CommandLineMessage,
        /// <summary>
        /// The shutdown
        /// </summary>
        Shutdown,
        /// <summary>
        /// The add new torrent from search view
        /// </summary>
        AddNewTorrentFromSearchView
    }

    /// <summary>
    /// Class ApplicationMessage.
    /// </summary>
    public class ApplicationMessage
    {
        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>The type of the message.</value>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public IEnumerable<string> Arguments { get; set; }
    }
}
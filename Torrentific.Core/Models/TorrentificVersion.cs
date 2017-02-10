// ***********************************************************************
// Assembly         : Torrentific.Core
// Author           : Admir Cosic
// Created          : 02-10-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-10-2017
// ***********************************************************************
// <copyright file="TorrentificVersion.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Torrentific.Core.Models
{
    /// <summary>
    /// Class TorrentificVersion.
    /// </summary>
    public class TorrentificVersion
    {
        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName { get; set; }
        /// <summary>
        /// Gets or sets the assembly version.
        /// </summary>
        /// <value>The assembly version.</value>
        public string AssemblyVersion { get; set; }
    }
}

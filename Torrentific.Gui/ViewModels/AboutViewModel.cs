// ***********************************************************************
// Assembly         : Torrentific
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-10-2017
// ***********************************************************************
// <copyright file="AboutViewModel.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Torrentific.Core.Models;
using Torrentific.Infrastructure;

namespace Torrentific.ViewModels
{
    /// <summary>
    /// Class AboutViewModel. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="Torrentific.Infrastructure.ViewModelBase" />
    public sealed class AboutViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutViewModel" /> class.
        /// </summary>
        public AboutViewModel()
        {
            DisplayName = "About";

            Name = "Torrentific.Gui - Bittorrent client for Windows";
            SubHeading = "Torrentific assembly versions:";
            Description =
                "This is a lightweight torrent client, currently the magnetlink conversion and the Piratebay search engine can sometimes malfunction. This is because of the DDOS protection on the services used.";
            Version = GetProductVersion();
        }

        private IEnumerable<Assembly> GetAssemblyVersions()
        {
            var list = new List<string>();
            var stack = new Stack<Assembly>();
            stack.Push(Assembly.GetEntryAssembly());
            do
            {
                var asm = stack.Pop();
                yield return asm;

                foreach (var reference in asm.GetReferencedAssemblies())
                    if (!list.Contains(reference.FullName))
                    {
                        stack.Push(Assembly.Load(reference));
                        list.Add(reference.FullName);
                    }

            }
            while (stack.Count > 0);
        }

        public List<TorrentificVersion> GetProductVersion()
        {
            return (from assembly in GetAssemblyVersions()
                select FileVersionInfo.GetVersionInfo(assembly.Location)
                into fvi
                where fvi != null && !string.IsNullOrEmpty(fvi.FileVersion) && !string.IsNullOrEmpty(fvi.OriginalFilename)
                select new TorrentificVersion
                {
                    AssemblyVersion = fvi.FileVersion, FullName = fvi.OriginalFilename
                }).ToList();
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        public string SubHeading { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public IEnumerable<TorrentificVersion> Version { get; set; }
    }
}
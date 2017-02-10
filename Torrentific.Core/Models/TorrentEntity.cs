// ***********************************************************************
// Assembly         : Torrentific.Core
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="TorrentEntity.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Ragnar;
using Torrentific.Core.Annotations;
using Torrentific.Core.Data;
using Torrentific.Core.Enums;

namespace Torrentific.Core.Models
{
    /// <summary>
    /// Class TorrentEntity.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// <seealso cref="Torrentific.Core.Data.IEntity" />
    public class TorrentEntity : INotifyPropertyChanged, IEntity
    {
        /// <summary>
        /// The basic torrent state
        /// </summary>
        private TorrentBaseState _basicTorrentState;
        /// <summary>
        /// The date added
        /// </summary>
        private string _dateAdded;
        /// <summary>
        /// Down speed
        /// </summary>
        private string _downSpeed;
        /// <summary>
        /// The eta
        /// </summary>
        private string _eta;
        /// <summary>
        /// The name
        /// </summary>
        private string _name;
        /// <summary>
        /// The peers
        /// </summary>
        private int _peers;
        /// <summary>
        /// The progress
        /// </summary>
        private double _progress;
        /// <summary>
        /// The save path
        /// </summary>
        private string _savePath;
        /// <summary>
        /// The seeds
        /// </summary>
        private int _seeds;
        /// <summary>
        /// The size
        /// </summary>
        private string _size;
        /// <summary>
        /// The state
        /// </summary>
        private string _state;
        /// <summary>
        /// Up speed
        /// </summary>
        private string _upSpeed;

        /// <summary>
        /// Gets or sets the torrent files.
        /// </summary>
        /// <value>The torrent files.</value>
        public ObservableCollection<TorrentFileEntity> TorrentFiles { get; set; }

        /// <summary>
        /// Gets or sets the torrent metadata.
        /// </summary>
        /// <value>The torrent metadata.</value>
        public byte[] TorrentMetadata { get; set; }

        /// <summary>
        /// Gets or sets the torrent URI.
        /// </summary>
        /// <value>The torrent URI.</value>
        public string TorrentUri { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has finished once.
        /// </summary>
        /// <value><c>true</c> if this instance has finished once; otherwise, <c>false</c>.</value>
        public bool HasFinishedOnce { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the save path.
        /// </summary>
        /// <value>The save path.</value>
        public string SavePath
        {
            get { return _savePath; }
            set
            {
                _savePath = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the date added.
        /// </summary>
        /// <value>The date added.</value>
        public string DateAdded
        {
            get { return _dateAdded; }
            set
            {
                _dateAdded = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public string State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the progress.
        /// </summary>
        /// <value>The progress.</value>
        public double Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public string Size
        {
            get { return _size; }
            set
            {
                _size = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the state of the basic torrent.
        /// </summary>
        /// <value>The state of the basic torrent.</value>
        public TorrentBaseState BasicTorrentState
        {
            get { return _basicTorrentState; }
            set
            {
                _basicTorrentState = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets down speed.
        /// </summary>
        /// <value>Down speed.</value>
        public string DownSpeed
        {
            get { return _downSpeed; }
            set
            {
                _downSpeed = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets up speed.
        /// </summary>
        /// <value>Up speed.</value>
        public string UpSpeed
        {
            get { return _upSpeed; }
            set
            {
                _upSpeed = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the eta.
        /// </summary>
        /// <value>The eta.</value>
        public string Eta
        {
            get { return _eta; }
            set
            {
                _eta = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the seeds.
        /// </summary>
        /// <value>The seeds.</value>
        public int Seeds
        {
            get { return _seeds; }
            set
            {
                _seeds = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the peers.
        /// </summary>
        /// <value>The peers.</value>
        public int Peers
        {
            get { return _peers; }
            set
            {
                _peers = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the torrent handle.
        /// </summary>
        /// <value>The torrent handle.</value>
        [XmlIgnore]
        public TorrentHandle TorrentHandle { get; set; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
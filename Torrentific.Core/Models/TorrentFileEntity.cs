// ***********************************************************************
// Assembly         : Torrentific.Core
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="TorrentFileEntity.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Torrentific.Core.Annotations;
using Torrentific.Core.Data;
using Torrentific.Core.Enums;

namespace Torrentific.Core.Models
{
    /// <summary>
    /// Class TorrentFileEntity.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// <seealso cref="Torrentific.Core.Data.IEntity" />
    public class TorrentFileEntity : INotifyPropertyChanged, IEntity
    {
        /// <summary>
        /// The is selected
        /// </summary>
        private bool _isSelected;
        /// <summary>
        /// The name
        /// </summary>
        private string _name;
        /// <summary>
        /// The priority
        /// </summary>
        private TorrentPriority _priority;
        /// <summary>
        /// The progress
        /// </summary>
        private double _progress;
        /// <summary>
        /// The progress text
        /// </summary>
        private string _progressText;
        /// <summary>
        /// The size
        /// </summary>
        private string _size;

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>The length.</value>
        public long Length { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>The priority.</value>
        public TorrentPriority Priority
        {
            get { return _priority; }
            set
            {
                _priority = value;
                OnPropertyChanged();
            }
        }

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
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
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
        /// Gets or sets the progress text.
        /// </summary>
        /// <value>The progress text.</value>
        public string ProgressText
        {
            get { return _progressText; }
            set
            {
                _progressText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the priority text.
        /// </summary>
        /// <value>The priority text.</value>
        public string PriorityText
        {
            get { return _priorityText; }
            set
            {
                _priorityText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }

        #region INotifyPropertyChanged Support

        /// <summary>
        /// The priority text
        /// </summary>
        private string _priorityText;
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
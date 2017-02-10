// ***********************************************************************
// Assembly         : Torrentific
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="ViewModelBase.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Torrentific.Core.Annotations;

namespace Torrentific.Infrastructure
{
    /// <summary>
    /// Defines a base class for all view models
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// The display name
        /// </summary>
        private string _displayName;
        /// <summary>
        /// The is working
        /// </summary>
        private bool _isWorking;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is working.
        /// </summary>
        /// <value><c>true</c> if this instance is working; otherwise, <c>false</c>.</value>
        public bool IsWorking
        {
            get { return _isWorking; }
            set
            {
                _isWorking = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                OnPropertyChanged();
            }
        }

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
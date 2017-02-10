// ***********************************************************************
// Assembly         : Torrentific
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="IMessageService.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace Torrentific.Infrastructure
{
    /// <summary>
    /// Interface IMessageService
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Registers the specified action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The action.</param>
        void Register<T>(Action<T> action);
        /// <summary>
        /// Unregisters the specified action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The action.</param>
        void Unregister<T>(Action<T> action);
        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        void Send<T>(T message);
    }
}
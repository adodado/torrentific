// ***********************************************************************
// Assembly         : Torrentific
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="MessageService.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Torrentific.Infrastructure
{
    /// <summary>
    /// Class MessageService.
    /// </summary>
    /// <seealso cref="Torrentific.Infrastructure.IMessageService" />
    public class MessageService : IMessageService
    {
        /// <summary>
        /// The subscribers
        /// </summary>
        private readonly Dictionary<Type, List<object>> _subscribers;
        /// <summary>
        /// The synchronize
        /// </summary>
        private readonly object _sync;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageService"/> class.
        /// </summary>
        public MessageService()
        {
            _subscribers = new Dictionary<Type, List<object>>();
            _sync = new object();
        }

        /// <summary>
        /// Registers the specified action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The action.</param>
        public void Register<T>(Action<T> action)
        {
            lock (_sync)
            {
                if (_subscribers.ContainsKey(typeof(T)))
                {
                    var actions = _subscribers[typeof(T)];
                    actions.Add(action);
                }
                else
                {
                    var actions = new List<object> {action};
                    _subscribers.Add(typeof(T), actions);
                }
            }
        }

        /// <summary>
        /// Unregisters the specified action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The action.</param>
        public void Unregister<T>(Action<T> action)
        {
            lock (_sync)
            {
                if (!_subscribers.ContainsKey(typeof(T))) return;

                var actions = _subscribers[typeof(T)];
                actions.Remove(action);
                if (actions.Count == 0)
                {
                    _subscribers.Remove(typeof(T));
                }
            }
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        public void Send<T>(T message)
        {
            lock (_sync)
            {
                if (!_subscribers.ContainsKey(typeof(T))) return;

                var actions = _subscribers[typeof(T)];
                foreach (Action<T> action in actions)
                {
                    action.Invoke(message);
                }
            }
        }
    }
}
// ***********************************************************************
// Assembly         : Torrentific.Core
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="DataRepository.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;

namespace Torrentific.Core.Data
{
    /// <summary>
    /// Class DataRepository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Torrentific.Core.Data.IDataRepository{T}" />
    public class DataRepository<T> : IDataRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// The entities
        /// </summary>
        private List<T> _entities;

        /// <summary>
        /// Initializes the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void Initialize(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath);
                }

                var serializer = new XmlSerializer(typeof(List<T>));
                using (var reader = new StreamReader(filePath))
                {
                    _entities = (List<T>) serializer.Deserialize(reader);
                }
            }
            catch (Exception)
            {
                _entities = new List<T>();
            }
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>T.</returns>
        public T GetById(Guid id)
        {
            return (from c in _entities where c.Id == id select c).Single();
        }

        /// <summary>
        /// Inserts the or update.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void InsertOrUpdate(T entity)
        {
            var existing = _entities.FirstOrDefault(x => x.Id == entity.Id);

            if (existing != null)
            {
                _entities[_entities.IndexOf(existing)] = entity;
            }
            else
            {
                _entities.Add(entity);
            }
        }

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="appDataFolder">The application data folder.</param>
        public void SaveChanges(string filePath, string appDataFolder)
        {
            if (!Directory.Exists(appDataFolder))
            {
                Directory.CreateDirectory(appDataFolder);
            }

            var serializer = new XmlSerializer(_entities.GetType());
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, _entities);
            }
        }

        /// <summary>
        /// Searches for.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>IQueryable&lt;T&gt;.</returns>
        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return null;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>List&lt;T&gt;.</returns>
        public List<T> GetAll()
        {
            return _entities;
        }
    }
}
// ***********************************************************************
// Assembly         : Torrentific.Core
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="IDataRepository.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Torrentific.Core.Data
{
    /// <summary>
    /// Interface IDataRepository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataRepository<T>
    {
        /// <summary>
        /// Initializes the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        void Initialize(string filePath);
        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="appDataFolder">The application data folder.</param>
        void SaveChanges(string filePath, string appDataFolder);
        /// <summary>
        /// Inserts the or update.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void InsertOrUpdate(T entity);
        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Remove(T entity);
        /// <summary>
        /// Searches for.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>IQueryable&lt;T&gt;.</returns>
        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>List&lt;T&gt;.</returns>
        List<T> GetAll();
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>T.</returns>
        T GetById(Guid id);
    }
}
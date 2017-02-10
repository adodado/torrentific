// ***********************************************************************
// Assembly         : Torrentific
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="ServiceManager.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Ragnar;
using Torrentific.Core.Data;
using Torrentific.Core.Models;
using Torrentific.Framework.Services;
using Torrentific.ViewModels;

namespace Torrentific.Infrastructure
{
    /// <summary>
    /// Class ServiceManager.
    /// </summary>
    /// <seealso cref="Torrentific.Infrastructure.IServiceManager" />
    public class ServiceManager : IServiceManager
    {
        /// <summary>
        /// The container
        /// </summary>
        private readonly IWindsorContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceManager"/> class.
        /// </summary>
        public ServiceManager()
        {
            _container = new WindsorContainer();

            _container.Register(
                Component.For<IServiceManager>().Instance(this),
                Component.For<IDataRepository<TorrentEntity>>().ImplementedBy<DataRepository<TorrentEntity>>(),
                Component.For<IDataRepository<ApplicationSettings>>()
                    .ImplementedBy<DataRepository<ApplicationSettings>>(),
                Component.For<SettingsViewModel>(),
                Component.For<AddNewTorrentViewModel>(),
                Component.For<ManualDownloadViewModel>(),
                Component.For<SearchViewModel>(),
                Component.For<IMessageService>().ImplementedBy<MessageService>(),
                Component.For<IAppSettingsService>().ImplementedBy<AppSettingsService>(),
                Component.For<IMetadataService>().ImplementedBy<MetadataService>(),
                Component.For<ITorrentService>().ImplementedBy<TorrentService>(),
                Component.For<ISearchService>().ImplementedBy<SearchService>(),
                Component.For<IDialogService>().ImplementedBy<DialogService>(),
                Component.For<ISession>().ImplementedBy<Session>(),
                Component.For<MainViewModel>().OnCreate(x => x.Initialize())
                );
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        public T Get<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
// ***********************************************************************
// Assembly         : Torrentific.Gui
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="App.xaml.cs" company="None">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Torrentific.Infrastructure;
using Torrentific.ViewModels;
using Torrentific.Windows;

namespace Torrentific
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Application" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    /// <seealso cref="ISingleInstanceApp" />
    public partial class App : ISingleInstanceApp
    {
        /// <summary>
        /// The unique
        /// </summary>
        private const string Unique = "torrentific-F5FFFE3F-4899-48CB-BFBD-2A533B6B4B9A";

        /// <summary>
        /// The service manager
        /// </summary>
        private IServiceManager _serviceManager;

        /// <summary>
        /// Occurs when the application is being launched but an instance of it is already running
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            // Bring main window to foreground
            if (MainWindow.WindowState == WindowState.Minimized)
            {
                MainWindow.WindowState = WindowState.Normal;
            }

            MainWindow.Activate();

            // Publish command line messages 
            var messenger = _serviceManager.Get<IMessageService>();
            messenger.Send(new ApplicationMessage
            {
                Arguments = args.ToArray(),
                MessageType = MessageType.CommandLineMessage
            });

            return true;
        }

        /// <summary>
        /// Application entry point
        /// </summary>
        [STAThread]
        [DebuggerNonUserCode]
        public static void Main()
        {
            // Single instance application
            if (!SingleInstance<App>.InitializeAsFirstInstance(Unique)) return;
            var app = new App();
            app.InitializeComponent();
            app.Run();

            // Allow single instance code to perform cleanup operations
            SingleInstance<App>.Cleanup();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            _serviceManager = new ServiceManager();

            var mainWindow = new MainWindow {DataContext = _serviceManager.Get<MainViewModel>()};

            mainWindow.Closing += (sender, eventArgs) =>
            {
                var messenger = _serviceManager.Get<IMessageService>();
                messenger.Send(new ApplicationMessage
                {
                    MessageType = MessageType.Shutdown
                });
            };

            mainWindow.Show();
        }
    }
}
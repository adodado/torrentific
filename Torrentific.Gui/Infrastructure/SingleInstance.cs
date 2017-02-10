// ***********************************************************************
// Assembly         : Torrentific
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="SingleInstance.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Torrentific.Infrastructure
{
    /// <summary>
    /// Enum Wm
    /// </summary>
    internal enum Wm
    {
        /// <summary>
        /// The null
        /// </summary>
        Null = 0x0000,
        /// <summary>
        /// The create
        /// </summary>
        Create = 0x0001,
        /// <summary>
        /// The destroy
        /// </summary>
        Destroy = 0x0002,
        /// <summary>
        /// The move
        /// </summary>
        Move = 0x0003,
        /// <summary>
        /// The size
        /// </summary>
        Size = 0x0005,
        /// <summary>
        /// The activate
        /// </summary>
        Activate = 0x0006,
        /// <summary>
        /// The setfocus
        /// </summary>
        Setfocus = 0x0007,
        /// <summary>
        /// The killfocus
        /// </summary>
        Killfocus = 0x0008,
        /// <summary>
        /// The enable
        /// </summary>
        Enable = 0x000A,
        /// <summary>
        /// The setredraw
        /// </summary>
        Setredraw = 0x000B,
        /// <summary>
        /// The settext
        /// </summary>
        Settext = 0x000C,
        /// <summary>
        /// The gettext
        /// </summary>
        Gettext = 0x000D,
        /// <summary>
        /// The gettextlength
        /// </summary>
        Gettextlength = 0x000E,
        /// <summary>
        /// The paint
        /// </summary>
        Paint = 0x000F,
        /// <summary>
        /// The close
        /// </summary>
        Close = 0x0010,
        /// <summary>
        /// The queryendsession
        /// </summary>
        Queryendsession = 0x0011,
        /// <summary>
        /// The quit
        /// </summary>
        Quit = 0x0012,
        /// <summary>
        /// The queryopen
        /// </summary>
        Queryopen = 0x0013,
        /// <summary>
        /// The erasebkgnd
        /// </summary>
        Erasebkgnd = 0x0014,
        /// <summary>
        /// The syscolorchange
        /// </summary>
        Syscolorchange = 0x0015,
        /// <summary>
        /// The showwindow
        /// </summary>
        Showwindow = 0x0018,
        /// <summary>
        /// The activateapp
        /// </summary>
        Activateapp = 0x001C,
        /// <summary>
        /// The setcursor
        /// </summary>
        Setcursor = 0x0020,
        /// <summary>
        /// The mouseactivate
        /// </summary>
        Mouseactivate = 0x0021,
        /// <summary>
        /// The childactivate
        /// </summary>
        Childactivate = 0x0022,
        /// <summary>
        /// The queuesync
        /// </summary>
        Queuesync = 0x0023,
        /// <summary>
        /// The getminmaxinfo
        /// </summary>
        Getminmaxinfo = 0x0024,

        /// <summary>
        /// The windowposchanging
        /// </summary>
        Windowposchanging = 0x0046,
        /// <summary>
        /// The windowposchanged
        /// </summary>
        Windowposchanged = 0x0047,

        /// <summary>
        /// The contextmenu
        /// </summary>
        Contextmenu = 0x007B,
        /// <summary>
        /// The stylechanging
        /// </summary>
        Stylechanging = 0x007C,
        /// <summary>
        /// The stylechanged
        /// </summary>
        Stylechanged = 0x007D,
        /// <summary>
        /// The displaychange
        /// </summary>
        Displaychange = 0x007E,
        /// <summary>
        /// The geticon
        /// </summary>
        Geticon = 0x007F,
        /// <summary>
        /// The seticon
        /// </summary>
        Seticon = 0x0080,
        /// <summary>
        /// The nccreate
        /// </summary>
        Nccreate = 0x0081,
        /// <summary>
        /// The ncdestroy
        /// </summary>
        Ncdestroy = 0x0082,
        /// <summary>
        /// The nccalcsize
        /// </summary>
        Nccalcsize = 0x0083,
        /// <summary>
        /// The nchittest
        /// </summary>
        Nchittest = 0x0084,
        /// <summary>
        /// The ncpaint
        /// </summary>
        Ncpaint = 0x0085,
        /// <summary>
        /// The ncactivate
        /// </summary>
        Ncactivate = 0x0086,
        /// <summary>
        /// The getdlgcode
        /// </summary>
        Getdlgcode = 0x0087,
        /// <summary>
        /// The syncpaint
        /// </summary>
        Syncpaint = 0x0088,
        /// <summary>
        /// The ncmousemove
        /// </summary>
        Ncmousemove = 0x00A0,
        /// <summary>
        /// The nclbuttondown
        /// </summary>
        Nclbuttondown = 0x00A1,
        /// <summary>
        /// The nclbuttonup
        /// </summary>
        Nclbuttonup = 0x00A2,
        /// <summary>
        /// The nclbuttondblclk
        /// </summary>
        Nclbuttondblclk = 0x00A3,
        /// <summary>
        /// The ncrbuttondown
        /// </summary>
        Ncrbuttondown = 0x00A4,
        /// <summary>
        /// The ncrbuttonup
        /// </summary>
        Ncrbuttonup = 0x00A5,
        /// <summary>
        /// The ncrbuttondblclk
        /// </summary>
        Ncrbuttondblclk = 0x00A6,
        /// <summary>
        /// The ncmbuttondown
        /// </summary>
        Ncmbuttondown = 0x00A7,
        /// <summary>
        /// The ncmbuttonup
        /// </summary>
        Ncmbuttonup = 0x00A8,
        /// <summary>
        /// The ncmbuttondblclk
        /// </summary>
        Ncmbuttondblclk = 0x00A9,

        /// <summary>
        /// The syskeydown
        /// </summary>
        Syskeydown = 0x0104,
        /// <summary>
        /// The syskeyup
        /// </summary>
        Syskeyup = 0x0105,
        /// <summary>
        /// The syschar
        /// </summary>
        Syschar = 0x0106,
        /// <summary>
        /// The sysdeadchar
        /// </summary>
        Sysdeadchar = 0x0107,
        /// <summary>
        /// The command
        /// </summary>
        Command = 0x0111,
        /// <summary>
        /// The syscommand
        /// </summary>
        Syscommand = 0x0112,

        /// <summary>
        /// The mousemove
        /// </summary>
        Mousemove = 0x0200,
        /// <summary>
        /// The lbuttondown
        /// </summary>
        Lbuttondown = 0x0201,
        /// <summary>
        /// The lbuttonup
        /// </summary>
        Lbuttonup = 0x0202,
        /// <summary>
        /// The lbuttondblclk
        /// </summary>
        Lbuttondblclk = 0x0203,
        /// <summary>
        /// The rbuttondown
        /// </summary>
        Rbuttondown = 0x0204,
        /// <summary>
        /// The rbuttonup
        /// </summary>
        Rbuttonup = 0x0205,
        /// <summary>
        /// The rbuttondblclk
        /// </summary>
        Rbuttondblclk = 0x0206,
        /// <summary>
        /// The mbuttondown
        /// </summary>
        Mbuttondown = 0x0207,
        /// <summary>
        /// The mbuttonup
        /// </summary>
        Mbuttonup = 0x0208,
        /// <summary>
        /// The mbuttondblclk
        /// </summary>
        Mbuttondblclk = 0x0209,
        /// <summary>
        /// The mousewheel
        /// </summary>
        Mousewheel = 0x020A,
        /// <summary>
        /// The xbuttondown
        /// </summary>
        Xbuttondown = 0x020B,
        /// <summary>
        /// The xbuttonup
        /// </summary>
        Xbuttonup = 0x020C,
        /// <summary>
        /// The xbuttondblclk
        /// </summary>
        Xbuttondblclk = 0x020D,
        /// <summary>
        /// The mousehwheel
        /// </summary>
        Mousehwheel = 0x020E,


        /// <summary>
        /// The capturechanged
        /// </summary>
        Capturechanged = 0x0215,

        /// <summary>
        /// The entersizemove
        /// </summary>
        Entersizemove = 0x0231,
        /// <summary>
        /// The exitsizemove
        /// </summary>
        Exitsizemove = 0x0232,

        /// <summary>
        /// The IME setcontext
        /// </summary>
        ImeSetcontext = 0x0281,
        /// <summary>
        /// The IME notify
        /// </summary>
        ImeNotify = 0x0282,
        /// <summary>
        /// The IME control
        /// </summary>
        ImeControl = 0x0283,
        /// <summary>
        /// The IME compositionfull
        /// </summary>
        ImeCompositionfull = 0x0284,
        /// <summary>
        /// The IME select
        /// </summary>
        ImeSelect = 0x0285,
        /// <summary>
        /// The IME character
        /// </summary>
        ImeChar = 0x0286,
        /// <summary>
        /// The IME request
        /// </summary>
        ImeRequest = 0x0288,
        /// <summary>
        /// The IME keydown
        /// </summary>
        ImeKeydown = 0x0290,
        /// <summary>
        /// The IME keyup
        /// </summary>
        ImeKeyup = 0x0291,

        /// <summary>
        /// The ncmouseleave
        /// </summary>
        Ncmouseleave = 0x02A2,

        /// <summary>
        /// The dwmcompositionchanged
        /// </summary>
        Dwmcompositionchanged = 0x031E,
        /// <summary>
        /// The dwmncrenderingchanged
        /// </summary>
        Dwmncrenderingchanged = 0x031F,
        /// <summary>
        /// The dwmcolorizationcolorchanged
        /// </summary>
        Dwmcolorizationcolorchanged = 0x0320,
        /// <summary>
        /// The dwmwindowmaximizedchange
        /// </summary>
        Dwmwindowmaximizedchange = 0x0321,

        #region Windows 7

        /// <summary>
        /// The dwmsendiconicthumbnail
        /// </summary>
        Dwmsendiconicthumbnail = 0x0323,
        /// <summary>
        /// The dwmsendiconiclivepreviewbitmap
        /// </summary>
        Dwmsendiconiclivepreviewbitmap = 0x0326,

        #endregion

        /// <summary>
        /// The user
        /// </summary>
        User = 0x0400,

        // This is the hard-coded message value used by WinForms for Shell_NotifyIcon.
        // It's relatively safe to reuse.
        /// <summary>
        /// The traymousemessage
        /// </summary>
        Traymousemessage = 0x800, //WM_USER + 1024
        /// <summary>
        /// The application
        /// </summary>
        App = 0x8000
    }

    /// <summary>
    /// Class NativeMethods.
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    internal static class NativeMethods
    {
        /// <summary>
        /// Delegate declaration that matches WndProc signatures.
        /// </summary>
        /// <param name="uMsg">The u MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <param name="handled">if set to <c>true</c> [handled].</param>
        /// <returns>IntPtr.</returns>
        public delegate IntPtr MessageHandler(Wm uMsg, IntPtr wParam, IntPtr lParam, out bool handled);

        /// <summary>
        /// Commands the line to argv w.
        /// </summary>
        /// <param name="cmdLine">The command line.</param>
        /// <param name="numArgs">The number arguments.</param>
        /// <returns>IntPtr.</returns>
        [DllImport("shell32.dll", EntryPoint = "CommandLineToArgvW", CharSet = CharSet.Unicode)]
        private static extern IntPtr _CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string cmdLine,
            out int numArgs);


        /// <summary>
        /// Locals the free.
        /// </summary>
        /// <param name="hMem">The h memory.</param>
        /// <returns>IntPtr.</returns>
        [DllImport("kernel32.dll", EntryPoint = "LocalFree", SetLastError = true)]
        private static extern IntPtr _LocalFree(IntPtr hMem);


        /// <summary>
        /// Commands the line to argv w.
        /// </summary>
        /// <param name="cmdLine">The command line.</param>
        /// <returns>System.String[].</returns>
        /// <exception cref="System.ComponentModel.Win32Exception"></exception>
        public static string[] CommandLineToArgvW(string cmdLine)
        {
            var argv = IntPtr.Zero;
            try
            {
                var numArgs = 0;

                argv = _CommandLineToArgvW(cmdLine, out numArgs);
                if (argv == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }
                var result = new string[numArgs];

                for (var i = 0; i < numArgs; i++)
                {
                    var currArg = Marshal.ReadIntPtr(argv, i*Marshal.SizeOf(typeof(IntPtr)));
                    result[i] = Marshal.PtrToStringUni(currArg);
                }

                return result;
            }
            finally
            {
                var p = _LocalFree(argv);
                // Otherwise LocalFree failed.
                // Assert.AreEqual(IntPtr.Zero, p);
            }
        }
    }

    /// <summary>
    /// Interface ISingleInstanceApp
    /// </summary>
    public interface ISingleInstanceApp
    {
        /// <summary>
        /// Signals the external command line arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool SignalExternalCommandLineArgs(IList<string> args);
    }

    /// <summary>
    /// This class checks to make sure that only one instance of
    /// this application is running at a time.
    /// </summary>
    /// <typeparam name="TApplication">The type of the t application.</typeparam>
    /// <remarks>Note: this class should be used with some caution, because it does no
    /// security checking. For example, if one instance of an app that uses this class
    /// is running as Administrator, any other instance, even if it is not
    /// running as Administrator, can activate it with command line arguments.
    /// For most apps, this will not be much of an issue.</remarks>
    public static class SingleInstance<TApplication>
        where TApplication : Application, ISingleInstanceApp
    {
        #region Public Properties

        /// <summary>
        /// Gets list of command line arguments for the application.
        /// </summary>
        /// <value>The command line arguments.</value>
        public static IList<string> CommandLineArgs { get; private set; }

        #endregion

        #region Private Classes

        /// <summary>
        /// Remoting service class which is exposed by the server i.e the first instance and called by the second instance
        /// to pass on the command line arguments to the first instance and cause it to activate itself.
        /// </summary>
        /// <seealso cref="System.MarshalByRefObject" />
        private class IpcRemoteService : MarshalByRefObject
        {
            /// <summary>
            /// Activates the first instance of the application.
            /// </summary>
            /// <param name="args">List of arguments to pass to the first instance.</param>
            public void InvokeFirstInstance(IList<string> args)
            {
                if (Application.Current != null)
                {
                    // Do an asynchronous call to ActivateFirstInstance function
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        new DispatcherOperationCallback(ActivateFirstInstanceCallback), args);
                }
            }

            /// <summary>
            /// Remoting Object's ease expires after every 5 minutes by default. We need to override the InitializeLifetimeService
            /// class
            /// to ensure that lease never expires.
            /// </summary>
            /// <returns>Always null.</returns>
            /// <PermissionSet>
            ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="RemotingConfiguration, Infrastructure" />
            /// </PermissionSet>
            public override object InitializeLifetimeService()
            {
                return null;
            }
        }

        #endregion

        #region Private Fields

        /// <summary>
        /// String delimiter used in channel names.
        /// </summary>
        private const string Delimiter = ":";

        /// <summary>
        /// Suffix to the channel name.
        /// </summary>
        private const string ChannelNameSuffix = "SingeInstanceIPCChannel";

        /// <summary>
        /// Remote service name.
        /// </summary>
        private const string RemoteServiceName = "SingleInstanceApplicationService";

        /// <summary>
        /// IPC protocol used (string).
        /// </summary>
        private const string IpcProtocol = "ipc://";

        /// <summary>
        /// Application mutex.
        /// </summary>
        private static Mutex _singleInstanceMutex;

        /// <summary>
        /// IPC channel for communications.
        /// </summary>
        private static IpcServerChannel _channel;

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if the instance of the application attempting to start is the first instance.
        /// If not, activates the first instance.
        /// </summary>
        /// <param name="uniqueName">Name of the unique.</param>
        /// <returns>True if this is the first instance of the application.</returns>
        public static bool InitializeAsFirstInstance(string uniqueName)
        {
            CommandLineArgs = GetCommandLineArgs(uniqueName);

            // Build unique application Id and the IPC channel name.
            var applicationIdentifier = uniqueName + Environment.UserName;

            var channelName = string.Concat(applicationIdentifier, Delimiter, ChannelNameSuffix);

            // Create mutex based on unique application Id to check if this is the first instance of the application. 
            bool firstInstance;
            _singleInstanceMutex = new Mutex(true, applicationIdentifier, out firstInstance);
            if (firstInstance)
            {
                CreateRemoteService(channelName);
            }
            else
            {
                SignalFirstInstance(channelName, CommandLineArgs);
            }

            return firstInstance;
        }

        /// <summary>
        /// Cleans up single-instance code, clearing shared resources, mutexes, etc.
        /// </summary>
        public static void Cleanup()
        {
            if (_singleInstanceMutex != null)
            {
                _singleInstanceMutex.Close();
                _singleInstanceMutex = null;
            }

            if (_channel != null)
            {
                ChannelServices.UnregisterChannel(_channel);
                _channel = null;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets command line args - for ClickOnce deployed applications, command line args may not be passed directly, they
        /// have to be retrieved.
        /// </summary>
        /// <param name="uniqueApplicationName">Name of the unique application.</param>
        /// <returns>List of command line arg strings.</returns>
        private static IList<string> GetCommandLineArgs(string uniqueApplicationName)
        {
            string[] args = null;
            if (AppDomain.CurrentDomain.ActivationContext == null)
            {
                // The application was not clickonce deployed, get args from standard API's
                args = Environment.GetCommandLineArgs();
            }
            else
            {
                // The application was clickonce deployed
                // Clickonce deployed apps cannot recieve traditional commandline arguments
                // As a workaround commandline arguments can be written to a shared location before 
                // the app is launched and the app can obtain its commandline arguments from the 
                // shared location               
                var appFolderPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), uniqueApplicationName);

                var cmdLinePath = Path.Combine(appFolderPath, "cmdline.txt");
                if (File.Exists(cmdLinePath))
                {
                    try
                    {
                        using (TextReader reader = new StreamReader(cmdLinePath, Encoding.Unicode))
                        {
                            args = NativeMethods.CommandLineToArgvW(reader.ReadToEnd());
                        }

                        File.Delete(cmdLinePath);
                    }
                    catch (IOException)
                    {
                    }
                }
            }

            if (args == null)
            {
                args = new string[] {};
            }

            return new List<string>(args);
        }

        /// <summary>
        /// Creates a remote service for communication.
        /// </summary>
        /// <param name="channelName">Application's IPC channel name.</param>
        private static void CreateRemoteService(string channelName)
        {
            var serverProvider = new BinaryServerFormatterSinkProvider
            {
                TypeFilterLevel = TypeFilterLevel.Full
            };

            IDictionary props = new Dictionary<string, string>();

            props["name"] = channelName;
            props["portName"] = channelName;
            props["exclusiveAddressUse"] = "false";

            // Create the IPC Server channel with the channel properties
            _channel = new IpcServerChannel(props, serverProvider);

            // Register the channel with the channel services
            ChannelServices.RegisterChannel(_channel, true);

            // Expose the remote service with the REMOTE_SERVICE_NAME
            var remoteService = new IpcRemoteService();
            RemotingServices.Marshal(remoteService, RemoteServiceName);
        }

        /// <summary>
        /// Creates a client channel and obtains a reference to the remoting service exposed by the server -
        /// in this case, the remoting service exposed by the first instance. Calls a function of the remoting service
        /// class to pass on command line arguments from the second instance to the first and cause it to activate itself.
        /// </summary>
        /// <param name="channelName">Application's IPC channel name.</param>
        /// <param name="args">Command line arguments for the second instance, passed to the first instance to take appropriate action.</param>
        private static void SignalFirstInstance(string channelName, IList<string> args)
        {
            var secondInstanceChannel = new IpcClientChannel();
            ChannelServices.RegisterChannel(secondInstanceChannel, true);

            var remotingServiceUrl = IpcProtocol + channelName + "/" + RemoteServiceName;

            // Obtain a reference to the remoting service exposed by the server i.e the first instance of the application
            var firstInstanceRemoteServiceReference =
                (IpcRemoteService) RemotingServices.Connect(typeof(IpcRemoteService), remotingServiceUrl);

            // Check that the remote service exists, in some cases the first instance may not yet have created one, in which case
            // the second instance should just exit
            if (firstInstanceRemoteServiceReference != null)
            {
                // Invoke a method of the remote service exposed by the first instance passing on the command line
                // arguments and causing the first instance to activate itself
                firstInstanceRemoteServiceReference.InvokeFirstInstance(args);
            }
        }

        /// <summary>
        /// Callback for activating first instance of the application.
        /// </summary>
        /// <param name="arg">Callback argument.</param>
        /// <returns>Always null.</returns>
        private static object ActivateFirstInstanceCallback(object arg)
        {
            // Get command line args to be passed to first instance
            var args = arg as IList<string>;
            ActivateFirstInstance(args);
            return null;
        }

        /// <summary>
        /// Activates the first instance of the application with arguments from a second instance.
        /// </summary>
        /// <param name="args">List of arguments to supply the first instance of the application.</param>
        private static void ActivateFirstInstance(IList<string> args)
        {
            // Set main window state and process command line args
            if (Application.Current == null)
            {
                return;
            }

            ((TApplication) Application.Current).SignalExternalCommandLineArgs(args);
        }

        #endregion
    }
}
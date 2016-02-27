using EasyBike.Models.Storage;
using EasyBike.WinPhone.Common;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace EasyBike.WinPhone
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Bootstrapper.Start();
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            UnhandledException += App_UnhandledException;
        }

        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // this can happen when backkeypress is clicked quickly after navigating to the contract list.
            // can't figure out why
            if (e is UnhandledExceptionEventArgs && e.Message.Contains("The parameter is incorrect."))
            {
                e.Handled = true;
                return;
            }
        }


        private async void RestoreStatus(ApplicationExecutionState previousExecutionState)
        {
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (previousExecutionState == ApplicationExecutionState.Terminated)
            {
                // Restore the saved session state only when appropriate
                try
                {
                    await SuspensionManager.RestoreAsync();
                }
                catch (SuspensionManagerException)
                {
                    //Something went wrong restoring state.
                    //Assume there is no state and continue
                }
            }
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
            if (args.Kind == ActivationKind.Protocol)
            {
                ProtocolActivatedEventArgs protocolArgs = args as ProtocolActivatedEventArgs;
                Frame rootFrame = CreateRootFrame();
                RestoreStatus(args.PreviousExecutionState);

                if (rootFrame.Content == null)
                {
                    if (!rootFrame.Navigate(typeof(MainPage)))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }

                double lat = 0, lon = 0;
                try
                {

                    string pattern = @"(?<=lt=)-?[0-9]\d*\.*\,*\d+";
                    if (Regex.IsMatch(protocolArgs.Uri.Query, pattern))
                    {
                        var regex = new Regex(pattern).Match(protocolArgs.Uri.Query);
                        if (regex != null && regex.Captures.Count > 0)
                        {
                            lat = double.Parse(regex.Captures[0].Value.Replace(',', '.'), CultureInfo.InvariantCulture);
                        }
                    }
                    pattern = @"(?<=ln=)-?[0-9]\d*\.*\,*\d+";
                    if (Regex.IsMatch(protocolArgs.Uri.Query, pattern))
                    {
                        var regex = new Regex(pattern).Match(protocolArgs.Uri.Query);
                        if (regex != null && regex.Captures.Count > 0)
                        {
                            lon = double.Parse(regex.Captures[0].Value.Replace(',', '.'), CultureInfo.InvariantCulture);
                        }
                    }

                }
                catch (Exception e)
                {
                    var dialog = new MessageDialog("Unable to find the passed location :(");
                    dialog.ShowAsync();
                }
                // Ensure the current window is active
                Window.Current.Activate();

                var p = rootFrame.Content as MainPage;
                if (p == null)
                {
                    if (rootFrame.CanGoBack)
                    {
                        rootFrame.GoBack();
                    }
                }
                p = rootFrame.Content as MainPage;

                if (p != null)
                {
                    if (lat != 0 && lon != 0)
                    {
                        p.SetViewToLocation(lat, lon);
                    }
                }
            }
        }
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }


        private Frame CreateRootFrame()
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // Set the default language
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");


                rootFrame.Navigate(typeof(MainPage), null);
                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            return rootFrame;
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            // CAREFULL: This is never reached in DEBUG mode
            var settingsService = SimpleIoc.Default.GetInstance<ISettingsService>();
            await settingsService.SaveSettingAsync();

            deferral.Complete();
        }
    }
}
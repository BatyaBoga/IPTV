using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using IPTV.Service;
using IPTV.ViewModels;
using IPTV.Views;
using Windows.UI.Core;

namespace IPTV
{

    sealed partial class App : Application
    {

        private static string currentThemeForApp;
        public App()
        {
            this.InitializeComponent();

            this.Suspending += OnSuspending;

            object theme = ApplicationData.Current.LocalSettings.Values["themeSetting"];

            object language = ApplicationData.Current.LocalSettings.Values["languageSetting"];

            if (language != null)
            {
                Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = (string)language;
            }

            if (theme != null)
            {
                App.Current.RequestedTheme = (ApplicationTheme)(int)theme;
            }

            CurrentThemeForApp = Current.RequestedTheme.ToString();

            NavigationService.Map.Add(typeof(PlayListViewModel), typeof(PlayListView));

            NavigationService.Map.Add(typeof(OptionsViewModel), typeof(OptionsView));

            DialogService.CurrentInstance.RegisterDialog<AddPlaylistDialog, AddListViewModel>();
        }

        public static string CurrentThemeForApp
        {
            get { return currentThemeForApp; }
            set
            {
                currentThemeForApp = value;
            }
        }
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;


            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                   
                }

                Window.Current.Content = rootFrame;
            }


            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                
                Window.Current.Activate();
            }
            

            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;

            rootFrame.Navigated += (s, args) =>
            {
                if (rootFrame.CanGoBack)
                {
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                                            AppViewBackButtonVisibility.Visible;
                }
                else
                {
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                        AppViewBackButtonVisibility.Collapsed;
                }

            };
        }
        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;

            if (frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
        }


        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}

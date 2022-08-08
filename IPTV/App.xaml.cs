using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using IPTV.ViewModels;
using Windows.Globalization;
using IPTV.Views;
using Windows.UI.Core;
using IPTV.Services;
using IPTV.Constants;

namespace IPTV
{
    sealed partial class App : Application
    {
        private static string currentThemeForApp;

        public App()
        {
            InitializeComponent();

            Suspending += OnSuspending;

            SetOptions();

            CurrentThemeForApp = Current.RequestedTheme.ToString();

            DependencyContainer.RegisterDependecy<AddPlaylistDialog, AddListViewModel>();

            DependencyContainer.RegisterDependecy<PlayListView, PlayListViewModel>();

            DependencyContainer.RegisterDependecy<OptionsView, OptionsViewModel>();
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
            NavigationService.Instance.GoBack();

            e.Handled = true;
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

        private void SetOptions()
        {
            object theme = ApplicationData.Current.LocalSettings.Values[Constant.ThemeSetting];

            string language = ApplicationData.Current.LocalSettings.Values[Constant.LanguageSettings] as string;

            if (language != null)
            {
                ApplicationLanguages.PrimaryLanguageOverride = language.ToString();
            }

            if (theme != null)
            {
                App.Current.RequestedTheme = (ApplicationTheme)Convert.ToInt32(theme);
            }
        }
    }
}

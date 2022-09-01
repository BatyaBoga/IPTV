using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Networking.Connectivity;
using CommunityToolkit.Mvvm.DependencyInjection;
using IPTV.Views;
using IPTV.Models;
using IPTV.Services;
using IPTV.Constants;
using IPTV.ViewModels;
using IPTV.Interfaces;
using IPTV.Models.Interfaces;

namespace IPTV
{
    public partial class App : Application
    {
        private readonly Updater updater;

        private readonly ISaveStateService saveStateService;

        public static ViewModelLocator ViewModel;

        public App()
        {
            InitializeComponent();

            ViewModel = ViewModelLocator.Instance;

            saveStateService = Ioc.Default.GetRequiredService<ISaveStateService>();

            updater = new Updater(Ioc.Default.GetRequiredService<IIptvManager>());

            SetOptions();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var rootFrame = GetRootFrame();

            if (e.PrelaunchActivated == false)
            {
               NavigateToRoot(rootFrame);
            }

            saveStateService.GoToLoadedMedia();

            Window.Current.Activate();
        }

        public static void IsBackButtonEnabled(bool isEnabled)
        {
            if (isEnabled)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                                        AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                                        AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Ioc.Default.GetRequiredService<INavigationService>().GoBack();

            e.Handled = true;
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            deferral.Complete();
        }

        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);

            IBackgroundTaskInstance taskInstance = args.TaskInstance;

            updater.Run(taskInstance);
        }

        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            var rootFrame = GetRootFrame();

            NavigateToRoot(rootFrame);

            Ioc.Default.GetRequiredService<INavigationService>()
                .Navigate<StreamViewModel>(args.Files[0] as StorageFile);

            Window.Current.Activate();
        }

        private Frame GetRootFrame()
        {
            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                Window.Current.Content = rootFrame;
            }

            return rootFrame;
        }

        private void NavigateToRoot(Frame rootFrame)
        {
            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainPage), saveStateService.ParametrToMain);
            }

            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
        }

        private void RegisterDependency()
        {
            DependencyTypeContainer.RegisterDependecy(
          new Dictionary<Type, Type>()
          .AddDependency<AddPlaylistDialog, AddListViewModel>()
          .AddDependency<MainPage, MainViewModel>()
          .AddDependency<PlayListView, PlayListViewModel>()
          .AddDependency<StreamView, StreamViewModel>()
          .AddDependency<OptionsView, OptionsViewModel>()
          .AddDependency<RemoteListView, RemoteListViewModel>()
          .AddDependency<LocalListView, LocalListViewModel>());
        }

        private void SetThemeFromStorgae()
        {
            var theme = ApplicationData.Current.LocalSettings.Values[Constant.ThemeSetting];

            if (theme != null)
            {
                Current.RequestedTheme = (ApplicationTheme)Convert.ToInt32(theme);
            }

            ThemeManager.CurrentThemeForApp = Current.RequestedTheme.ToString();
        }

        private void SetOptions()
        {
            Suspending += OnSuspending;

            SetThemeFromStorgae();

            RegisterDependency();

            saveStateService.LoadSavedMedia();

            NetworkInformation.NetworkStatusChanged += 
                Ioc.Default.GetRequiredService<IInternetChecker>().OnNetworkStatusChange;
        }
    }
}

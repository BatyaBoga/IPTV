using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using CommunityToolkit.Mvvm.DependencyInjection;
using IPTV.ViewModels;
using IPTV.Interfaces;
using IPTV.Services;
using IPTV.Views;
using IPTV.Models;
using IPTV.Models.Model;
using Windows.Storage;
using Windows.Networking.Connectivity;
using IPTV.Constants;

namespace IPTV
{
    sealed partial class App : Application
    {
        private readonly Updater updater;
        public App()
        {
            InitializeComponent();

            Suspending += OnSuspending;

            ThemeManager.SetThemeFromStorgae();

            DependencyTypeContainer.RegisterDependecy<AddPlaylistDialog, AddListViewModel>();

            DependencyTypeContainer.RegisterDependecy<PlayListView, PlayListViewModel>();

            DependencyTypeContainer.RegisterDependecy<StreamView, StreamViewModel>();

            DependencyTypeContainer.RegisterDependecy<OptionsView, OptionsViewModel>();

            var locator = ViewModelLocator.Instance;

            Ioc.Default.GetRequiredService<ISaveStateService>().LoadSaveState();

            updater = new Updater(Ioc.Default.GetRequiredService<IIptvManager>());

            NetworkInformation.NetworkStatusChanged += Ioc.Default.GetRequiredService<IInternetChecker>().OnNetworkStatusChange;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var rootFrame = GetRootFrame();

            if (e.PrelaunchActivated == false)
            {
               NavigateToRoot(rootFrame);
            }

            ActiveBackBtn(rootFrame);

            Ioc.Default.GetRequiredService<ISaveStateService>().GoToSaveState();

            Window.Current.Activate();
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Ioc.Default.GetRequiredService<INavigationService>().GoBack();

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

            ActiveBackBtn(rootFrame);

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
                rootFrame.Navigate(typeof(MainPage));
            }

            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
        }

        private void ActiveBackBtn(Frame rootFrame)
        {

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

    }
}

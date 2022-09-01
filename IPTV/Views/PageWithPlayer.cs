using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Core;
using CommunityToolkit.Mvvm.DependencyInjection;
using IPTV.Constants;
using IPTV.Interfaces;

namespace IPTV.Views
{
    public class PageWithPlayer : Page
    {
        private object parameter;

        protected static object ViewModel;

        protected static ISaveStateService SaveServise => Ioc.Default.GetRequiredService<ISaveStateService>();
        protected static IInternetChecker InterneServise => Ioc.Default.GetRequiredService<IInternetChecker>();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            parameter = e.Parameter;

            SetMediaSource();

            App.IsBackButtonEnabled(true);

            SaveServise.ActiveSave(Constant.Remote, ViewModel);

            InterneServise.InternetRestoringEvent += OnInternetRestoring;

            DataContext = ViewModel;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            SaveServise.DeactiveSave();

            DataContext = null;

            InterneServise.InternetRestoringEvent += OnInternetRestoring;

            App.IsBackButtonEnabled(false);
        }

        protected async virtual void OnInternetRestoring()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                SetMediaSource(parameter);
            });
        }

        private void SetMediaSource() 
        {
            if(parameter != null)
            {
                SetMediaSource(parameter);
            }
        }

        protected virtual void SetMediaSource(object parameter) { }
    }
}

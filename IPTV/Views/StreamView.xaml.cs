using CommunityToolkit.Mvvm.DependencyInjection;
using IPTV.Services;
using System;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using IPTV.ViewModels;
using IPTV.Interfaces;
using Windows.Media.Playback;

namespace IPTV.Views
{
    public sealed partial class StreamView : Page
    {
        public StreamView()
        {
            InitializeComponent();
        }

        private static StreamViewModel ViewModel { get => ViewModelLocator.Instance.Stream;}

        private static ISaveStateService SaveServise { get => Ioc.Default.GetRequiredService<ISaveStateService>(); }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (e.Parameter != null)
            {
                if(e.Parameter is StorageFile)
                {
                    ViewModel.SetSource(e.Parameter as StorageFile);
                }
                else
                {
                    ViewModel.SetSource(e.Parameter.ToString());

                    SaveServise.ActiveSave(ViewModel);
                } 
            }

            DataContext = ViewModel;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            SaveServise.DeactiveSave();

            DataContext = null;
        }

    }
}

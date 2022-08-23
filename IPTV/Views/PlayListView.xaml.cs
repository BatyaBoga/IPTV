using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Core;
using IPTV.Models.Model;
using IPTV.Services;
using Windows.UI.ViewManagement;
using Windows.UI.Core.Preview;
using IPTV.ViewModels;
using Newtonsoft.Json;
using IPTV.Interfaces;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace IPTV.Views
{
    public sealed partial class PlayListView : Page
    {
        public PlayListView()
        {
            InitializeComponent();
        }

        private static PlayListViewModel ViewModel { get => ViewModelLocator.Instance.PlayList;}

        private static ISaveStateService SaveServise { get => Ioc.Default.GetRequiredService<ISaveStateService>(); }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                ViewModel.PlayList = e.Parameter as Playlist;
            }

            SaveServise.ActiveSave(ViewModel);

            DataContext = ViewModel;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
           SaveServise.DeactiveSave();

           DataContext = null;
        }
    }
}

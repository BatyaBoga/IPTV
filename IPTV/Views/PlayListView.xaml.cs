using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using IPTV.ViewModels;
using IPTV.Models;
using IPTV.Services;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;

namespace IPTV.Views
{
    public sealed partial class PlayListView : Page
    {
        public PlayListView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                PlayListViewModel viewmodel = ViewModelLocator.Instance.PlayList;
                viewmodel.PlayList = e.Parameter as LinksInfo;
                DataContext = viewmodel;
                
            }
        }

        protected async override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Player.MediaPlayer.Pause();
            });
        }
    }
}

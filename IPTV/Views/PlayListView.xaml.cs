using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Core;
using IPTV.Models.Model;
using IPTV.Services;
using Windows.UI.ViewManagement;

namespace IPTV.Views
{
    public sealed partial class PlayListView : Page
    {
        public PlayListView()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                var viewmodel = ViewModelLocator.Instance.PlayList;

                viewmodel.PlayList = e.Parameter as Playlist;

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

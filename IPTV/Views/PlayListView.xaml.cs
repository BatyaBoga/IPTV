using System;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
using IPTV.Models.Model;
using IPTV.ViewModels;

namespace IPTV.Views
{
    public sealed partial class PlayListView : PageWithPlayer
    {
        public PlayListView()
        {
            InitializeComponent();

            ViewModel = ViewModelLocator.Instance.PlayList;
        }
  
        protected override void SetMediaSource(object parameter)
        {
            (ViewModel as PlayListViewModel).PlayList = parameter as Playlist;   
        }

        protected override async void OnInternetRestoring()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                (ViewModel as PlayListViewModel).OnSelectedChanelChnaged();
            });
        }
    }
}

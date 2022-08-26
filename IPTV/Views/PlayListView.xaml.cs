using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CommunityToolkit.Mvvm.DependencyInjection;
using IPTV.Models.Model;
using IPTV.Services;
using IPTV.ViewModels;
using IPTV.Interfaces;
using IPTV.Constants;

namespace IPTV.Views
{
    public sealed partial class PlayListView : Page
    {
        public PlayListView()
        {
            InitializeComponent();
        }

        private static PlayListViewModel ViewModel => ViewModelLocator.Instance.PlayList;

        private static ISaveStateService SaveServise => Ioc.Default.GetRequiredService<ISaveStateService>(); 

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App.IsBackButtonEnabled(true);

            if (e.Parameter != null)
            {
                ViewModel.PlayList = e.Parameter as Playlist;
            }

            SaveServise.ActiveSave(Constant.Remote, ViewModel);

            DataContext = ViewModel;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
           SaveServise.DeactiveSave();

           DataContext = null;

           App.IsBackButtonEnabled(false);
        }
    }
}

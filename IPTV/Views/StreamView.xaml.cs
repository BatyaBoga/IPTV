using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CommunityToolkit.Mvvm.DependencyInjection;
using IPTV.Services;
using IPTV.ViewModels;
using IPTV.Interfaces;
using IPTV.Constants;

namespace IPTV.Views
{
    public sealed partial class StreamView : Page
    {
        public StreamView()
        {
            InitializeComponent();
        }

        private static StreamViewModel ViewModel => ViewModelLocator.Instance.Stream;

        private static ISaveStateService SaveServise => Ioc.Default.GetRequiredService<ISaveStateService>();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App.IsBackButtonEnabled(true);

            if (e.Parameter != null)
            {
                if(e.Parameter is StorageFile)
                {
                    ViewModel.SetSource(e.Parameter as StorageFile);

                    SaveServise.ActiveSave(Constant.Local, ViewModel);
                }
                else
                {
                    ViewModel.SetSource(e.Parameter.ToString());

                    SaveServise.ActiveSave(Constant.Remote, ViewModel);
                } 
            }

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

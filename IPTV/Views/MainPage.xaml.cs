using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using IPTV.Services;
using IPTV.Constants;
using IPTV.ViewModels;

namespace IPTV
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            DataContext = ViewModel;
        }

        public Frame NavigationFrame => ContentFrame;

        private static MainViewModel ViewModel => ViewModelLocator.Instance.Main;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _ =  Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                string tag = tag = Constant.Remote;


                if (ViewModel.SelectedItem != null)
                {
                    tag = (ViewModel.SelectedItem as NavigationViewItem).Tag as string;
                }
                else if (e.Parameter != null)
                {
                    tag = e.Parameter.ToString();
                }
               

                if ( tag != null && tag != Constant.Options)
                {
                    Pane.SelectedItem = tag == Constant.Local ? LocalBtn : RemoteBtn;
                }
                else
                {
                    Pane.SelectedItem = Pane.SettingsItem;
                }
            });
        }
    }
}

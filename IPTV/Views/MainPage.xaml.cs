using Windows.UI.Xaml.Controls;
using IPTV.Services;

namespace IPTV
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            DataContext = ViewModelLocator.Instance.Main;
        }
    }
}

using Windows.UI.Xaml.Controls;
using IPTV.Services;

namespace IPTV.Views
{
    public sealed partial class LocalListView : Page
    {
        public LocalListView()
        {
            InitializeComponent();

            DataContext = ViewModelLocator.Instance.LocalList;
        }
    }
}

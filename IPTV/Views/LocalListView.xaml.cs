using Windows.UI.Xaml.Controls;

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

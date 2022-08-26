using Windows.UI.Xaml.Controls;
using IPTV.Services;

namespace IPTV.Views
{
    public sealed partial class RemoteListView : Page
    {
        public RemoteListView()
        {
            InitializeComponent();

            DataContext = ViewModelLocator.Instance.RemouteList;
        }
    }
}

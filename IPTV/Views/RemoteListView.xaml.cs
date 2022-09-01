using Windows.UI.Xaml.Controls;

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

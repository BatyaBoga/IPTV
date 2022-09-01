using Windows.UI.Xaml.Controls;

namespace IPTV.Views
{
    public sealed partial class OptionsView : Page
    {
        public OptionsView()
        {
            InitializeComponent();

            DataContext  = ViewModelLocator.Instance.Options;
        }
    }
}

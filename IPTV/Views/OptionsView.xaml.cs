using Windows.UI.Xaml.Controls;
using IPTV.Services;


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

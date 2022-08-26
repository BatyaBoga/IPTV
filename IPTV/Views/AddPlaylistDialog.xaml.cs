using Windows.UI.Xaml.Controls;
using IPTV.Models.Model;
using IPTV.ViewModels;
using IPTV.Services;

namespace IPTV.Views
{
    public sealed partial class AddPlaylistDialog : ContentDialog
    {
        public AddPlaylistDialog()
        {
            InitializeComponent();

            ViewModel.ConfigureToAdd();

            DataContext = ViewModel;
        }

        public AddPlaylistDialog(Playlist playlist) : this()
        {
            ViewModel.ConfigureToEdit(playlist);
        }

        private static AddListViewModel ViewModel => ViewModelLocator.Instance.AddList;
    }
}

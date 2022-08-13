using Windows.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using IPTV.Models.Model;
using IPTV.ViewModels;

namespace IPTV.Views
{
    public sealed partial class AddPlaylistDialog : ContentDialog
    {

        public AddPlaylistDialog()
        {
            InitializeComponent();

            ViewModel = Ioc.Default.GetRequiredService<AddListViewModel>();

            ViewModel.ConfigureToAdd();

            DataContext = ViewModel;
        }

        public AddListViewModel ViewModel { get; set; }

        public AddPlaylistDialog(Playlist playlist) : this()
        {
            ViewModel.ConfigureToEdit(playlist);
        }

    }
}

using Windows.UI.Xaml.Controls;
using IPTV.Models;
using IPTV.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;

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

        public AddPlaylistDialog(LinksInfo LinksInfoToEditId) : this()
        {
            ViewModel.ConfigureToEdit(LinksInfoToEditId);
        }

    }
}

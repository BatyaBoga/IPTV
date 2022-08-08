using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using IPTV.Models;
using IPTV.ViewModels;
using System.Collections.ObjectModel;

namespace IPTV.Views
{
    public sealed partial class AddPlaylistDialog : ContentDialog
    {

        private AddPlaylistDialog()
        {
            InitializeComponent();
        }
        public AddPlaylistDialog(ObservableCollection<LinksInfo> links, LinksInfo LinksInfoToEditId) : this()
        {
            this.DataContext = new AddListViewModel(links, LinksInfoToEditId);
        }

        public AddPlaylistDialog(ObservableCollection<LinksInfo> links) : this()
        {
            this.DataContext = new AddListViewModel(links);
          
        }



    }
}

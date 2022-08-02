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
using IPTV.ViewModels;
using IPTV.Service;
using IPTV.Models;


namespace IPTV.Views
{
    public sealed partial class PlayListView : Page
    {
        public PlayListView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                this.DataContext = new PlayListViewModel((LinksInfo)e.Parameter);
                NavigationService.CurrentInstance.NavigationFrame = PlayListFrame;
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using IPTV.ViewModels;
using IPTV.Views;
using IPTV.Service;


namespace IPTV
{

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            NavigationService.CurrentInstance.NavigationFrame = MainFrame;

            NS.typeMap.Add(typeof(PlayListViewModel), typeof(PlayListView));

            DialogService.CurrentInstance.RegisterDialog<AddPlaylistDialog, AddListViewModel> ();
        }

    }
}

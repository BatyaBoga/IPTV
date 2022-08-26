using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;
using IPTV.ViewModels;
using IPTV.Interfaces;
using IPTV.Models;
using IPTV.Views;

namespace IPTV.Services
{
    public class ViewModelLocator
    {
        private static readonly Lazy<ViewModelLocator> instance =
            new Lazy<ViewModelLocator>(() => new ViewModelLocator());

        public static ViewModelLocator Instance => instance.Value;

        private ViewModelLocator()
        {
            AddServices();

            AddDependency();
        }

        public OptionsViewModel Options => Ioc.Default.GetRequiredService<OptionsViewModel>();

        public PlayListViewModel PlayList => Ioc.Default.GetRequiredService<PlayListViewModel>();

        public StreamViewModel Stream => Ioc.Default.GetRequiredService<StreamViewModel>();

        public MainViewModel Main => Ioc.Default.GetRequiredService<MainViewModel>();

        public RemoteListViewModel RemouteList => Ioc.Default.GetRequiredService<RemoteListViewModel>();

        public LocalListViewModel LocalList => Ioc.Default.GetRequiredService<LocalListViewModel>();

        public AddListViewModel AddList => Ioc.Default.GetRequiredService<AddListViewModel>();

        private static void AddServices()
        {
            Ioc.Default.ConfigureServices(
              new ServiceCollection()
              .AddSingleton<ILangugeManager, LanguageManager>()
              .AddSingleton<IThemeManager, ThemeManager>()
              .AddSingleton<INavigationService, NavigationService>()
              .AddSingleton<IDialogService, DialogService>()
              .AddSingleton<IInternetChecker, InternetChecker>()
              .AddSingleton<IMessageDialog, MessageDialogManager>()
              .AddSingleton<IIptvManager, IptvManager>()
              .AddTransient<IExplorer, Explorer>()
              .AddSingleton<ISaveStateService, SaveStateService>()
              .AddSingleton<IMediaFile, MediaFileManager>()
              .AddSingleton<OptionsViewModel>()
              .AddSingleton<MainViewModel>()
              .AddSingleton<RemoteListViewModel>()
              .AddSingleton<LocalListViewModel>()
              .AddSingleton<AddListViewModel>()
              .AddSingleton<PlayListViewModel>()
              .AddSingleton<StreamViewModel>()
              .BuildServiceProvider());
        }

        private static void AddDependency()
        {
            DependencyTypeContainer.RegisterDependecy(
                new Dictionary<Type, Type>()
                .AddDependency<AddPlaylistDialog, AddListViewModel>()
                .AddDependency<MainPage, MainViewModel>()
                .AddDependency<PlayListView, PlayListViewModel>()
                .AddDependency<StreamView, StreamViewModel>()
                .AddDependency<OptionsView, OptionsViewModel>()
                .AddDependency<RemoteListView, RemoteListViewModel>()
                .AddDependency<LocalListView, LocalListViewModel>());
        }
    }
}

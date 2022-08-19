using System;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;
using IPTV.ViewModels;
using IPTV.Interfaces;
using IPTV.Models;
using Windows.ApplicationModel.Resources;

namespace IPTV.Services
{
    public class ViewModelLocator
    {
        private static readonly Lazy<ViewModelLocator> instance =
            new Lazy<ViewModelLocator>(() => new ViewModelLocator());

        public static ViewModelLocator Instance => instance.Value;
        private ViewModelLocator()
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
              .AddSingleton<IExplorer, Explorer>()
              .AddSingleton<OptionsViewModel>()
              .AddSingleton<MainViewModel>()
              .AddSingleton<AddListViewModel>()
              .AddSingleton<PlayListViewModel>()
              .AddSingleton<StreamViewModel>()
              .BuildServiceProvider());
        }

        public OptionsViewModel Options => Ioc.Default.GetRequiredService<OptionsViewModel>();

        public PlayListViewModel PlayList => Ioc.Default.GetRequiredService<PlayListViewModel>();

        public StreamViewModel Stream => Ioc.Default.GetRequiredService<StreamViewModel>();

        public MainViewModel Main => Ioc.Default.GetRequiredService<MainViewModel>();

        public AddListViewModel AddList => Ioc.Default.GetRequiredService<AddListViewModel>();
    }
}

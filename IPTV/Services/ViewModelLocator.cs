using CommunityToolkit.Mvvm.DependencyInjection;
using IPTV.Managers;
using IPTV.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

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
              .AddSingleton<IMessageDialog, MessageDialogManager>()
              .AddSingleton<OptionsViewModel>()
              .AddSingleton<MainViewModel>()
              .AddSingleton<AddListViewModel>()
              .AddTransient<PlayListViewModel>()
              .BuildServiceProvider());
        }

        public OptionsViewModel Options => Ioc.Default.GetRequiredService<OptionsViewModel>();

        public PlayListViewModel PlayList => Ioc.Default.GetRequiredService<PlayListViewModel>();

        public MainViewModel Main => Ioc.Default.GetRequiredService<MainViewModel>();

        public AddListViewModel AddList => Ioc.Default.GetRequiredService<AddListViewModel>();
    }
}

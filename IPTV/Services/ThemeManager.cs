using System;
using Windows.Storage;
using Windows.UI.Xaml;
using IPTV.Constants;
using IPTV.Interfaces;
using IPTV.ViewModels;

namespace IPTV.Services
{
    public class ThemeManager : IThemeManager
    {
        private static string currentThemeForApp;

        private readonly INavigationService navigation;

        public ThemeManager(INavigationService navigation)
        {
            this.navigation = navigation;
        }

        public static string CurrentThemeForApp
        {
            get
            {
                return currentThemeForApp;
            }
            set
            {
                if (currentThemeForApp != value)
                {
                    currentThemeForApp = value;
                }
            }
        }

        public bool IsLightTheme => CurrentThemeForApp == Constant.LightTheme;

        private static object AppTheme
        {
            get => ApplicationData.Current.LocalSettings.Values[Constant.ThemeSetting];

            set
            {
                ApplicationData.Current.LocalSettings.Values[Constant.ThemeSetting] = value;
            }
        }

        public static void SetThemeFromStorgae()
        {
            object theme = AppTheme;

            if (theme != null)
            {
                App.Current.RequestedTheme = (ApplicationTheme)Convert.ToInt32(theme);
            }

            CurrentThemeForApp = App.Current.RequestedTheme.ToString();
        }

        public void ChangeTheme(bool theme)
        {
            CurrentThemeForApp = theme ? Constant.LightTheme : Constant.DarkTheme;

            AppTheme = theme ? 0 : 1;

            navigation.Refresh<OptionsViewModel>();
        }
    }
}

using Windows.Storage;
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

        public string Theme => currentThemeForApp;

        public bool IsLightTheme => CurrentThemeForApp == Constant.LightTheme;


        public void ChangeTheme(bool theme)
        {
            CurrentThemeForApp = theme ? Constant.LightTheme : Constant.DarkTheme;

            ApplicationData.Current.LocalSettings.Values[Constant.ThemeSetting] = theme ? 0 : 1;

            navigation.Refresh<OptionsViewModel>();
        }
    }
}

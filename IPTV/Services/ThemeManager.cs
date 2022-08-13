using System;
using Windows.Storage;
using Windows.UI.Xaml;
using IPTV.Constants;
using IPTV.Interfaces;

namespace IPTV.Services
{
    public class ThemeManager : IThemeManager
    {
        private static string currentThemeForApp;
        public static string CurrentThemeForApp
        {
            get 
            { 
                return currentThemeForApp; 
            }
            set
            {
                if(currentThemeForApp != value)
                {
                    currentThemeForApp = value;
                }
            }
        }

        public bool IsLightTheme
        {
            get
            {
               return CurrentThemeForApp == Constant.LightTheme ? true : false;
            }
        }

        public static void SetThemeFromStorgae()
        {
            object theme = ApplicationData.Current.LocalSettings.Values[Constant.ThemeSetting];

            if (theme != null)
            {
                App.Current.RequestedTheme = (ApplicationTheme)Convert.ToInt32(theme);
            }

            CurrentThemeForApp = App.Current.RequestedTheme.ToString();
        }

        public void ChangeTheme(bool theme)
        {
             CurrentThemeForApp = theme ? Constant.LightTheme : Constant.DarkTheme;

            ApplicationData.Current.LocalSettings.Values[Constant.ThemeSetting] = theme ? 0 : 1;
        }
    }
}

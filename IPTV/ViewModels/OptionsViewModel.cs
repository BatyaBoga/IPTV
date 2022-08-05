using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.Globalization;
using Windows.ApplicationModel.Resources;
using IPTV.Service;

namespace IPTV.ViewModels
{
    public class OptionsViewModel : ViewModel
    {
        private bool isToogleOn;

        private Dictionary<string, string> languages;

        private int selectedIndex;
        public OptionsViewModel()
        {
            SetToogle();

            languages = new Dictionary<string, string>();
            languages.Add("en-Us", "English");
            languages.Add("ru", "Русский");

            SetLanguage();
        }
        public string ToogleOn
        {
            get
            {
                var resourceLoader = ResourceLoader.GetForCurrentView();
                return resourceLoader.GetString("Light");
            }
        }
        public bool IsToogleOn
        {
            get { return isToogleOn; }
            set {
                isToogleOn = value;
                ChangeTheme(value);
                OnPropertyChanged();
            }
        }
        public Dictionary<string,string> Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                OnPropertyChanged();
            }
        }
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (value != selectedIndex)
                {
                    selectedIndex = value;
                    ChnageLanguage(languages.ElementAt(selectedIndex).Key);
                }

                OnPropertyChanged();
            }
        }
        public string CurrentTheme
        {
            get { return App.CurrentThemeForApp; }
            set
            {
                App.CurrentThemeForApp = value;
                OnPropertyChanged();
            }
        }
        private async void ChnageLanguage(string culture)
        {
            ApplicationLanguages.PrimaryLanguageOverride = culture;
            ApplicationData.Current.LocalSettings.Values["languageSetting"] = culture;
            await NavigationService.Instance.Refresh(typeof(OptionsViewModel));
        }
        private void ChangeTheme(bool theme)
        {
            CurrentTheme = theme ? "Light" : "Dark";
            ApplicationData.Current.LocalSettings.Values["themeSetting"] = theme ? 0: 1;
        }
        private void SetToogle()
        {
            if (App.CurrentThemeForApp == "Light")
            {
                isToogleOn = true;
            }
            else
            {
                isToogleOn = false;
            }

        }
        private void SetLanguage()
        {
            object languageObj = ApplicationData.Current.LocalSettings.Values["languageSetting"];

            string languageName = string.Empty;

            if (languageObj != null)
            {
                languageName = (string)languageObj;
            }
            else
            {
                languageName = ApplicationLanguages.PrimaryLanguageOverride;
            }


            for (int i = 0; i < languages.Count; i++)
            {
                if (languages.ElementAt(i).Key == languageName)
                {
                    selectedIndex = i;
                }
            }
        }
    }
}

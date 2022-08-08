using System.Collections.Generic;
using Windows.Storage;
using Windows.ApplicationModel.Resources;
using IPTV.Constants;
using IPTV.Managers;

namespace IPTV.ViewModels
{
    public class OptionsViewModel : ViewModel
    {
        private bool isToogleOn;

        private LanguageManager languageManager;

        private int selectedIndex;

        public OptionsViewModel()
        {
            SetToogleState();

            languageManager = new LanguageManager();

            selectedIndex = languageManager.SelectedLanguageIndex();
        }

        public string ToogleOn
        {
            get => ResourceLoader.GetForCurrentView().GetString(Constant.LightTheme); 
        }

        public bool IsToogleOn
        {
            get
            {
                return isToogleOn;
            }
            set 
            {
                isToogleOn = value;

                ChangeTheme(value);

                OnPropertyChanged();
            }
        }

        public Dictionary<string,string> Languages
        {
            get
            {
                return languageManager.languages;
            } 
            set
            {
                languageManager.languages = value;
                OnPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                if (value != selectedIndex)
                {
                    selectedIndex = value;

                    languageManager.ChnageLanguage(selectedIndex);

                    OnPropertyChanged();
                }  
            }
        }

        public string CurrentTheme
        {
            get
            {
                return App.CurrentThemeForApp;
            }
            set
            {
                App.CurrentThemeForApp = value;

                OnPropertyChanged();
            }
        }

        private void SetToogleState()
        {
            if (App.CurrentThemeForApp == Constant.LightTheme)
            {
                isToogleOn = true;
            }
            else
            {
                isToogleOn = false;
            }

        }

        private void ChangeTheme(bool theme)
        {
            CurrentTheme = theme ? Constant.LightTheme : Constant.DarkTheme;

            ApplicationData.Current.LocalSettings.Values[Constant.ThemeSetting] = theme ? 0: 1;
        }
    }
}

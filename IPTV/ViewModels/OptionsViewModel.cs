using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using CommunityToolkit.Mvvm.ComponentModel;
using IPTV.Constants;
using IPTV.Interfaces;
using IPTV.Services;

namespace IPTV.ViewModels
{
    public class OptionsViewModel : ObservableObject
    {
        private bool isToogleOn;

        private int selectedIndex;

        private readonly ILangugeManager languageManager;

        private readonly IThemeManager themeManager;

        public OptionsViewModel(ILangugeManager languageManager, IThemeManager themeManager)
        {
            this.languageManager = languageManager;  

            this.themeManager = themeManager;

            isToogleOn = this.themeManager.IsLightTheme;

            selectedIndex = this.languageManager.SelectedLanguageIndex();
        }

        public Dictionary<string,string>.ValueCollection Languages => languageManager.Languages.Values;

        public string ToogleOn => ResourceLoader.GetForCurrentView().GetString(Constant.LightTheme);

        public string CurrentTheme => ThemeManager.CurrentThemeForApp;

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                if(SetProperty(ref selectedIndex, value))
                {
                    languageManager.ChangeLanguage(selectedIndex);
                }   
            }
        }

        public bool IsToogleOn
        {
            get
            {
                return isToogleOn;
            }
            set 
            {
                if(SetProperty(ref isToogleOn, value))
                {
                    themeManager.ChangeTheme(value);

                    OnPropertyChanged(nameof(CurrentTheme));
                }
            }
        }
    }
}

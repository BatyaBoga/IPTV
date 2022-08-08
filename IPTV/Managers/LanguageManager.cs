using IPTV.Constants;
using IPTV.Services;
using IPTV.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Windows.Globalization;
using Windows.Storage;

namespace IPTV.Managers
{
    public class LanguageManager
    {
        public Dictionary<string, string> languages = new Dictionary<string, string>();

        public LanguageManager()
        {
            languages.Add("en-Us", "English");

            languages.Add("ru", "Русский");
        }
        public async void ChnageLanguage(int languageIndex)
        {
            var culture = languages.ElementAt(languageIndex).Key;

            ApplicationLanguages.PrimaryLanguageOverride = culture;

            ApplicationData.Current.LocalSettings.Values[Constant.LanguageSettings] = culture;

            await NavigationService.Instance.Refresh<OptionsViewModel>();
        }

        public int SelectedLanguageIndex()
        {
            string languageName = ApplicationLanguages.PrimaryLanguageOverride;

            for (int i = 0; i < languages.Count; i++)
            {
                if (languages.ElementAt(i).Key == languageName)
                {
                     return i;
                }
            }

            return 0; 
        }
    }
}

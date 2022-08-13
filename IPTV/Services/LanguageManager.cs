using System.Linq;
using System.Collections.Generic;
using Windows.Globalization;
using IPTV.ViewModels;
using IPTV.Interfaces;

namespace IPTV.Services
{
    public class LanguageManager : ILangugeManager
    {
        private Dictionary<string, string> languages = new Dictionary<string, string>();

        private INavigationService navigation;
        public LanguageManager(INavigationService navigation)
        {
            this.navigation = navigation;

            languages.Add("en-Us", "English");

            languages.Add("ru", "Русский");
        }

        public Dictionary<string, string> Languages
        {
            get { return languages; }
        }

        public async void ChangeLanguage(int languageIndex)
        {
           var culture = languages.ElementAt(languageIndex).Key;

           ApplicationLanguages.PrimaryLanguageOverride = culture;

           await navigation.Refresh<OptionsViewModel>();
        }

        public int SelectedLanguageIndex()
        {
            string languageName = ApplicationLanguages.PrimaryLanguageOverride;

            int selectedIndex = 0;

            for (int i = 0; i < languages.Count; i++)
            {
                if (languages.ElementAt(i).Key == languageName)
                {
                     selectedIndex = i;
                }
            }

            return selectedIndex; 
        }
    }
}

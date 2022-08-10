using IPTV.Services;
using System.Collections.Generic;

namespace IPTV.Managers
{
    public interface ILangugeManager
    {
        Dictionary<string, string> Languages { get;}

        void ChangeLanguage(int languageIndex);

        int SelectedLanguageIndex();
    }
}

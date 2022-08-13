using System.Collections.Generic;

namespace IPTV.Interfaces
{
    public interface ILangugeManager
    {
        Dictionary<string, string> Languages { get;}

        void ChangeLanguage(int languageIndex);

        int SelectedLanguageIndex();
    }
}

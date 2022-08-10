using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPTV.Managers
{
    public interface IThemeManager
    {
        bool IsLightTheme { get; }

        void ChangeTheme(bool theme);
    }
}

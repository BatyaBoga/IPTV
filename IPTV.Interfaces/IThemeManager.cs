namespace IPTV.Interfaces
{
    public interface IThemeManager
    {
        bool IsLightTheme { get; }

        string Theme { get; }

        void ChangeTheme(bool theme);
    }
}

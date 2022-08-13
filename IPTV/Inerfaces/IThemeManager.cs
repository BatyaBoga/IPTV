namespace IPTV.Interfaces
{
    public interface IThemeManager
    {
        bool IsLightTheme { get; }

        void ChangeTheme(bool theme);
    }
}

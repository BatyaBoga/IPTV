namespace IPTV.Constants
{
    public static class Constant
    {
        public  const string DataFileName = "Links.json";

        public const string RegexForChnaels = @"tvg-logo=""(([^""]+)?)"".+,(.+)\s(https?\S+)";

        public const string RegexForTitle = @"^\w+$";

        public const string RegexForLink = @"https?:\/\/[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*).m3u$";

        public const string ThemeSetting = "themeSetting";

        public const string LanguageSettings = "languageSetting";

        public const string LightTheme = "Light";

        public const string DarkTheme = "Dark";
    }
}

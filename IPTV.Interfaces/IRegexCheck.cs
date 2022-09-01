namespace IPTV.Interfaces
{
    public interface IRegexCheck
    {
        bool IsLink(string link);

        bool IsTitle(string title);
    }
}

namespace IPTV.Interfaces
{
    public interface IInternetChecker
    {
        bool IsConnected { get; }
        void OnNetworkStatusChange(object sender);
    }
}

using System.Threading.Tasks;
using Windows.UI.Popups;

namespace IPTV.Interfaces
{
    public interface IMessageDialog
    {
        Task ShowInfoMsg(string msg);

        Task ShureMsg(string msg, UICommandInvokedHandler onYesClick);
    }
}

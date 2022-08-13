using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.ApplicationModel.Resources;
using IPTV.Interfaces;

namespace IPTV.Services
{
    public class MessageDialogManager : IMessageDialog
    {
        private readonly ResourceLoader resload;

        public MessageDialogManager()
        {
            resload = ResourceLoader.GetForCurrentView();
        }

        public async Task ShowInfoMsg(string msg)
        {
            var dialog = new MessageDialog(resload.GetString(msg));

            dialog.Commands.Add(new UICommand("OK"));

            await dialog.ShowAsync();
        }

        public async Task ShureMsg(string msg, UICommandInvokedHandler onYesClick)
        {
            var dialog = new MessageDialog(resload.GetString(msg));

            dialog.Commands.Add(new UICommand(resload.GetString("Yes"), onYesClick));

            dialog.Commands.Add(new UICommand(resload.GetString("No")));

            await dialog.ShowAsync();
        }
    }
}

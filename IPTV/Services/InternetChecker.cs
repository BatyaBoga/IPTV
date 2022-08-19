using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using IPTV.Interfaces;
using Windows.ApplicationModel.Resources;

namespace IPTV.Services
{
    public class InternetChecker : IInternetChecker
    {

        private readonly ResourceLoader resload;

        public InternetChecker()
        {
            resload = new ResourceLoader();
        }
        public bool IsConnected
        {
            get => NetworkInformation.GetInternetConnectionProfile() != null;
        }

        public void OnNetworkStatusChange(object sender)
        {

            if (IsConnected)
            {
               ShowMsg(resload.GetString("InternetEstablished"));
            }
            else
            {
                ShowMsg(resload.GetString("InternetLost"));
            }
        }

        private void ShowMsg(string body)
        {
            var ToastNotifier = ToastNotificationManager.CreateToastNotifier();

            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode("IPTV"));
            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(body));
            var toastNode = toastXml.SelectSingleNode("/toast");
            var audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            var toast = new ToastNotification(toastXml);
            toast.ExpirationTime = DateTime.Now.AddSeconds(4);
            ToastNotifier.Show(toast);
        }
    }
}

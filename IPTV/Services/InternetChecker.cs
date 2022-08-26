using System;
using Windows.UI.Notifications;
using Windows.Networking.Connectivity;
using Windows.ApplicationModel.Resources;
using IPTV.Interfaces;
using IPTV.Constants;

namespace IPTV.Services
{
    public class InternetChecker : IInternetChecker
    {
        private readonly ResourceLoader resload = new ResourceLoader();

        public bool IsConnected => NetworkInformation.GetInternetConnectionProfile() != null;

        public void OnNetworkStatusChange(object sender)
        {
            if (IsConnected)
            {
                ShowMsg(resload.GetString(Constant.InternetEstablished));
            }
            else
            {
                ShowMsg(resload.GetString(Constant.InternetLost));
            }
        }

        private void ShowMsg(string body)
        {
            var ToastNotifier = ToastNotificationManager.CreateToastNotifier();

            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            var toastNodeList = toastXml.GetElementsByTagName("text");

            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode(Constant.Iptv));

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

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

        private bool InternetStatus;

        public bool IsConnected => NetworkInformation.GetInternetConnectionProfile() != null;

        public event Action InternetRestoringEvent;

        public InternetChecker()
        {
            InternetStatus = IsConnected;
        }

        public void OnNetworkStatusChange(object sender)
        {
            if (InternetStatus != IsConnected)
            {
                if (IsConnected)
                {
                    ShowMsg(resload.GetString(Constant.InternetEstablished));

                    InternetRestoringEvent?.Invoke();
                }
                else
                {
                    ShowMsg(resload.GetString(Constant.InternetLost));
                }

                InternetStatus = IsConnected;
            }
        }

        private void ShowMsg(string body)
        {
            var ToastNotifier = ToastNotificationManager.CreateToastNotifier();

            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            var toastNodeList = toastXml.GetElementsByTagName("text");

            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode(Constant.Iptv));

            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(body));

            toastXml.SelectSingleNode("/toast");

            var audio = toastXml.CreateElement("audio");

            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            var toast = new ToastNotification(toastXml)
            {
                ExpirationTime = DateTime.Now.AddSeconds(2)
            };

            ToastNotifier.Show(toast);
        }
    }
}

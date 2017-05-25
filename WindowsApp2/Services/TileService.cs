using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using Windows.UI.Notifications;
using NotificationsExtensions.Tiles;
using NotificationsExtensions.Toasts;
using WindowsApp2.ViewModels;

namespace Windowsapp2.Services
{
    class TileService
    {
        public static void ShowToastNotification(string title, string stringContent, int time)
        {
            var ToastNotifier = ToastNotificationManager.CreateToastNotifier();
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText04);
            var toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode(title));
            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(stringContent));
            var toastNode = toastXml.SelectSingleNode("/toast");
            var audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            var toast = new ToastNotification(toastXml);
            toast.ExpirationTime = DateTime.Now.AddSeconds(time);
            ToastNotifier.Show(toast);
        }
        public static void UpdateTile(string infoString)
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            var tileXml =
                TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text01);

            var tileAttributes = tileXml.GetElementsByTagName("text");
            tileAttributes[0].AppendChild(tileXml.CreateTextNode(infoString));
            var tileNotification = new TileNotification(tileXml);

            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }
    }
}

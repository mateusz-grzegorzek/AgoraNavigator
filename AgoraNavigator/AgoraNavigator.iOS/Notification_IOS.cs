using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using UserNotifications;

[assembly: Xamarin.Forms.Dependency(typeof(AgoraNavigator.iOS.Notification_IOS))]
namespace AgoraNavigator.iOS
{
    class Notification_IOS : INotification
    {
        public Notification_IOS() { }

        public void Notify(string title, string msg)
        {
            var content = new UNMutableNotificationContent();

            content.Title = title;
            content.Body = msg;

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(5, false);
            var requestID = "sampleRequest";
            var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                {
                    // Do something with error...
                }
            });
        }
    }
}
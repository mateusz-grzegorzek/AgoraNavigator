using Firebase.CloudMessaging;
using Foundation;
using System;
using UIKit;
using UserNotifications;
using Google.Maps;
using Plugin.FirebasePushNotification;

namespace AgoraNavigator.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        const string MapsApiKey = "AIzaSyB2Yxx7le70m6vrXQDM8fZd8aEnwc1RWro";
        public AudioManager AudioManager { get; set; } = new AudioManager();

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            // Get current device token
            var DeviceToken = deviceToken.Description;
            if (!string.IsNullOrWhiteSpace(DeviceToken))
            {
                DeviceToken = DeviceToken.Trim('<').Trim('>');
            }

            // Get previous device token
            var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

            // Has the token changed?
            if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(DeviceToken))
            {
                //TODO: Put your own logic here to notify your server that the device token has changed/been created!
                FirebaseMessagingClient.TokenRefresh(DeviceToken);
            }

            // Save new device token
            NSUserDefaults.StandardUserDefaults.SetString(DeviceToken, "PushDeviceToken");
        }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.Init();

            Xamarin.FormsGoogleMaps.Init(MapsApiKey);

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                                   UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                                   new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }

            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            LoadApplication(new App());
            FirebasePushNotificationManager.Initialize(options, true);

            return base.FinishedLaunching(app, options);
        }

        public override void DidEnterBackground(UIApplication application)
        {
            AudioManager.SuspendBackgroundMusic();
            AudioManager.DeactivateAudioSession();
        }

        public override void WillEnterForeground(UIApplication application)
        {
            AudioManager.ReactivateAudioSession();
            AudioManager.RestartBackgroundMusic();
        }

        public override void WillTerminate(UIApplication application)
        {
            AudioManager.StopBackgroundMusic();
            AudioManager.DeactivateAudioSession();
        }
    }
}

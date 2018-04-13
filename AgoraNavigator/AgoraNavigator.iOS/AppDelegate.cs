using Firebase.CloudMessaging;
using Foundation;
using System;
using UIKit;
using UserNotifications;

namespace AgoraNavigator.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        const string MapsApiKey = "AIzaSyB2Yxx7le70m6vrXQDM8fZd8aEnwc1RWro";
        public AudioManager AudioManager { get; set; } = new AudioManager();

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.Init();

            Xamarin.FormsGoogleMaps.Init(MapsApiKey);

            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = (IUNUserNotificationCenterDelegate)this;

                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
                    Console.WriteLine(granted);
                });
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }
            Messaging.SharedInstance.Delegate = (IMessagingDelegate)this;
            UIApplication.SharedApplication.RegisterForRemoteNotifications();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            LoadApplication(new App());
            
            return base.FinishedLaunching(app, options);
        }

        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            Console.WriteLine($"Firebase registration token: {fcmToken}");
            FirebaseMessagingClient.TokenRefresh(fcmToken);
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

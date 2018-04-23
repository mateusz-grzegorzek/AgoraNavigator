using Firebase.CloudMessaging;
using Foundation;
using System;
using UIKit;
using UserNotifications;
using Plugin.FirebasePushNotification;
using Xamarin.Forms;
using System.Threading.Tasks;
using KeyboardOverlap;

namespace AgoraNavigator.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        const string MapsApiKey = "AIzaSyB2Yxx7le70m6vrXQDM8fZd8aEnwc1RWro";
        public static AudioManager AudioManager { get; set; } = new AudioManager();

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            KeyboardOverlap.Forms.Plugin.iOSUnified.KeyboardOverlapRenderer.Init();


            Xamarin.FormsGoogleMaps.Init(MapsApiKey);

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10 or later
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
                    Console.WriteLine(granted);
                });

                UNUserNotificationCenter.Current.Delegate = this;
                Messaging.SharedInstance.Delegate = this;
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }
            UIApplication.SharedApplication.RegisterForRemoteNotifications();

            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            LoadApplication(new App());
            FirebasePushNotificationManager.Initialize(options, true);
            FirebasePushNotificationManager.CurrentNotificationPresentationOption = UNNotificationPresentationOptions.Sound |
                UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Badge;
            Reachability.InternetConnectionStatus();
            Reachability.LocalWifiConnectionStatus();
            Reachability.RemoteHostStatus();

            Reachability.ReachabilityChanged += delegate
            {
                Task.Run(async () => { await FirebaseMessagingClient.NetworkStatusChangedAsync(); });
            };
            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
            FirebaseMessagingClient.TokenRefresh(deviceToken.ToString());

            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);

        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            System.Console.WriteLine(userInfo);
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            FirebasePushNotificationManager.Connect();
        }

        public override void DidEnterBackground(UIApplication application)
        {
            AudioManager.SuspendBackgroundMusic();
            AudioManager.DeactivateAudioSession();
            FirebasePushNotificationManager.Disconnect();
        }

        public override void WillEnterForeground(UIApplication application)
        {
            AudioManager.RestartBackgroundMusic();
        }

        public override void WillTerminate(UIApplication application)
        {
            AudioManager.StopBackgroundMusic();
            AudioManager.DeactivateAudioSession();
        }
    }
}

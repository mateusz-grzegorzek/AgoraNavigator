using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Permissions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Plugin.FirebasePushNotification;
using Android.Content;
using Xamarin;
using ZXing.Mobile;
using Plugin.DeviceInfo;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace AgoraNavigator.Droid
{
    [Activity(Label = "Agora Navigator", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Forms.Init(this, bundle);
            FormsGoogleMaps.Init(this, bundle);
            LoadApplication(new App());
            FirebasePushNotificationManager.ProcessIntent(this, Intent);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            MobileBarcodeScanner.Initialize(Application);
            
            CrossDevice.Network.WhenStatusChanged().Subscribe(x =>
            {
                Task.Run(async () => { await FirebaseMessagingClient.NetworkStatusChangedAsync(); });
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}


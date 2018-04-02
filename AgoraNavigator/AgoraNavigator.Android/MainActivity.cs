using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Permissions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Plugin.FirebasePushNotification;
using Android.Content;
using Xamarin;

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
        }

        protected override void OnNewIntent(Intent intent)
        {
            FirebasePushNotificationManager.ProcessIntent(this, intent);
            base.OnNewIntent(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}


using System;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Permissions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Firebase;
using Plugin.FirebasePushNotification;
using Android.Content;

namespace AgoraNavigator.Droid
{
    [Activity(Label = "Agora Navigator", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            Xamarin.FormsGoogleMaps.Init(this, bundle);
            FirebaseApp.InitializeApp(Android.App.Application.Context);
            LoadApplication(new App());
            FirebasePushNotificationManager.ProcessIntent(Intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}


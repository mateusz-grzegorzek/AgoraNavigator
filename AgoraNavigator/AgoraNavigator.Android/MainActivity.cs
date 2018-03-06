using System;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Permissions;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;

namespace AgoraNavigator.Droid
{
    [Activity(Label = "Agora Navigator", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            Log.Debug("MainActivity", "InstanceID token: " + FirebaseInstanceId.Instance.Token);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}


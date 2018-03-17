using Android.App;
using Android.Gms.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Iid;
using Plugin.CurrentActivity;
using Plugin.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AgoraNavigator
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    class FirebaseMessagingClient : FirebaseInstanceIdService
    {
        private static FirebaseClient firebaseClient;
        public static String firebaseToken;

        public static bool IsNetworkAvailable()
        {
            bool result = false;
            NetworkInfo netInfo = new NetworkInfo();
            if(NetworkReachability.NotReachable != netInfo.InternetReachability && 
                NetworkReachability.Unknown != netInfo.InternetReachability)
            {
                result = true;
            }
            else
            {
                DependencyService.Get<INotification>().Notify("No internet connection", "Turn on the network.");
            }
            return result;
        }

        public override void OnTokenRefresh()
        {
            firebaseToken = FirebaseInstanceId.Instance.Token;
        }

        public static void InitFirebaseMessagingClient()
        {
            firebaseClient = new FirebaseClient(Configuration.FirebaseEndpoint);
        }

        public static async System.Threading.Tasks.Task SendMessage(String path, String msg)
        {  
            if(IsNetworkAvailable())
            {
                await firebaseClient.Child(path).PutAsync(msg);
            }
        }

        public static async Task<IReadOnlyCollection<FirebaseObject<T>>> SendQuery<T>(String path)
        {
            return await firebaseClient.Child(path).OnceAsync<T>();
        }

        public static async Task<T> SendSingleQuery<T>(String path)
        {
            return await firebaseClient.Child(path).OnceSingleAsync<T>();
        }
    }
}

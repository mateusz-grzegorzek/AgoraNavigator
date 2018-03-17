using Android.App;
using Firebase.Database;
using Firebase.Iid;
using Newtonsoft.Json;
using Plugin.DeviceInfo;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
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
        private static bool isRegistered = false;

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
            CrossDevice.Network.WhenStatusChanged().Subscribe(x => Device.BeginInvokeOnMainThread(() =>
            {
                Console.WriteLine("WhenStatusChanged");
                if(IsNetworkAvailable() && !isRegistered)
                {
                    Console.WriteLine("NetworkAvailable");
                    String databasePath = "/register/";
                    FirebaseMessagingClient.SendMessage(databasePath, JsonConvert.SerializeObject(FirebaseInstanceId.Instance.Token));
                    CrossFirebasePushNotification.Current.Subscribe("Agora_News");
                    CrossFirebasePushNotification.Current.Subscribe("Agora_Integration");
                    isRegistered = true;
                }
            }));                    
        }

        public static async Task SendMessage(String path, String msg)
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

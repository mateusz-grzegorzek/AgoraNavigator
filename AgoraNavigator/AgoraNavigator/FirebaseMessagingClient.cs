using AgoraNavigator.Downloads;
using AgoraNavigator.Login;
using Android.App;
using Firebase.Database;
using Firebase.Iid;
using Newtonsoft.Json;
using Plugin.DeviceInfo;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Net;
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
            return result;
        }

        public override void OnTokenRefresh()
        {
            firebaseToken = FirebaseInstanceId.Instance.Token;
            Plugin.Settings.CrossSettings.Current.AddOrUpdateValue("firebaseToken", firebaseToken);
            SubscribeForTopics(true);
        }

        public static void InitFirebaseMessagingClientAsync()
        {
            firebaseToken = Plugin.Settings.CrossSettings.Current.GetValueOrDefault("firebaseToken", FirebaseInstanceId.Instance.Token);
            Console.WriteLine("InitFirebaseMessagingClientAsync:firebaseToken=" + firebaseToken);
            firebaseClient = new FirebaseClient(Configuration.FirebaseEndpoint);
            SubscribeForTopics(false);
            CrossDevice.Network.WhenStatusChanged().Subscribe(x => Device.BeginInvokeOnMainThread(() =>
            {
                SubscribeForTopics(false);
            }));                    
        }

        private static void SubscribeForTopics(bool tokenChanged)
        {
            if (tokenChanged || (IsNetworkAvailable() && !isRegistered))
            {
                Console.WriteLine("SubscribeForTopics");
                String databasePath = "/register/";
                if(SendMessage(databasePath, JsonConvert.SerializeObject(firebaseToken)))
                {
                    CrossFirebasePushNotification.Current.Subscribe("Agora_News");
                    CrossFirebasePushNotification.Current.Subscribe("Agora_Integration");
                    if (Users.isUserLogged)
                    {
                        CrossFirebasePushNotification.Current.Subscribe("User_" + Users.loggedUser.Id);
                    }
                    isRegistered = true;
                }
                else
                {
                    DependencyService.Get<INotification>().Notify("No internet connection", "Turn on the network to register for Agora News!");
                }
            }
        }

        public static bool SendMessage(String path, String msg)
        {
            bool result = false;
            if(IsNetworkAvailable())
            {
                try
                {
                    firebaseClient.Child(path).PutAsync(msg);
                    result = true;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            return result;
        }

        public static async Task<IReadOnlyCollection<FirebaseObject<T>>> SendQuery<T>(String path)
        {
            return await firebaseClient.Child(path).OnceAsync<T>();
        }

        public static async Task<T> SendSingleQuery<T>(String path)
        {
            return await firebaseClient.Child(path).OnceSingleAsync<T>();
        }

        public static void DownloadFileAsync(String url, String fileName)
        {
            string pathToFileFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Download/AgoraData";
            Java.IO.File dir = new Java.IO.File(pathToFileFolder);
            if (!dir.Exists())
            {
                dir.Mkdirs();
            }  
            string pathToFile = pathToFileFolder + "/" + fileName;

            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, pathToFile);

            Device.BeginInvokeOnMainThread(() =>
            {
                DownloadsPage.downloadsMasterPage.AddNewFile(url, fileName);
            });
        }
    }
}

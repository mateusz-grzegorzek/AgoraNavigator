using AgoraNavigator.Downloads;
using AgoraNavigator.Login;
using AgoraNavigator.Schedule;
using Firebase.Database;
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
    class FirebaseMessagingClient
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

        public static void TokenRefresh(String token)
        {
            firebaseToken = token;
            Plugin.Settings.CrossSettings.Current.AddOrUpdateValue("firebaseToken", firebaseToken);
            SubscribeForTopics(true);
        }

        public static void InitFirebaseMessagingClientAsync()
        {
            firebaseToken = Plugin.Settings.CrossSettings.Current.GetValueOrDefault("firebaseToken", "");
            Console.WriteLine("InitFirebaseMessagingClientAsync:firebaseToken=" + firebaseToken);
            firebaseClient = new FirebaseClient(Configuration.FirebaseEndpoint);
            SubscribeForTopics(false);
            CrossDevice.Network.WhenStatusChanged().Subscribe(x => Device.BeginInvokeOnMainThread(() =>
            {
                SubscribeForTopics(false);
                SchedulePage.scheduleDaysPage.FetchScheduleAsync();
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

        public static void AddUrlToDownloads(String url, String fileName)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DownloadsPage.downloadsMasterPage.AddNewFile(url, fileName);
            });
        }
    }
}

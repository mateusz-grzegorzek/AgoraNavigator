using Firebase.Database;
using Newtonsoft.Json;
using Plugin.FirebasePushNotification;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AgoraNavigator
{
    class FirebaseMessagingClient
    {
        private static FirebaseClient firebaseClient;
        public static String firebaseToken;

        public static bool IsNetworkAvailable()
        {
            return DependencyService.Get<INetworkInfo>().IsNetworkAvailable();
        }

        public static void TokenRefresh(String token)
        {
            firebaseToken = token;
            CrossSettings.Current.AddOrUpdateValue("firebaseToken", firebaseToken);
            SubscribeForTopics(true);
        }

        public static void InitFirebaseMessagingClientAsync()
        {
            firebaseToken = Plugin.Settings.CrossSettings.Current.GetValueOrDefault("firebaseToken", "");
            Console.WriteLine("InitFirebaseMessagingClientAsync:firebaseToken=" + firebaseToken);
            firebaseClient = new FirebaseClient(Configuration.FirebaseEndpoint);
            SubscribeForTopics(false);
            DependencyService.Get<INetworkInfo>().WhenStatusChanged();
        }

        public static void SubscribeForTopics(bool tokenChanged)
        {
            bool isRegistered = CrossSettings.Current.GetValueOrDefault("isRegistered", false);
            if (tokenChanged || (IsNetworkAvailable() && !isRegistered))
            {
                Console.WriteLine("SubscribeForTopics");
                String databasePath = "/register/";
                if(SendMessage(databasePath, JsonConvert.SerializeObject(firebaseToken)))
                {
                    CrossFirebasePushNotification.Current.Subscribe("Agora");
                    CrossSettings.Current.AddOrUpdateValue("isRegistered", true);
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
    }
}

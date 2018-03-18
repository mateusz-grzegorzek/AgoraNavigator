using Android.App;
using Android.Gms.Tasks;
using Firebase.Database;
using Firebase.Iid;
using Firebase.Storage;
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
            Plugin.Settings.CrossSettings.Current.AddOrUpdateValue("firebaseToken", firebaseToken);
        }

        public static async System.Threading.Tasks.Task InitFirebaseMessagingClientAsync()
        {
            firebaseToken = Plugin.Settings.CrossSettings.Current.GetValueOrDefault("firebaseToken", FirebaseInstanceId.Instance.Token);
            Console.WriteLine("InitFirebaseMessagingClientAsync:firebaseToken=" + firebaseToken);
            firebaseClient = new FirebaseClient(Configuration.FirebaseEndpoint);
            await SubscribeForTopicsAsync();
            CrossDevice.Network.WhenStatusChanged().Subscribe(x => Device.BeginInvokeOnMainThread(async () =>
            {
                await SubscribeForTopicsAsync();
            }));                    
        }

        private static async System.Threading.Tasks.Task SubscribeForTopicsAsync()
        {
            if (IsNetworkAvailable() && !isRegistered)
            {
                Console.WriteLine("SubscribeForTopics");
                String databasePath = "/register/";
                await SendMessage(databasePath, JsonConvert.SerializeObject(firebaseToken));
                CrossFirebasePushNotification.Current.Subscribe("Agora_News");
                CrossFirebasePushNotification.Current.Subscribe("Agora_Integration");
                isRegistered = true;
            }
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

        public static void DownloadFileAsync(String path)
        {
            FirebaseStorage storage = FirebaseStorage.Instance;
            StorageReference storageRef = storage.GetReferenceFromUrl("gs://agora-ada18.appspot.com");
            StorageReference fileRef = storageRef.Child(path);

            Java.IO.File dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Download/");
            if (!dir.Exists())
                dir.Mkdirs();

            Java.IO.File file = new Java.IO.File(dir, path);
            file.CreateNewFile();
            fileRef.GetFile(file).AddOnSuccessListener(new OnSuccessListener<FileDownloadTask.TaskSnapshot>());
        }
    }

    internal class OnSuccessListener<T> : Activity, IOnSuccessListener
    {
        public void OnSuccess(Java.Lang.Object result)
        {
            Console.WriteLine("Download completed!");
        }
    }
}

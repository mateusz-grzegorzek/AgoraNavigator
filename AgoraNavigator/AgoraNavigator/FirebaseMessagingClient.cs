using AgoraNavigator.Login;
using Android.App;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Iid;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgoraNavigator
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    class FirebaseMessagingClient : FirebaseInstanceIdService
    {
        private static FirebaseClient firebaseClient;
        public static String firebaseToken;

        public override void OnTokenRefresh()
        {
            firebaseToken = FirebaseInstanceId.Instance.Token;
        }

        public static async Task CreateUserProfile(String token)
        {
            await SendMessage("users/" + Users.loggedUser.Id, token);
        }
        public static void SignInWithCustomToken(String token)
        {
            FirebaseAuth.Instance.SignInWithCustomToken(token);
        }

        public static void InitFirebaseMessagingClient()
        {
            firebaseClient = new FirebaseClient(Configuration.FirebaseEndpoint);
        }

        public static async Task SendMessage(String path, String msg)
        {  
            await firebaseClient.Child(path).PutAsync(msg);
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

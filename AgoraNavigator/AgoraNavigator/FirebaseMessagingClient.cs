using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgoraNavigator
{
    class FirebaseMessagingClient
    {
        private static FirebaseClient firebaseClient;
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
    }
}

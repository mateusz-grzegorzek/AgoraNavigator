using AgoraNavigator.Login;
using AgoraNavigator.Tasks;
using Firebase.Auth;
using Plugin.FirebasePushNotification;
using Plugin.FirebasePushNotification.Abstractions;
using System;
using Xamarin.Forms;

namespace AgoraNavigator
{
    public class App : Application
    {
        class Message
        {
            public String head;
            public String body;
        }

        public App()
        {
            Console.WriteLine("Application started!");
            FirebaseMessagingClient.InitFirebaseMessagingClient();
            GameTask.AddTasks();
            CrossFirebasePushNotification.Current.OnNotificationReceived += FirebasePushNotificationDataEventHandler;
            MainPage = new StartingPage();
        }

        async void FirebasePushNotificationDataEventHandler(object source, FirebasePushNotificationDataEventArgs e)
        {
            Console.WriteLine("OnNotificationReceived:");
            foreach (var data in e.Data)
            {
                Console.WriteLine($"{data.Key} : {data.Value}");
                if (data.Value.ToString().Contains("AEGEE_Army"))
                {
                    Console.WriteLine("AEGEE_Army");
                    await GameTask.closeTask(2);
                }
                else if(((Message)data.Value).head != null && ((Message)data.Value).head == "customToken")
                {
                    FirebaseMessagingClient.SignInWithCustomToken(((Message)data.Value).body);
                }
            }
            var userAS = FirebaseAuth.Instance.CurrentUser;
        }
    }
}


using AgoraNavigator.Login;
using Plugin.FirebasePushNotification;
using System;
using Xamarin.Forms;

namespace AgoraNavigator
{
    public class App : Application
    {
        public App()
        {
            Console.WriteLine("Application started!");
            FirebaseMessagingClient.InitFirebaseMessagingClient();
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                Console.WriteLine("OnNotificationReceived:");
                foreach (var data in p.Data)
                {
                    Console.WriteLine($"{data.Key} : {data.Value}");
                }
            };
            MainPage = new StartingPage();
        }
    }
}


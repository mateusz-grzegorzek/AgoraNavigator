using AgoraNavigator.Login;
using AgoraNavigator.Tasks;
using Newtonsoft.Json;
using Plugin.FirebasePushNotification;
using Plugin.FirebasePushNotification.Abstractions;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AgoraNavigator
{
    public class App : Application
    {    
        public App()
        {
            Console.WriteLine("Application started!");
            FirebaseMessagingClient.InitFirebaseMessagingClient();
            CrossFirebasePushNotification.Current.Subscribe("Agora_News");
            CrossFirebasePushNotification.Current.Subscribe("Agora_Integration");
            GameTask.AddTasks();
            CrossFirebasePushNotification.Current.OnNotificationReceived += FirebasePushNotificationDataEventHandler;
            MainPage = new StartingPage();
        }

        async void FirebasePushNotificationDataEventHandler(object source, FirebasePushNotificationDataEventArgs e)
        {
            Console.WriteLine("OnNotificationReceived");
            String title = e.Data["title"].ToString();
            Console.WriteLine("OnNotificationReceived:title:" + title);
            if (title == "Login state")
            {
                GameLoginNavPage.gameLoginPage.Login(e.Data);
            }
            else if(title == "AEGEE Army")
            {
                Console.WriteLine("AEGEE Army task done!");
                await GameTask.closeTask(2);
            }
        }
    }
}


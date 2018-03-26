using AgoraNavigator.Menu;
using AgoraNavigator.Tasks;
using Plugin.FirebasePushNotification;
using Plugin.FirebasePushNotification.Abstractions;
using System;
using Xamarin.Forms;

namespace AgoraNavigator
{
    public class App : Application
    {
        public static MainPage mainPage;
        public App()
        {
            Console.WriteLine("Application started!");
            Console.WriteLine("Application started:firebaseToken="+ FirebaseMessagingClient.firebaseToken);
            GameTask.AddTasks();
            FirebaseMessagingClient.InitFirebaseMessagingClientAsync();
            CrossFirebasePushNotification.Current.OnNotificationReceived += FirebasePushNotificationDataEventHandler;
            CrossFirebasePushNotification.Current.OnNotificationOpened += FirebasePushNotificationDataEventHandler;
            mainPage = new MainPage();
            MainPage = mainPage;
        }

        void FirebasePushNotificationDataEventHandler(object source, FirebasePushNotificationDataEventArgs e)
        {
            Console.WriteLine("OnNotificationReceived");
            try
            {
                String title = e.Data["title"].ToString();
                Console.WriteLine("OnNotificationReceived:title:" + title);
                if (title == "Download")
                {
                    String url = e.Data["url"].ToString();
                    String fileName = e.Data["fileName"].ToString();
                    FirebaseMessagingClient.AddUrlToDownloads(url, fileName);
                }
            }
            catch(Exception err)
            {
                Console.WriteLine("OnNotificationReceived:" + err.ToString());
            }
        }
        void FirebasePushNotificationDataEventHandler(object source, FirebasePushNotificationResponseEventArgs e)
        {
            Console.WriteLine("OnNotificationReceived");
            try
            {
                String title = e.Data["title"].ToString();
                Console.WriteLine("OnNotificationReceived:title:" + title);
                if (title == "Download")
                {
                    String url = e.Data["url"].ToString();
                    String fileName = e.Data["fileName"].ToString();
                    FirebaseMessagingClient.AddUrlToDownloads(url, fileName);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("OnNotificationReceived:" + err.ToString());
            }
        }
        
    }
}


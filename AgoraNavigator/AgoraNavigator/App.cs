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
                if (title == "Login state")
                {
                    GameLoginNavPage.gameLoginPage.Login(e.Data);
                }
                else if (title == "Download")
                {
                    String url = e.Data["url"].ToString();
                    String fileName = e.Data["fileName"].ToString();
                    FirebaseMessagingClient.DownloadFileAsync(url, fileName);
                }
                else if (title == "AEGEE Army")
                {
                    Console.WriteLine("AEGEE Army task done!");
                    GameTask.CloseTask(2);
                }
            }
            catch(Exception err)
            {
                Console.WriteLine("OnNotificationReceived:" + err.ToString());
            }
        }
    }
}


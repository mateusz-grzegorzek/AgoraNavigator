using AgoraNavigator.Login;
using AgoraNavigator.Tasks;
using Plugin.FirebasePushNotification;
using Plugin.FirebasePushNotification.Abstractions;
using System;
using Xamarin.Forms;

namespace AgoraNavigator
{
    public class App : Application
    {    
        public App()
        {
            Console.WriteLine("Application started!");
            Console.WriteLine("Application started:firebaseToken="+ FirebaseMessagingClient.firebaseToken);
            GameTask.AddTasks();
            CrossFirebasePushNotification.Current.OnNotificationReceived += FirebasePushNotificationDataEventHandler;
            MainPage = new StartingPage();
        }

        async void FirebasePushNotificationDataEventHandler(object source, FirebasePushNotificationDataEventArgs e)
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
                    String path = e.Data["body"].ToString();
                    FirebaseMessagingClient.DownloadFileAsync(path);
                }
                else if (title == "AEGEE Army")
                {
                    Console.WriteLine("AEGEE Army task done!");
                    await GameTask.CloseTask(2);
                }
            }
            catch(Exception err)
            {
                Console.WriteLine("OnNotificationReceived:" + err.ToString());
            }
        }
    }
}


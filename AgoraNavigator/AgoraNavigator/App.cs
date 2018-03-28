using AgoraNavigator.Menu;
using AgoraNavigator.Schedule;
using AgoraNavigator.Tasks;
using Plugin.FirebasePushNotification;
using Plugin.FirebasePushNotification.Abstractions;
using System;
using System.Collections.Generic;
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
            FirebasePushNotificationDataEventHandler(e.Data);
        }
        void FirebasePushNotificationDataEventHandler(object source, FirebasePushNotificationResponseEventArgs e)
        {
            FirebasePushNotificationDataEventHandler(e.Data);
        }

        void FirebasePushNotificationDataEventHandler(IDictionary<string, object> data)
        {
            Console.WriteLine("OnNotificationReceived");
            try
            {
                if (data.Keys.Contains("download"))
                {
                    String url = data["url"].ToString();
                    String fileName = data["fileName"].ToString();
                    FirebaseMessagingClient.AddUrlToDownloads(url, fileName);
                }
                if(data.Keys.Contains("changePage"))
                {
                    mainPage.SetStartedPage(data["changePage"].ToString());
                }
                if (data.Keys.Contains("scheduleUpdate"))
                {
                    SchedulePage.scheduleDaysPage.FetchScheduleAsync(true);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("OnNotificationReceived:" + err.ToString());
            }
        }
        
    }
}


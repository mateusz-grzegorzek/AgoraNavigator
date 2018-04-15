using AgoraNavigator.Downloads;
using AgoraNavigator.Menu;
using AgoraNavigator.Schedule;
using AgoraNavigator.Tasks;
using Plugin.FirebasePushNotification;
using Plugin.FirebasePushNotification.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AgoraNavigator
{
    public class App : Application
    {
        public static MainPage mainPage;
        public App()
        {
            Console.WriteLine("Application started:firebaseToken="+ FirebaseMessagingClient.firebaseToken);
            GameTask.InitTasks();
            Beacons.InitBeaconScanner();
            FirebaseMessagingClient.InitFirebaseMessagingClientAsync();
            mainPage = new MainPage();
            MainPage = mainPage;
        }

        protected override void OnStart()
        {
            CrossFirebasePushNotification.Current.OnNotificationReceived += FirebasePushNotificationDataEventHandlerAsync;
            CrossFirebasePushNotification.Current.OnNotificationOpened += FirebasePushNotificationDataEventHandlerAsync;
        }

        void FirebasePushNotificationDataEventHandlerAsync(object source, FirebasePushNotificationDataEventArgs e)
        {
            FirebasePushNotificationDataEventHandlerAsync(e.Data);
        }
        void FirebasePushNotificationDataEventHandlerAsync(object source, FirebasePushNotificationResponseEventArgs e)
        {
            FirebasePushNotificationDataEventHandlerAsync(e.Data);
        }

        void FirebasePushNotificationDataEventHandlerAsync(IDictionary<string, object> data)
        {
            Console.WriteLine("OnNotificationReceived");
            try
            {
                if(data.Keys.Contains("changePage"))
                {
                    mainPage.SetStartedPage(data["changePage"].ToString());
                }
                if (data.Keys.Contains("scheduleUpdate"))
                {
                    Task.Run(async () => { await SchedulePage.scheduleDaysPage.FetchScheduleAsync(true); });
                }
                if (data.Keys.Contains("downloadUpdate"))
                {
                    Task.Run(async () => { await DownloadsPage.downloadsMasterPage.FetchDownloadFilesAsync(true); });
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("OnNotificationReceived:" + err.ToString());
            }
        }
        
    }
}


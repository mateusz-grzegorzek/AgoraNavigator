using AgoraNavigator.Downloads;
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

        async void FirebasePushNotificationDataEventHandlerAsync(object source, FirebasePushNotificationDataEventArgs e)
        {
            await FirebasePushNotificationDataEventHandlerAsync(e.Data);
        }
        async void FirebasePushNotificationDataEventHandlerAsync(object source, FirebasePushNotificationResponseEventArgs e)
        {
            await FirebasePushNotificationDataEventHandlerAsync(e.Data);
        }

        async System.Threading.Tasks.Task FirebasePushNotificationDataEventHandlerAsync(IDictionary<string, object> data)
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
                    await SchedulePage.scheduleDaysPage.FetchScheduleAsync(true);
                }
                if (data.Keys.Contains("downloadUpdate"))
                {
                    await DownloadsPage.downloadsMasterPage.FetchDownloadFilesAsync(true);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("OnNotificationReceived:" + err.ToString());
            }
        }
        
    }
}


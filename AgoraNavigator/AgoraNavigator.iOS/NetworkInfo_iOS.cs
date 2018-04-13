using System;
using AgoraNavigator;
using AgoraNavigator.Schedule;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(Reachability.AgoraNavigator.iOS.NetworkInfo_iOS))]
namespace Reachability
{
    namespace AgoraNavigator.iOS
    {
        public class NetworkInfo_iOS : INetworkInfo
        {
            public bool IsNetworkAvailable()
            {
                return Reachability.IsHostReachable("http://google.com");
            }

            public void WhenStatusChanged()
            {
                Reachability.InternetConnectionStatus();
                Reachability.LocalWifiConnectionStatus();
                Reachability.RemoteHostStatus();

                Reachability.ReachabilityChanged += delegate
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        FirebaseMessagingClient.SubscribeForTopics(false);
                        SchedulePage.scheduleDaysPage.FetchScheduleAsync();
                    });
                };;
            }
        }
    }
}

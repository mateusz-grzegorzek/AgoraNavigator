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
                bool result = false;
                NetworkStatus internetStatus = Reachability.InternetConnectionStatus();
                if (internetStatus != NetworkStatus.NotReachable)
                {
                    result = true;
                }
                return result;
            }

            public void WhenStatusChanged()
            {
                // TODO Not working for now
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

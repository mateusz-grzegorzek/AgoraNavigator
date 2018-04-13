using AgoraNavigator.Downloads;
using AgoraNavigator.Schedule;
using Plugin.DeviceInfo;
using System;
using System.Reactive.Linq;
using Xamarin.Forms;

[assembly: Dependency(typeof(AgoraNavigator.Droid.NetworkInfo_Android))]
namespace AgoraNavigator.Droid
{
    class NetworkInfo_Android : INetworkInfo
    {
        public bool IsNetworkAvailable()
        {
            bool result = false;
            NetworkInfo netInfo = new NetworkInfo();
            if (NetworkReachability.NotReachable != netInfo.InternetReachability &&
                NetworkReachability.Unknown != netInfo.InternetReachability)
            {
                result = true;
            }
            return result;
        }

        public void WhenStatusChanged()
        {
            CrossDevice.Network.WhenStatusChanged().Subscribe(x => Device.BeginInvokeOnMainThread(async () =>
            {
                FirebaseMessagingClient.SubscribeForTopics(false);
                await SchedulePage.scheduleDaysPage.FetchScheduleAsync();
                await DownloadsPage.downloadsMasterPage.FetchDownloadFilesAsync();
            }));
        }
    }
}


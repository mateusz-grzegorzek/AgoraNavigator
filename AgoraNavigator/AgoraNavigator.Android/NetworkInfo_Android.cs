using Plugin.DeviceInfo;
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
    }
}


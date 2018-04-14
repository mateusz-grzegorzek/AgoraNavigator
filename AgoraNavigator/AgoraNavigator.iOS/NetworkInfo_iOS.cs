using Xamarin.Forms;

[assembly: Dependency(typeof(AgoraNavigator.iOS.NetworkInfo_iOS))]
namespace AgoraNavigator.iOS
{
    public class NetworkInfo_iOS : INetworkInfo
    {
        public bool IsNetworkAvailable()
        {
            return Reachability.IsHostReachable("http://google.com");
        }
    }
}


namespace AgoraNavigator
{
    public interface INetworkInfo
    {
        bool IsNetworkAvailable();

        void WhenStatusChanged();
    }
}

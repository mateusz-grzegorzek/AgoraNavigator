using Android.App;
using Firebase.Iid;

namespace AgoraNavigator.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    class FirebaseMessagingClient_Android : FirebaseInstanceIdService
    {
        public override void OnTokenRefresh()
        {
            FirebaseMessagingClient.TokenRefresh(FirebaseInstanceId.Instance.Token);
        }
    }
}
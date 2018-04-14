
using AgoraNavigator.Popup;
using Xamarin.Forms;

[assembly: Dependency(typeof(AgoraNavigator.iOS.Popup_iOS))]
namespace AgoraNavigator.iOS
{
    class Popup_iOS : IPopup
    {
        public void ShowPopup(string title, string body, bool succes)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await App.mainPage.DisplayAlert(title, body, "OK");
            });
        }
    }
}

using Rg.Plugins.Popup.Extensions;
using AgoraNavigator.Popup;
using Xamarin.Forms;

[assembly: Dependency(typeof(AgoraNavigator.Droid.Popup_Android))]
namespace AgoraNavigator.Droid
{
    class Popup_Android : IPopup
    {
        public void ShowPopup(string title, string body, bool succes)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                SimplePopup popup = new SimplePopup(title, body, succes);
                await App.mainPage.Navigation.PushPopupAsync(popup);
            });
        }
    }
}

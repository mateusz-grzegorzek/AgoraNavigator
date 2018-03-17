using Firebase.Iid;
using Xamarin.Forms;

namespace AgoraNavigator.Info
{
    public class InfoPage : NavigationPage
    {
        public static InfoMasterPage infoMasterPage;

        public InfoPage()
        {
            infoMasterPage = new InfoMasterPage();
            Navigation.PushAsync(infoMasterPage);
        }
    }

    public class InfoMasterPage : ContentPage
    {
        public InfoMasterPage()
        {
            Title = "Important info";
            Content = new StackLayout
            {
                Children = {
                   new Label { Text = FirebaseInstanceId.Instance.Token }
                }
            };
        }
    }
}
using Xamarin.Forms;

namespace AgoraNavigator.Menu
{
    public class WelcomePage : NavigationPage
    {
        public static WelcomeMasterPage welcomeMasterPage;

        public WelcomePage()
        {
            welcomeMasterPage = new WelcomeMasterPage();
            BarTextColor = AgoraColor.Blue;
            Navigation.PushAsync(welcomeMasterPage);
        }
    }

    public class WelcomeMasterPage : ContentPage
    {
        public WelcomeMasterPage()
        {
            Title = "Hello dear AEGEEan!";
            BackgroundImage = "Welcome.png";
        }
    }
}
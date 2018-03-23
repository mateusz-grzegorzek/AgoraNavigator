using Xamarin.Forms;

namespace AgoraNavigator.Menu
{
    public class WelcomePage : NavigationPage
    {
        public static WelcomeMasterPage welcomeMasterPage;

        public WelcomePage()
        {
            welcomeMasterPage = new WelcomeMasterPage();
            Navigation.PushAsync(welcomeMasterPage);
        }
    }

    public class WelcomeMasterPage : ContentPage
    {
        public WelcomeMasterPage()
        {
            Title = "Hello dear AEGEEan!";
            Content = new StackLayout
            {
                Children = {
                   new Label { Text = "Hello dear AEGEEan!" }
                }
            };
        }
    }
}
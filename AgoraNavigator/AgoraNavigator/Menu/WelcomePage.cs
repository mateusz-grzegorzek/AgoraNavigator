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
            Image backgroundImage = new Image
            {
                Source = "Welcome.png",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            StackLayout stackLayout = new StackLayout();
            stackLayout.Children.Add(backgroundImage);
            BackgroundColor = AgoraColor.DarkBlue;
            Content = stackLayout;
        }
    }
}
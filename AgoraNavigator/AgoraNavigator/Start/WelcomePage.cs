using AgoraNavigator.Menu;
using Firebase.Iid;
using Newtonsoft.Json;
using Plugin.FirebasePushNotification;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AgoraNavigator.Login
{
    class StartingPage : NavigationPage
    {
        WelcomePage welcomePage;

        public StartingPage()
        {
            Console.WriteLine("StartingPage");
            welcomePage = new WelcomePage();
            Navigation.PushAsync(welcomePage);
        }
    }

    public class WelcomePage : ContentPage
    {
        public static MainPage mainPage;

        public WelcomePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            Label welcomeLabel = new Label {Text = "Welcome in Agora Navigator!"};

            Button welcomeButton = new Button{Text = "Enter Agora Navigator!"};
            welcomeButton.Clicked += OnWelcomeButtonClickedAsync;
            welcomeButton.BackgroundColor = Color.Transparent;
            welcomeButton.TextColor = Color.Blue;
            welcomeButton.BorderWidth = 1;
            welcomeButton.BorderColor = Color.Black;

            AbsoluteLayout.SetLayoutBounds(welcomeLabel, new Rectangle(.5, .1, .5, .1));
            AbsoluteLayout.SetLayoutBounds(welcomeButton, new Rectangle(.5, .9, .25, .2));
            AbsoluteLayout.SetLayoutFlags(welcomeLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(welcomeButton, AbsoluteLayoutFlags.All);

            AbsoluteLayout simpleLayout = new AbsoluteLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            simpleLayout.Children.Add(welcomeLabel);
            simpleLayout.Children.Add(welcomeButton);
            Content = simpleLayout;
            
        }

        public async void OnWelcomeButtonClickedAsync(object sender, EventArgs e)
        {
            Console.WriteLine("OnWelcomeButtonClicked");
            await FirebaseMessagingClient.InitFirebaseMessagingClientAsync();
            mainPage = new MainPage();
            await Navigation.PushAsync(mainPage);
        }
    }
}
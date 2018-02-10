using AgoraNavigator.Menu;
using System;
using Xamarin.Forms;

namespace AgoraNavigator.Login
{
    class StartingPage : NavigationPage
    {
        WelcomePage welcomePage;

        public StartingPage()
        {
            Console.WriteLine("StartingPage");
            Users.InitUsers();
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
            Label welcomeLabel = new Label();
            welcomeLabel.Text = "Welcome in Agora Navigator!";

            Button welcomeButton = new Button();
            welcomeButton.Text = "Enter Agora Navigator!";
            welcomeButton.Clicked += OnWelcomeButtonClicked;
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

        public void OnWelcomeButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("OnWelcomeButtonClicked");
            mainPage = new MainPage();
            Navigation.PushAsync(mainPage);
        }
    }
}
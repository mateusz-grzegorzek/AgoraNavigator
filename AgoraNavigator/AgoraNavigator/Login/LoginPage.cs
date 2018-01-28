using AgoraNavigator.Menu;
using Plugin.DeviceInfo;
using System;
using Xamarin.Forms;

namespace AgoraNavigator.Login
{
    class StartingPage : NavigationPage
    {
        LoginPage loginPage;

        public StartingPage()
        {
            Console.WriteLine("StartingPage");
            Users.InitUsers();
            loginPage = new LoginPage();
            Navigation.PushAsync(loginPage);
        }
    }

    public class LoginPage : ContentPage
    {
        MainPage mainPage;
        Entry idEntry;
        Entry pinEntry;

        public LoginPage()
        {
            Console.WriteLine("LoginPage");
            mainPage = new MainPage();
            NavigationPage.SetHasNavigationBar(this, false);

            int screenHeight = CrossDevice.Hardware.ScreenHeight;
            int screenWidth = CrossDevice.Hardware.ScreenWidth;

            Image backgroundImage = new Image();
            backgroundImage.Source = "StartingPage.png";

            Button loginButton = new Button();
            loginButton.Text = "Login";
            loginButton.Clicked += OnLoginButtonClicked;
            loginButton.BackgroundColor = Color.Transparent;
            loginButton.TextColor = Color.Blue;
            loginButton.BorderWidth = 1;
            loginButton.BorderColor = Color.Black;

            Label welcomeLabel = new Label();
            welcomeLabel.Text = "Welcome in Agora Navigator!";
            Label idLabel = new Label();
            idLabel.Text = "Enter your ID:";
            Label pinLabel = new Label();
            pinLabel.Text = "Enter your PIN:";

            idEntry = new Entry();
            pinEntry = new Entry();

            AbsoluteLayout simpleLayout = new AbsoluteLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            AbsoluteLayout.SetLayoutBounds(welcomeLabel, new Rectangle(.5, .1, .5, .1));
            AbsoluteLayout.SetLayoutBounds(idLabel,  new Rectangle(.5, .4, .5, .1));
            AbsoluteLayout.SetLayoutBounds(idEntry,  new Rectangle(.5,.45, .5,.08));
            AbsoluteLayout.SetLayoutBounds(pinLabel, new Rectangle(.5, .6, .5, .1));
            AbsoluteLayout.SetLayoutBounds(pinEntry, new Rectangle(.5,.65, .5,.08));
            AbsoluteLayout.SetLayoutBounds(loginButton, new Rectangle(.5, .9, .25, .12));

            AbsoluteLayout.SetLayoutFlags(welcomeLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(idLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(idEntry, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(pinLabel, AbsoluteLayoutFlags.All);   
            AbsoluteLayout.SetLayoutFlags(pinEntry, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(loginButton, AbsoluteLayoutFlags.All);

            //simpleLayout.Children.Add(backgroundImage);

            simpleLayout.Children.Add(welcomeLabel);
            simpleLayout.Children.Add(idLabel);
            simpleLayout.Children.Add(idEntry);
            simpleLayout.Children.Add(pinLabel);
            simpleLayout.Children.Add(pinEntry);
            simpleLayout.Children.Add(loginButton);
            Content = simpleLayout;
        }

        public void OnLoginButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("OnLoginButtonClicked");
            Console.WriteLine("OnLoginButtonClicked:idEntry=" + idEntry.Text + ", pinEntry=" + pinEntry);
            int id  = Convert.ToInt32(idEntry.Text);
            int pin = Convert.ToInt32(pinEntry.Text);
            foreach (User user in Users.users)
            {
                if(id == user.Id)
                {
                    Console.WriteLine("OnLoginButtonClicked:id=" + id);
                    if(pin == user.Pin)
                    {
                        Console.WriteLine("OnLoginButtonClicked:pin=" + pin);
                        Navigation.PushAsync(mainPage);
                    }
                    else
                    {
                        Console.WriteLine("OnLoginButtonClicked:Wrong pin!");
                    }
                    return;
                }
            }
            Console.WriteLine("OnLoginButtonClicked:User with id=" + id + " doesn't exist!");
        }
    }
}
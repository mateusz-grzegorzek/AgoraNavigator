using AgoraNavigator.Login;
using AgoraNavigator.Menu;
using Plugin.DeviceInfo;
using System;
using Xamarin.Forms;

namespace AgoraNavigator.Tasks
{
    public class GameLoginNavPage : NavigationPage
    {
        GameLoginPage gameLoginPage;
        public GameLoginNavPage()
        {
            gameLoginPage = new GameLoginPage();
            Navigation.PushAsync(gameLoginPage);
        }
    }

    public class GameLoginPage : ContentPage
    {
        Entry idEntry;
        Entry pinEntry;

        public GameLoginPage()
        {
            Console.WriteLine("GameLoginPage");

            int screenHeight = CrossDevice.Hardware.ScreenHeight;
            int screenWidth = CrossDevice.Hardware.ScreenWidth;

            Button loginButton = new Button();
            loginButton.Text = "Login";
            loginButton.Clicked += OnLoginButtonClicked;
            loginButton.BackgroundColor = Color.Transparent;
            loginButton.TextColor = Color.Blue;
            loginButton.BorderWidth = 1;
            loginButton.BorderColor = Color.Black;


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

            AbsoluteLayout.SetLayoutBounds(idLabel, new Rectangle(.5, .4, .5, .1));
            AbsoluteLayout.SetLayoutBounds(idEntry, new Rectangle(.5, .45, .5, .08));
            AbsoluteLayout.SetLayoutBounds(pinLabel, new Rectangle(.5, .6, .5, .1));
            AbsoluteLayout.SetLayoutBounds(pinEntry, new Rectangle(.5, .65, .5, .08));
            AbsoluteLayout.SetLayoutBounds(loginButton, new Rectangle(.5, .9, .25, .12));

            AbsoluteLayout.SetLayoutFlags(idLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(idEntry, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(pinLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(pinEntry, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(loginButton, AbsoluteLayoutFlags.All);

            simpleLayout.Children.Add(idLabel);
            simpleLayout.Children.Add(idEntry);
            simpleLayout.Children.Add(pinLabel);
            simpleLayout.Children.Add(pinEntry);
            simpleLayout.Children.Add(loginButton);
            Content = simpleLayout;
        }

        public async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            await Users.InitUserData(Users.users[0]);
            WelcomePage.mainPage.UserLoggedSuccessfully();
            /*
            Console.WriteLine("OnLoginButtonClicked");
            Console.WriteLine("OnLoginButtonClicked:idEntry=" + idEntry.Text + ", pinEntry=" + pinEntry);
            int id = Convert.ToInt32(idEntry.Text);
            int pin = Convert.ToInt32(pinEntry.Text);
            foreach (User user in Users.users)
            {
                if (id == user.Id)
                {
                    Console.WriteLine("OnLoginButtonClicked:id=" + id);
                    if (pin == user.Pin)
                    {
                        Console.WriteLine("OnLoginButtonClicked:pin=" + pin);
                        await DisplayAlert("Login", "Succes!", "Ok");
                        await Users.InitUserData(user);
                        WelcomePage.mainPage.UserLoggedSuccessfully();
                    }
                    else
                    {
                        Console.WriteLine("OnLoginButtonClicked:Wrong pin!");
                        await DisplayAlert("Login", "Wrong pin number!", "Ok");
                    }
                    return;
                }
            }
            Console.WriteLine("OnLoginButtonClicked:User with id=" + id + " doesn't exist!");
            await DisplayAlert("Login", "User with id=" + id + " doesn't exist!", "Ok");
            */
        }
    }
}
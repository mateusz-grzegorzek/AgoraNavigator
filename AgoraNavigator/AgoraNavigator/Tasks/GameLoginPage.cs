using AgoraNavigator.Login;
using Newtonsoft.Json;
using Plugin.DeviceInfo;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AgoraNavigator.Tasks
{
    
    public class GameLoginNavPage : NavigationPage
    {
        public static GameLoginPage gameLoginPage;
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
        Label infoLabel;

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
            infoLabel = new Label();
            infoLabel.Text = "Please enter your login ID and PIN number:";

            idEntry = new Entry();
            pinEntry = new Entry();

            idEntry.Text = "1";
            pinEntry.Text = "1";

            AbsoluteLayout simpleLayout = new AbsoluteLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            AbsoluteLayout.SetLayoutBounds(infoLabel, new Rectangle(.4, .2, .5, .1));
            AbsoluteLayout.SetLayoutBounds(idLabel, new Rectangle(.5, .4, .5, .1));
            AbsoluteLayout.SetLayoutBounds(idEntry, new Rectangle(.5, .45, .5, .08));
            AbsoluteLayout.SetLayoutBounds(pinLabel, new Rectangle(.5, .6, .5, .1));
            AbsoluteLayout.SetLayoutBounds(pinEntry, new Rectangle(.5, .65, .5, .08));
            AbsoluteLayout.SetLayoutBounds(loginButton, new Rectangle(.5, .9, .25, .12));

            AbsoluteLayout.SetLayoutFlags(infoLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(idLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(idEntry, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(pinLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(pinEntry, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(loginButton, AbsoluteLayoutFlags.All);

            simpleLayout.Children.Add(infoLabel);
            simpleLayout.Children.Add(idLabel);
            simpleLayout.Children.Add(idEntry);
            simpleLayout.Children.Add(pinLabel);
            simpleLayout.Children.Add(pinEntry);
            simpleLayout.Children.Add(loginButton);
            Content = simpleLayout;
        }

        public async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("OnLoginButtonClicked");
            if(idEntry.Text != null && pinEntry.Text != null)
            {
                Console.WriteLine("OnLoginButtonClicked:idEntry=" + idEntry.Text + ", pinEntry=" + pinEntry.Text);
                String id = idEntry.Text;
                String pin = pinEntry.Text;

                CrossFirebasePushNotification.Current.Subscribe("User_" + id);
                String databasePath = "/login/" + id;
                infoLabel.Text = "Logging in, please wait...";
                await FirebaseMessagingClient.SendMessage(databasePath, pin);
            }
            else
            {
                await DisplayAlert("Login", "Enter ID and PIN!", "Ok");
            }
        }

        public void Login(IDictionary<string, object> loginInfo)
        {
            String succes = loginInfo["succes"].ToString();
            if (succes == "true")
            {
                DependencyService.Get<INotification>().Notify("Login status", "Login succes!");
                //FirebaseMessagingClient.SignInWithCustomTokenAsync(loginInfo["token"].ToString());
                String obj = loginInfo["userInfo"].ToString();
                IDictionary<string, object> userInfo = JsonConvert.DeserializeObject<IDictionary<string, object>>(obj);
                Users.InitUserData(userInfo);
                WelcomePage.mainPage.UserLoggedSuccessfully();
            }
            else
            {
                DependencyService.Get<INotification>().Notify("Login status", "Login failed! Wrong PIN number!");
            }
        }
    }
}
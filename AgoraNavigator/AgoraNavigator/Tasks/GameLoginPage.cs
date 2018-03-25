using AgoraNavigator.Login;
using Newtonsoft.Json;
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

            infoLabel = new Label
            {
                Text = " Login to start \nGame of Tasks",
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = AgoraColor.Blue,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Label idLabel = new Label
            {
                Text = "Enter your ID:",
                FontFamily = AgoraFonts.GetPoppinsMedium(),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            idEntry = new Entry
            {
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Keyboard = Keyboard.Numeric
            };
            idEntry.TextChanged += OnEntryTextChanged;

            Image pinEntrySep = new Image
            {
                Source = "entry_separator.png",
                VerticalOptions = LayoutOptions.End
            };

            Label pinLabel = new Label
            {
                Text = "Enter your PIN:",
                FontFamily = AgoraFonts.GetPoppinsMedium(),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            
            pinEntry = new Entry
            {
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Keyboard = Keyboard.Numeric
            };
            pinEntry.TextChanged += OnEntryTextChanged;

            Image idEntrySep = new Image
            {
                Source = "entry_separator.png",
                VerticalOptions = LayoutOptions.End
            };

            idEntry.Placeholder = "XXXX";
            pinEntry.Placeholder = "XXXX";

            Button loginButton = new Button
            {
                Text = "LOGIN",
                BackgroundColor = AgoraColor.Blue,
                TextColor = AgoraColor.DarkBlue
            };
            loginButton.Clicked += OnLoginButtonClicked;

            AbsoluteLayout layout = new AbsoluteLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            AbsoluteLayout.SetLayoutBounds(infoLabel,  new Rectangle(.5, .45, .50, .20));
            AbsoluteLayout.SetLayoutBounds(idLabel,    new Rectangle(.5, .55, .50, .10));
            AbsoluteLayout.SetLayoutBounds(idEntry,    new Rectangle(.5, .60, .50, .08));
            AbsoluteLayout.SetLayoutBounds(idEntrySep, new Rectangle(.5,.585, .50, .08));
            AbsoluteLayout.SetLayoutBounds(pinLabel,   new Rectangle(.5, .75, .50, .10));
            AbsoluteLayout.SetLayoutBounds(pinEntry,   new Rectangle(.5, .80, .50, .08));
            AbsoluteLayout.SetLayoutBounds(pinEntrySep,new Rectangle(.5,.785, .50, .08));
            AbsoluteLayout.SetLayoutBounds(loginButton,new Rectangle(.5, .95, .35, .12));

            AbsoluteLayout.SetLayoutFlags(infoLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(idLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(idEntry, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(idEntrySep, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(pinLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(pinEntry, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(pinEntrySep, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(loginButton, AbsoluteLayoutFlags.All);

            layout.Children.Add(infoLabel);
            layout.Children.Add(idLabel);
            layout.Children.Add(idEntry);
            layout.Children.Add(idEntrySep);
            layout.Children.Add(pinLabel);
            layout.Children.Add(pinEntry);
            layout.Children.Add(pinEntrySep);
            layout.Children.Add(loginButton);
            Content = layout;
            BackgroundImage = "Login.png";
            Title = "Game of Tasks - Login";
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;

            if (entry.Text.Length > 4)
            {
                string entryText = entry.Text;
                entry.TextChanged -= OnEntryTextChanged;
                entry.Text = e.OldTextValue;
                entry.TextChanged += OnEntryTextChanged;
            }
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
                String databasePath = "/login/" + id + "/" + pin;
                if(FirebaseMessagingClient.SendMessage(databasePath, JsonConvert.SerializeObject(FirebaseMessagingClient.firebaseToken)))
                {
                    infoLabel.Text = "Logging in, please wait...";
                }
                else
                {
                    DependencyService.Get<INotification>().Notify("No internet connection", "You need internet connection to login in to the game!");
                }
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
                String obj = loginInfo["userInfo"].ToString();
                IDictionary<string, object> userInfo = JsonConvert.DeserializeObject<IDictionary<string, object>>(obj);
                Users.InitUserData(userInfo);
                Device.BeginInvokeOnMainThread(() =>
                {
                    App.mainPage.DisplayAlert("Login state", "Login succes!", "Ok");
                    App.mainPage.UserLoggedSuccessfully();
                });
            }
        }
    }
}
using AgoraNavigator.Login;
using AgoraNavigator.Popup;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;

namespace AgoraNavigator.Tasks
{
    public class GameLoginNavPage : NavigationPage
    {
        public static GameLoginPage gameLoginPage;
        public GameLoginNavPage()
        {
            BarTextColor = AgoraColor.Blue;
            gameLoginPage = new GameLoginPage();
            Navigation.PushAsync(gameLoginPage);
        }
    }

    public class GameLoginPage : ContentPage
    {
        Entry idEntry;
        Entry pinEntry;
        Label infoLabel;
        bool isLoginStarted = false;


        public GameLoginPage()
        {
            infoLabel = new Label
            {
                Text = " Login to start \nGame of Tasks",
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = AgoraColor.Blue,
                HorizontalTextAlignment = TextAlignment.Center,
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
                Keyboard = Keyboard.Numeric,
                HorizontalTextAlignment = TextAlignment.Center,
                PlaceholderColor = Color.LightGray,
                Placeholder = "ID"
            };
            idEntry.TextChanged += OnEntryTextChanged;

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
                Keyboard = Keyboard.Numeric,
                HorizontalTextAlignment = TextAlignment.Center,
                PlaceholderColor = Color.LightGray,
                Placeholder = "PIN",
                IsPassword = true
            };
            pinEntry.TextChanged += OnEntryTextChanged;

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

            AbsoluteLayout.SetLayoutBounds(infoLabel,  new Rectangle(.5, .45, .50, .2));
            AbsoluteLayout.SetLayoutBounds(idLabel,    new Rectangle(.5, .55, .50, .1));
            AbsoluteLayout.SetLayoutBounds(idEntry,    new Rectangle(.5, .60, .50, .08));
            AbsoluteLayout.SetLayoutBounds(pinLabel,   new Rectangle(.5, .70, .50, .1));
            AbsoluteLayout.SetLayoutBounds(pinEntry,   new Rectangle(.5, .75, .50, .08));
            AbsoluteLayout.SetLayoutBounds(loginButton,new Rectangle(.5, .95, .45, .15));

            AbsoluteLayout.SetLayoutFlags(infoLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(idLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(idEntry, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(pinLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(pinEntry, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(loginButton, AbsoluteLayoutFlags.All);

            layout.Children.Add(infoLabel);
            layout.Children.Add(idLabel);
            layout.Children.Add(idEntry);
            layout.Children.Add(pinLabel);
            layout.Children.Add(pinEntry);
            layout.Children.Add(loginButton);
            Content = layout;
            BackgroundImage = "Login_Background.png";
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
            if (!isLoginStarted)
            {
                isLoginStarted = true;
                if (idEntry.Text != null && pinEntry.Text != null)
                {
                    Console.WriteLine("OnLoginButtonClicked:idEntry=" + idEntry.Text + ", pinEntry=" + pinEntry.Text);
                    String id = idEntry.Text;
                    String pin = pinEntry.Text;
                    try
                    {
                        String databasePath = "/users/" + id + "/" + pin;
                        JObject userInfo = await FirebaseMessagingClient.SendSingleQuery<JObject>(databasePath);
                        Users.InitUserData(Convert.ToInt32(id), Convert.ToInt32(pin), userInfo);
                        SimplePopup popup = new SimplePopup("Login succes!", "Let's start Game of Tasks!")
                        {
                            ColorBackground = Color.Green,
                            ColorBody = Color.White,
                            ColorTitle = Color.White,
                        };
                        popup.SetColors();
                        await Navigation.PushPopupAsync(popup);
                        App.mainPage.UserLoggedSuccessfully();
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine("OnLoginButtonClicked:err=" + err.ToString());
                        SimplePopup popup = new SimplePopup("Login failed!", "Check your internet connection, ID and PIN number and try again")
                        {
                            ColorBackground = Color.Red,
                            ColorBody = Color.White,
                            ColorTitle = Color.White,
                        };
                        popup.SetColors();
                        await Navigation.PushPopupAsync(popup);
                        infoLabel.Text = "Login failed.";
                        isLoginStarted = false;
                    }
                }
                else
                {
                    SimplePopup popup = new SimplePopup("Login failed!", "Wrong ID or PIN number!")
                    {
                        ColorBackground = Color.Red,
                        ColorBody = Color.White,
                        ColorTitle = Color.White,
                    };
                    popup.SetColors();
                    await Navigation.PushPopupAsync(popup);
                    isLoginStarted = false;
                }
            }
        }      
    }
}
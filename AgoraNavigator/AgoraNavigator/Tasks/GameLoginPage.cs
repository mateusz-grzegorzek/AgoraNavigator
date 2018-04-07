using AgoraNavigator.Login;
using AgoraNavigator.Popup;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace AgoraNavigator.Tasks
{
    public class GameLoginNavPage : NavigationPage
    {
        public static GameLoginPage gameLoginPage;
        public GameLoginNavPage(Type navigateTo)
        {
            BarTextColor = AgoraColor.Blue;
            gameLoginPage = new GameLoginPage(navigateTo);
            Navigation.PushAsync(gameLoginPage);
        }
    }

    public class GameLoginPage : ContentPage
    {
        Entry idEntry;
        Entry pinEntry;
        Label infoLabel;
        bool isLoginStarted = false;
        Type _navigateToPage;


        public GameLoginPage(Type navigateTo)
        {
            _navigateToPage = navigateTo;
            infoLabel = new Label
            {
                Text = "Login to use\nall cool features!",
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                TextColor = AgoraColor.Blue,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            Label idLabel = new Label
            {
                Text = "Participant ID",
                FontFamily = AgoraFonts.GetPoppinsRegular(),
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            idEntry = new Entry
            {
                TextColor = Color.White,
                Keyboard = Keyboard.Numeric,
                HorizontalTextAlignment = TextAlignment.Center,
                PlaceholderColor = Color.LightGray,
                Placeholder = "000-0000",
            };
            idEntry.TextChanged += OnIDTextChanged;

            Label pinLabel = new Label
            {
                Text = "PIN Number",
                FontFamily = AgoraFonts.GetPoppinsRegular(),
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
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
            pinEntry.TextChanged += OnPinTextChanged;

            Button loginButton = new Button
            {
                Text = "LOGIN",
                BackgroundColor = AgoraColor.Blue,
                TextColor = AgoraColor.DarkBlue,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            loginButton.Clicked += OnLoginButtonClicked;


            Button scanButton = new Button
            {
                Text = "SCAN CODE",
                BackgroundColor = AgoraColor.Blue,
                TextColor = AgoraColor.DarkBlue,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            scanButton.Clicked += async (sender, e) => {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan(new ZXing.Mobile.MobileBarcodeScanningOptions()
                {
                    PossibleFormats = new List<ZXing.BarcodeFormat> { ZXing.BarcodeFormat.AZTEC }
                });

                if (result != null)
                {
                    await LoginUsingCodeAsync(result.Text);
                }
            };

            StackLayout layout = new StackLayout()
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                Margin = new Thickness(20, 5)
            };

            StackLayout buttonsLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Spacing = 10
            };

            buttonsLayout.Children.Add(loginButton);
            buttonsLayout.Children.Add(scanButton);

            layout.Children.Add(infoLabel);
            layout.Children.Add(idLabel);
            layout.Children.Add(idEntry);
            layout.Children.Add(pinLabel);
            layout.Children.Add(pinEntry);
            layout.Children.Add(buttonsLayout);

            Content = layout;
            BackgroundImage = "Login_Background.png";
            Title = "Login";
        }

        private async System.Threading.Tasks.Task LoginUsingCodeAsync(string code)
        {
            if(!ValidateCode(code))
            {
                SimplePopup popup = new SimplePopup("Scanning failed!", "Please scan the square-shaped code which you received.")
                {
                    ColorBackground = Color.Red,
                    ColorBody = Color.White,
                    ColorTitle = Color.White,
                };
                popup.SetColors();
                await Navigation.PushPopupAsync(popup);
                return;
            }
            string[] codeParts = code.Split('-');

            string participantId = codeParts[0] + codeParts[1];
            string participantPin = codeParts[2];

            await HandleLoginAsync(participantId, participantPin);
        }

        private bool ValidateCode(string code)
        {
            return new Regex(@"\d{3}-\d{4}-\d{4}")
                .Match(code)
                .Success;
        }

        private void OnPinTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;

            if (entry.Text.Length > 4)
            {
                string entryText = entry.Text;
                entry.TextChanged -= OnPinTextChanged;
                entry.Text = e.OldTextValue;
                entry.TextChanged += OnPinTextChanged;
            }
        }

        private void OnIDTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;

            if (entry.Text.Length > 7)
            {
                string entryText = entry.Text;
                entry.TextChanged -= OnIDTextChanged;
                entry.Text = e.OldTextValue;
                entry.TextChanged += OnIDTextChanged;
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
                    await HandleLoginAsync(id, pin);
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

        private async System.Threading.Tasks.Task HandleLoginAsync(string id, string pin)
        {
            try
            {
                String databasePath = "/users/" + id + "/" + pin;
                JObject userInfo = await FirebaseMessagingClient.SendSingleQuery<JObject>(databasePath);
                Users.InitUserData(Convert.ToInt32(id), Convert.ToInt32(pin), userInfo);
                SimplePopup popup = new SimplePopup("Login successful!", "Start the Game of Tasks or use your virtual badge!")
                {
                    ColorBackground = Color.Green,
                    ColorBody = Color.White,
                    ColorTitle = Color.White,
                };
                popup.SetColors();
                await Navigation.PushPopupAsync(popup);
                App.mainPage.UserLoggedSuccessfully(_navigateToPage);
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
                infoLabel.Text = "Login failed :(";
                isLoginStarted = false;
            }
        }
    }
}
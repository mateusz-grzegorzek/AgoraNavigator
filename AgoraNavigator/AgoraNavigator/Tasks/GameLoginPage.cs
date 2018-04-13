using AgoraNavigator.Login;
using AgoraNavigator.Popup;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using System.Threading.Tasks;

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
                BackgroundColor = Color.Transparent,
                Keyboard = Keyboard.Numeric,
                HorizontalTextAlignment = TextAlignment.Start,
                PlaceholderColor = Color.LightGray,
                Text = "220-0000",
                Placeholder = "XXX-XXXX",
                HorizontalOptions = LayoutOptions.Center
            };
            idEntry.TextChanged += OnIdTextChanged;

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
                BackgroundColor = Color.Transparent,
                Text = "0000",
                Keyboard = Keyboard.Numeric,
                HorizontalTextAlignment = TextAlignment.Center,
                PlaceholderColor = Color.LightGray,
                Placeholder = "XXXX",
                IsPassword = true,
                HorizontalOptions = LayoutOptions.Center
            };
            pinEntry.TextChanged += OnPinTextChanged;

            Button loginButton = new Button
            {
                Text = "LOGIN",
                BackgroundColor = AgoraColor.Blue,
                TextColor = AgoraColor.DarkBlue,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 100,
            };
            loginButton.Clicked += OnLoginButtonClicked;


            Button scanButton = new Button
            {
                Text = "SCAN CODE",
                BackgroundColor = AgoraColor.Blue,
                TextColor = AgoraColor.DarkBlue,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 100,
                WidthRequest = 150
            };
            scanButton.Clicked += async (sender, e) =>
            {
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
                Margin = new Thickness(10, 10)
            };

            Grid loginOptionsGrid = new Grid();

            loginOptionsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
            loginOptionsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) });

            StackLayout loginUsingIdAndPinLayout = new StackLayout();

            loginUsingIdAndPinLayout.Children.Add(idLabel);
            loginUsingIdAndPinLayout.Children.Add(idEntry);
            loginUsingIdAndPinLayout.Children.Add(pinLabel);
            loginUsingIdAndPinLayout.Children.Add(pinEntry);
            loginUsingIdAndPinLayout.Children.Add(loginButton);

            loginOptionsGrid.Children.Add(loginUsingIdAndPinLayout, 0, 0);
            loginOptionsGrid.Children.Add(scanButton, 1, 0);

            layout.Children.Add(infoLabel);
            layout.Children.Add(loginOptionsGrid);

            Title = "Login";
            BackgroundColor = AgoraColor.DarkBlue;
            Image backgroundImage = new Image()
            {
                Source = "Login_Background.png"
            };
            Content = new AbsoluteLayout
            {
                Children =
                {
                    {backgroundImage, new Rectangle (0, 0, 1, 1), AbsoluteLayoutFlags.All},
                    {layout, new Rectangle (0, 0, 1, 1), AbsoluteLayoutFlags.All}
                }
            };
        }

        private async Task LoginUsingCodeAsync(string code)
        {
            if (!ValidateCode(code))
            {
                await Task.Delay(1000); /* adding delay, so app can return from scanning mode */
                DependencyService.Get<IPopup>().ShowPopup("Scanning failed!", "Please scan the square-shaped code which you received.", false);
                App.mainPage.ShowLoginScreen(typeof(TasksPage));
            }
            else
            {
                string[] codeParts = code.Split('-');

                string participantId = codeParts[0];
                string participantPin = codeParts[1];

                await HandleLoginAsync(participantId, participantPin);
            }
        }

        private bool ValidateCode(string code)
        {
            return new Regex(@"\d{4}-\d{4}")
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

        private void OnIdTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;

            if (entry.Text.Length < 4)
            {
                entry.TextChanged -= OnIdTextChanged;
                entry.Text = e.OldTextValue;
                entry.TextChanged += OnIdTextChanged;
            }
            if (entry.Text.Length > 8)
            {
                entry.TextChanged -= OnIdTextChanged;
                entry.Text = e.OldTextValue;
                entry.TextChanged += OnIdTextChanged;
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
                    String id = idEntry.Text.Substring(4, 4);
                    String pin = pinEntry.Text;
                    await HandleLoginAsync(id, pin);
                }
                else
                {
                    DependencyService.Get<IPopup>().ShowPopup("Login failed!", "Wrong ID or PIN number!", false);
                    isLoginStarted = false;
                }
            }
        }

        private async Task HandleLoginAsync(string id, string pin)
        {
            try
            {
                String databasePath = "/users/" + id + "/" + pin;
                JObject userInfo = await FirebaseMessagingClient.SendSingleQuery<JObject>(databasePath);
                Users.InitUserData(Convert.ToInt32(id), Convert.ToInt32(pin), userInfo);
                DependencyService.Get<IPopup>().ShowPopup("Login successful!", "Start the Game of Tasks or use your virtual badge!", true);
                App.mainPage.NavigateTo(_navigateToPage);
            }
            catch (Exception err)
            {
                Console.WriteLine("OnLoginButtonClicked:err=" + err.ToString());
                DependencyService.Get<IPopup>().ShowPopup("Login failed!", "Check your internet connection, ID and PIN number and try again", false);
                infoLabel.Text = "Login failed :(";
                isLoginStarted = false;
            }
        }
    }
}
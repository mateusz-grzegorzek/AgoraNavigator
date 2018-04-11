using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AgoraNavigator.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimplePopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public SimplePopup(string title, string body, bool succes)
        {
            InitializeComponent();

            Label labelTitle = new Label
            {
                Text = title,
                TextColor = Color.White,
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            Label labelBody = new Label
            {
                Text = body,
                TextColor = Color.White,
                FontFamily = AgoraFonts.GetPoppinsMedium(),
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
            };

            Button buttonOk = new Button
            {
                Text = "OK",
                TextColor = Color.Black,
                BorderColor = Color.Black,
                BackgroundColor = Color.White,
                FontFamily = AgoraFonts.GetPoppinsMedium(),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 120
            };
            buttonOk.Clicked += buttonOk_Clicked;

            Grid grid = new Grid
            {
                Margin = new Thickness(10, 10),
                RowSpacing = 5
            };

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });

            grid.Children.Add(labelTitle, 0, 0);
            grid.Children.Add(labelBody, 0, 1);
            grid.Children.Add(buttonOk, 0, 2);

            if(succes)
            {
                frame.BackgroundColor = Color.Green;
            }
            else
            {
                frame.BackgroundColor = Color.Red;
            }

            frame.Content = grid;
        }

        private async void buttonOk_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.RemovePageAsync(this);
        }
    }
}
using AgoraNavigator.Login;
using System;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace AgoraNavigator.Badge
{
    public class BadgePage : NavigationPage
    {
        public static BadgeMasterPage barCodeMasterPage;

        public BadgePage()
        {
            if (!Users.isUserLogged)
            {
                App.mainPage.ShowLoginScreen(typeof(BadgePage));
                return;
            }
            barCodeMasterPage = new BadgeMasterPage();
            Navigation.PushAsync(barCodeMasterPage);
        }
    }

    public class BadgeMasterPage : ContentPage
    {
        public BadgeMasterPage()
        {
            Title = "Badge";
            if (!Users.isUserLogged)
            {
                return;
            }
            Appearing += OnAppearing;
        }

        public void OnAppearing(object sender, EventArgs e)
        {
            ZXingBarcodeImageView barcode = new ZXingBarcodeImageView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            barcode.BarcodeFormat = ZXing.BarcodeFormat.CODE_39;
            barcode.BarcodeOptions.Width = 1000;
            barcode.BarcodeOptions.Height = 500;
            barcode.BarcodeOptions.Margin = 10;

            string barcodeValue = BuildBarcodeValue();
            barcode.BarcodeValue = barcodeValue;

            Label idLabel = new Label();
            idLabel.HorizontalTextAlignment = TextAlignment.Center;
            idLabel.Text = barcodeValue;
            idLabel.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));

            Frame frame = new Frame();
            frame.Content = barcode;

            StackLayout stack = new StackLayout();
            stack.Children.Add(idLabel);

            StackLayout layout = new StackLayout()
            {
                VerticalOptions = LayoutOptions.Center,
            };
            layout.Children.Add(frame);
            layout.Children.Add(stack);

            Content = layout;
        }

        private string BuildBarcodeValue()
        {
            return "220-" + Users.loggedUser.Id.ToString();
        }
    }
}

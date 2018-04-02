using AgoraNavigator.Login;
using AgoraNavigator.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace AgoraNavigator.Badge
{
    public class BadgePage : NavigationPage
    {
        public static BadgeMasterPage barCodeMasterPage;

        public BadgePage()
        {
            if(!Users.isUserLogged)
            {
                Navigation.PushAsync(new GameLoginNavPage());
                return;
            }

            barCodeMasterPage = new BadgeMasterPage();
            Navigation.PushAsync(barCodeMasterPage);
        }
    }

    public class BadgeMasterPage : ContentPage
    {
        ZXingBarcodeImageView _barcode;
        Label _idLabel;

        public BadgeMasterPage()
        {
            Title = "Badge";
            if(!Users.isUserLogged)
            {
                return;
            }

            _barcode = new ZXingBarcodeImageView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
            _barcode.BarcodeFormat = ZXing.BarcodeFormat.CODE_39;
            _barcode.BarcodeOptions.Width = 300;
            _barcode.BarcodeOptions.Height = 150;
            _barcode.BarcodeOptions.Margin = 10;

            string barcodeValue = BuildBarcodeValue();
            _barcode.BarcodeValue = barcodeValue;

            _idLabel = new Label();
            _idLabel.HorizontalTextAlignment = TextAlignment.Center;
            _idLabel.Text = barcodeValue;

            Content = new StackLayout
            {
                Children = {
               _barcode,
               _idLabel
            }
            };
        }

        private string BuildBarcodeValue()
        {
            string id = Users.loggedUser.Id
                 .ToString()
                 .PadLeft(7, '0');

            string value = id.Substring(0, 3) + "-" + id.Substring(3, 4);

            return value;
        }
    }
}

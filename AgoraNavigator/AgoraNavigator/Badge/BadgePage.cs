using AgoraNavigator.Login;
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
            barCodeMasterPage = new BadgeMasterPage();
            Navigation.PushAsync(barCodeMasterPage);
        }
    }

    public class BadgeMasterPage : ContentPage
    {
        ZXingBarcodeImageView _barcode;
        public BadgeMasterPage()
        {
            Title = "Badge";
            _barcode = new ZXingBarcodeImageView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
            _barcode.BarcodeFormat = ZXing.BarcodeFormat.CODE_128;
            _barcode.BarcodeOptions.Width = 300;
            _barcode.BarcodeOptions.Height = 150;
            _barcode.BarcodeOptions.Margin = 10;
            _barcode.BarcodeValue = Users.loggedUser == null ? "0" : Users.loggedUser.Id.ToString();
            Content = new StackLayout
            {
                Children = {
               _barcode
            }
            };
        }
    }
}

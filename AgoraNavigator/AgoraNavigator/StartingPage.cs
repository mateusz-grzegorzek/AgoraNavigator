using Plugin.DeviceInfo;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AgoraNavigator
{
    class StartingPage : ContentPage
    {
        Button permissionButton;

        /*
        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }
        */

        public StartingPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            int screenHeight = CrossDevice.Hardware.ScreenHeight;
            int screenWidth = CrossDevice.Hardware.ScreenWidth;

            Image backgroundImage = new Image();
            backgroundImage.Source = "StartingPage.png";

            permissionButton = new Button();
            permissionButton.Text = "Welcome in Agora Navigator!";
            permissionButton.Clicked += OnButtonClicked;
            permissionButton.Rotation = 90;
            permissionButton.BackgroundColor = Color.Transparent;
            permissionButton.TextColor = Color.Turquoise;

            AbsoluteLayout simpleLayout = new AbsoluteLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            AbsoluteLayout.SetLayoutFlags(backgroundImage, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(backgroundImage, new Rectangle(0f, 0f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(permissionButton, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(permissionButton, new Rectangle(0.5f, 0.5f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            simpleLayout.Children.Add(backgroundImage);
            simpleLayout.Children.Add(permissionButton);
            Content = simpleLayout;
        }

        async void OnButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("StartingPage::OnButtonClicked");
            try
            {
                bool permissionsGranted = false;
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    Console.WriteLine("Not granted yet!");
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Need location", "OK", "Cancel");
                    }
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = results[Permission.Location];
                    if (status == PermissionStatus.Granted)
                    {
                        Console.WriteLine("Access granted! :)");
                        permissionsGranted = true;
                    }
                    else
                    {
                        Console.WriteLine("Not granted! :(");
                    }
                }
                else
                {
                    Console.WriteLine("Already granted!");
                    permissionsGranted = true;
                }
                if(permissionsGranted)
                {
                    await Navigation.PushAsync(new MainPage());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

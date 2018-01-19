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
        MainPage mainPage;
        Button permissionButton;

        public StartingPage(MainPage mainPage)
        {
            Console.WriteLine("StartingPage::Hello World!");
            this.mainPage = mainPage;

            permissionButton = new Button();
            permissionButton.Text = "Click me to enter application (and access location permissions!)";
            permissionButton.Clicked += OnButtonClicked;

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(permissionButton);
            Content = stack;
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
                    mainPage.StartNavigator();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

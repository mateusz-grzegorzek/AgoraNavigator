using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AgoraNavigator.GoogleMap
{
    public class MapPage : NavigationPage
    {
        public static GoogleMapPage googleMapPage;

        public MapPage()
        {
            Console.WriteLine("MapPage");
            BarTextColor = Color.Red;
            BackgroundColor = Color.Azure;
            googleMapPage = new GoogleMapPage();
            Navigation.PushAsync(googleMapPage);
        }
    }

    public class GoogleMapPage : ContentPage
    {
        public static Map map;
        public bool locationEnabled = false;

        public GoogleMapPage()
        {
            Title = "Map";
            map = new Map()
            {
                IsShowingUser = false,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(50.0655097, 19.9099141), Distance.FromMiles(0.05)));
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            Content = stack;
        }
    }
}

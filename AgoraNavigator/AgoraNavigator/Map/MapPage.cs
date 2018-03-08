using System;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

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
                MyLocationEnabled = true,
                HeightRequest = 100,
                WidthRequest = 960,
                IsIndoorEnabled = true,
                MapType = MapType.Hybrid,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            map.UiSettings.RotateGesturesEnabled = true;
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(50.0655097, 19.9099141), Distance.FromMiles(0.05)));
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            Content = stack;
        }
    }
}

using System;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace AgoraNavigator.GoogleMap
{
    public class MapPage : NavigationPage
    {
        public static GoogleMapPage googleMapPage;

        public MapPage(double latitude, double longitude)
        {
            Console.WriteLine("MapPage");
            BarTextColor = AgoraColor.Blue;
            googleMapPage = new GoogleMapPage(latitude, longitude);
            Navigation.PushAsync(googleMapPage);
        }
    }

    public class GoogleMapPage : ContentPage
    {
        public static Map map;
        public bool locationEnabled = false;
        Button buttonStreet;
        Button buttonHybrid;
        Button buttonSatellite;

        public GoogleMapPage(double latitude, double longitude)
        {
            Title = "Map";
            map = new Map()
            {
                IsIndoorEnabled = true,
                MapType = MapType.Hybrid,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(latitude, longitude), Distance.FromMiles(0.05)));
            map.UiSettings.MyLocationButtonEnabled = true;
            map.UiSettings.RotateGesturesEnabled = true;
            map.UiSettings.CompassEnabled = true;
            map.UiSettings.ZoomControlsEnabled = false;


            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            buttonStreet = new Button
            {
                Text = "Street",
                BackgroundColor = AgoraColor.Blue,
                TextColor = Color.White
            };
            buttonStreet.Clicked += ButtonStreet_Clicked;
            buttonSatellite = new Button
            {
                Text = "Satellite",
                BackgroundColor = AgoraColor.Blue,
                TextColor = Color.White
            };
            buttonSatellite.Clicked += ButtonSatellite_Clicked;
            buttonHybrid = new Button
            {
                Text = "Hybrid",
                BackgroundColor = AgoraColor.DarkBlue,
                TextColor = Color.White
            };
            buttonHybrid.Clicked += ButtonHybrid_Clicked;


            Grid.SetColumnSpan(map, 3);
            Grid.SetRowSpan(map, 2);
            grid.Children.Add(map, 0, 3, 0, 2);
            grid.Children.Add(buttonStreet, 0, 1);
            grid.Children.Add(buttonSatellite, 1, 1);
            grid.Children.Add(buttonHybrid, 2, 1);

            Content = grid; 
        }

        private void ButtonStreet_Clicked(object sender, EventArgs e)
        {
            map.MapType = MapType.Street;
            buttonStreet.BackgroundColor = AgoraColor.DarkBlue;
            buttonSatellite.BackgroundColor = AgoraColor.Blue;
            buttonHybrid.BackgroundColor = AgoraColor.Blue;
        }

        private void ButtonSatellite_Clicked(object sender, EventArgs e)
        {
            map.MapType = MapType.Satellite;
            buttonStreet.BackgroundColor = AgoraColor.Blue;
            buttonSatellite.BackgroundColor = AgoraColor.DarkBlue;
            buttonHybrid.BackgroundColor = AgoraColor.Blue;
        }

        private void ButtonHybrid_Clicked(object sender, EventArgs e)
        {
            map.MapType = MapType.Hybrid;
            buttonStreet.BackgroundColor = AgoraColor.Blue;
            buttonSatellite.BackgroundColor = AgoraColor.Blue;
            buttonHybrid.BackgroundColor = AgoraColor.DarkBlue;
        }
    }
}

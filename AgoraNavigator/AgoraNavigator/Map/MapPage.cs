using Plugin.Settings;
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
        static MapButton buttonStreet;
        static MapButton buttonHybrid;
        static MapButton buttonSatellite;
        static MapType activeMapType;

        public class MapButton : Button
        {
            MapType mapType;

            public MapButton(MapType _mapType)
            {
                mapType = _mapType;
                if (mapType == activeMapType)
                {
                    BackgroundColor = AgoraColor.DarkBlue;
                }
                else
                {
                    BackgroundColor = AgoraColor.Blue;
                }
                TextColor = Color.White;
                Clicked += MapButton_Clicked;
            }

            private void MapButton_Clicked(object sender, EventArgs e)
            {
                MapButton mapButton = (MapButton)sender;
                activeMapType = mapButton.mapType;
                map.MapType = activeMapType;
                switch (activeMapType)
                {
                    case MapType.Street:
                        buttonStreet.BackgroundColor = AgoraColor.DarkBlue;
                        buttonSatellite.BackgroundColor = AgoraColor.Blue;
                        buttonHybrid.BackgroundColor = AgoraColor.Blue;
                        break;
                    case MapType.Satellite:
                        buttonStreet.BackgroundColor = AgoraColor.Blue;
                        buttonSatellite.BackgroundColor = AgoraColor.DarkBlue;
                        buttonHybrid.BackgroundColor = AgoraColor.Blue;
                        break;
                    case MapType.Hybrid:
                        buttonStreet.BackgroundColor = AgoraColor.Blue;
                        buttonSatellite.BackgroundColor = AgoraColor.Blue;
                        buttonHybrid.BackgroundColor = AgoraColor.DarkBlue;
                        break;
                }
                CrossSettings.Current.AddOrUpdateValue("activeMapType", (int)activeMapType);
            }
        }

        public GoogleMapPage(double latitude, double longitude)
        {
            Title = "Map";
            activeMapType = (MapType)CrossSettings.Current.GetValueOrDefault("activeMapType", (int)MapType.Hybrid);
            map = new Map()
            {
                IsIndoorEnabled = true,
                MapType = activeMapType,
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

            buttonStreet = new MapButton(MapType.Street)
            {
                Text = "Street",
            };
            buttonSatellite = new MapButton(MapType.Satellite)
            {
                Text = "Satellite",
            };
            buttonHybrid = new MapButton(MapType.Hybrid)
            {
                Text = "Hybrid",
            };

            Grid.SetColumnSpan(map, 3);
            grid.Children.Add(map, 0, 3, 0, 1);
            grid.Children.Add(buttonStreet, 0, 1);
            grid.Children.Add(buttonSatellite, 1, 1);
            grid.Children.Add(buttonHybrid, 2, 1);

            Content = grid; 
        }
    }
}

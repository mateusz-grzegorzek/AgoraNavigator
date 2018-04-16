using Plugin.Settings;
using System;
using System.Collections.Generic;
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

        List<CustomPinWrapper> displayedPins = null;
        CustomPinWrapper selectedPinWrapper = null;

        List<CustomPinWrapper> pins_agoraSpots = new List<CustomPinWrapper>()
        {
            new CustomPinWrapper("Gym", "ul. Reymonta 22", 50.0656911, 19.9083581, "\uf236"),
            new CustomPinWrapper("Plenaries", "ul. Krupnicza 33", 50.0626844,19.9227462, "\uf086"),
            new CustomPinWrapper("Department of Law and Administration UJ", "ul. Krupnicza 33A", 50.0610109, 19.9328122, "\uf0e3"),
            new CustomPinWrapper("Canteen Nawojka - dinner from the second day", "ul. Reymonta 11", 50.0649061, 19.9183185, "\uf2e7")
        };

        List<CustomPinWrapper> pins_discover = new List<CustomPinWrapper>()
        {
            new CustomPinWrapper("Wawel", "Address", 50.0540529, 19.9332236, "\uf06e"),
            new CustomPinWrapper("Main Market Square", "Rynek Główny", 50.0619005, 19.9345672, "\uf276")
        };

        public GoogleMapPage(double latitude, double longitude)
        {
            activeMapType = (MapType)CrossSettings.Current.GetValueOrDefault("activeMapType", (int)MapType.Street);
            Title = "Map (Agora Spots)";

            ToolbarItem item1 = new ToolbarItem("Agora Spots", "", () => {
                Title = "Map (Agora Spots)";
                this.showPins(pins_agoraSpots);
            });
            item1.Order = ToolbarItemOrder.Secondary;

            ToolbarItem item2 = new ToolbarItem("Discover Kraków", "", () => {
                Title = "Map (Discover Kraków)";
                this.showPins(pins_discover);
            });
            item2.Order = ToolbarItemOrder.Secondary;

            ToolbarItem item3 = new ToolbarItem("Hide pins", "", () => {
                Title = "Map";
                this.showPins(null);
            });
            item3.Order = ToolbarItemOrder.Secondary;

            ToolbarItems.Add(item1);
            ToolbarItems.Add(item2);
            ToolbarItems.Add(item3);

            map = new Map()
            {
                IsIndoorEnabled = true,
                MapType = activeMapType,
            };

            map.MapClicked += Map_MapClicked;
            map.PinClicked += Map_PinClicked;
            this.showPins(pins_agoraSpots);

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

        private void Map_PinClicked(object sender, PinClickedEventArgs e)
        {
            Pin pin = e.Pin;

            if (pin != null && displayedPins != null)
            {
                CustomPinWrapper pinWrapper = displayedPins.Find(x => x.pin.Equals(pin));
                if (selectedPinWrapper != null)
                {
                    selectedPinWrapper.resetSelection();
                }

                pinWrapper.setAsSelected();
                selectedPinWrapper = pinWrapper;
            }
        }

        private void Map_MapClicked(object sender, MapClickedEventArgs e)
        {
            if (selectedPinWrapper != null)
            {
                selectedPinWrapper.resetSelection();
                selectedPinWrapper = null;
            }
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

        private void showPins(List<CustomPinWrapper> pinsWrappers)
        {
            map.Pins.Clear();

            if (pinsWrappers == null)
            {
                displayedPins = null;
                return;
            }


            displayedPins = pinsWrappers;
            foreach (var pinWrapper in pinsWrappers)
            {
                map.Pins.Add(pinWrapper.pin);
            }
        }
    }
}

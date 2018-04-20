using AgoraNavigator.Popup;
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
#if __ANDROID__
            Appearing += OnAppearing;
#endif
            BarTextColor = AgoraColor.Blue;
            googleMapPage = new GoogleMapPage(latitude, longitude);
            Navigation.PushAsync(googleMapPage);
        }

#if __ANDROID__
        public void OnAppearing(object sender, EventArgs e)
        {
            bool userInformedAboutUsage = CrossSettings.Current.GetValueOrDefault("userInformedAboutMapUsage", false);
            if (!userInformedAboutUsage)
            {
                DependencyService.Get<IPopup>().ShowPopup("Map usage", "Click on icon in upper right corner to change views!", true);
                CrossSettings.Current.AddOrUpdateValue("userInformedAboutMapUsage", true);
            }
        }
#endif
    }

    public class GoogleMapPage : ContentPage
    {
        public static Map map;
        public bool locationEnabled = false;
        static MapButton buttonStreet;
        static MapButton buttonHybrid;
        static MapButton buttonSatellite;
        static MapType activeMapType;

        public enum PinsType
        {
            AgoraSpots = 0,
            DiscoverKrakow = 1,
            HidePins = 2
        };
        PinsType activePinsType;

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
            new CustomPinWrapper("Gym", "ul. Reymonta 22", 50.0655713, 19.9083845, "\uf236"),
            new CustomPinWrapper("Auditorium Maximum", "ul. Krupnicza 33", 50.0627189, 19.9251657, "\uf086"),
            new CustomPinWrapper("Institute of Psychology UJ", "ul. Ingardena 6", 50.0616172, 19.9198355, "\uf086"),
            new CustomPinWrapper("Department of Law and Administration UJ", "ul. Krupnicza 33A", 50.0630855, 19.9256148, "\uf086"),
            new CustomPinWrapper("Canteen Nawojka - dinner from the second day", "ul. Reymonta 11", 50.0649061, 19.9183185, "\uf2e7"),
            new CustomPinWrapper("Stara Zajezdnia - Tuesday", "ul. Świętego Wawrzyńca 12", 50.0501472, 19.9471204, "\uf001"),
            new CustomPinWrapper("Prozak 2.0 - Wednesday", "plac Dominikański 6", 50.0590028, 19.9379979, "\uf001"),
            new CustomPinWrapper("ZET PE TE - Thursday", "ul. Dolnych Młynów 10", 50.0645571, 19.9255292, "\uf001"),
            new CustomPinWrapper("Forty Kleparz - Friday", "ul. Kamienna 2", 50.0747908, 19.9376157, "\uf001"),
            new CustomPinWrapper("Hala TS Wisła - Saturday", "ul. Reymonta 22", 50.0655961, 19.9097579, "\uf001")
        };

        List<CustomPinWrapper> pins_discover = new List<CustomPinWrapper>()
        {
            new CustomPinWrapper("Wawel", "Wawel 5", 50.0540529, 19.9332236, "\uf06e"),
            new CustomPinWrapper("Main Market Square", "Rynek Główny", 50.0619005, 19.9345672, "\uf06e"),
            new CustomPinWrapper("Oskar Schindler's Enamel Factory", "ul. Lipowa 4", 50.0474374, 19.9617823, "\uf06e"),
            new CustomPinWrapper("Kraków Barbican", "Basztowa", 50.0654663, 19.9416142, "\uf06e"),
            new CustomPinWrapper("Sukiennice", "ul. Rynek Główny 3", 50.0616869, 19.9373206, "\uf06e"),
            new CustomPinWrapper("Cracow National Museum", "Al. 3 Maja 1", 50.0604778, 19.9236189, "\uf06e"),
            new CustomPinWrapper("AGH Student Campus", "ul. Józefa Rostafińskiego 7a", 50.068057, 19.9054193, "\uf0fc")

        };

        public GoogleMapPage(double latitude, double longitude)
        {
            activeMapType = (MapType)CrossSettings.Current.GetValueOrDefault("activeMapType", (int)MapType.Street);
            activePinsType = (PinsType)CrossSettings.Current.GetValueOrDefault("activePinsType", (int)PinsType.AgoraSpots);

            ToolbarItem item1 = new ToolbarItem("Agora Spots", "", () =>
            {
                Title = "Map (Agora Spots)";
                this.showPins(pins_agoraSpots);
                CrossSettings.Current.AddOrUpdateValue("activePinsType", (int)PinsType.AgoraSpots);
            });
            item1.Order = ToolbarItemOrder.Secondary;

            ToolbarItem item2 = new ToolbarItem("Discover Kraków", "", () =>
            {
                Title = "Map (Discover Kraków)";
                this.showPins(pins_discover);
                CrossSettings.Current.AddOrUpdateValue("activePinsType", (int)PinsType.DiscoverKrakow);
            });
            item2.Order = ToolbarItemOrder.Secondary;

            ToolbarItem item3 = new ToolbarItem("Hide pins", "", () =>
            {
                Title = "Map";
                this.showPins(null);
                CrossSettings.Current.AddOrUpdateValue("activePinsType", (int)PinsType.HidePins);
            });
            item3.Order = ToolbarItemOrder.Secondary;

#if __ANDROID__
            ToolbarItems.Add(item1);
            ToolbarItems.Add(item2);
            ToolbarItems.Add(item3);
#endif
            map = new Map()
            {
                IsIndoorEnabled = true,
                MapType = activeMapType,
            };

            map.MapClicked += Map_MapClicked;
            map.PinClicked += Map_PinClicked;
#if __ANDROID__
            switch(activePinsType)
            {
                case PinsType.AgoraSpots:
                    Title = "Map (Agora Spots)";
                    this.showPins(pins_agoraSpots);
                    break;
                case PinsType.DiscoverKrakow:
                    Title = "Map (Discover Kraków)";
                    this.showPins(pins_discover);
                    break;
                case PinsType.HidePins:
                    Title = "Map";
                    this.showPins(null);
                    break;
            }
#endif

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

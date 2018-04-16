using Xamarin.Forms.GoogleMaps;

namespace AgoraNavigator.GoogleMap
{
    class CustomPinWrapper
    {
        public static string BackgroundDefault = "Map_Pin.png";
        public static string BackgroundSelected = "Map_Pin_Selected.png";

        public Pin pin;

        private string name;
        private string address;
        private double latitude;
        private double longtitude;
        private string iconChar;

        private CustomPinView view;

        public CustomPinWrapper(string name, string address, double lat, double lon, string iconChar)
        {
            this.name = name;
            this.address = address;
            this.latitude = lat;
            this.longtitude = lon;
            this.iconChar = iconChar;

            view = new CustomPinView(BackgroundDefault, iconChar);

            pin = new Pin()
            {
                Label = name,
                Address = address,
                Position = new Position(latitude, longtitude),
                Icon = BitmapDescriptorFactory.FromView(view)
            };
        }

        public void setAsSelected()
        {
            view = new CustomPinView(BackgroundSelected, iconChar);
            pin.Icon = BitmapDescriptorFactory.FromView(view);
        }

        public void resetSelection()
        {
            view = new CustomPinView(BackgroundDefault, iconChar);
            pin.Icon = BitmapDescriptorFactory.FromView(view);
        }
    }
}

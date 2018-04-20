using Xamarin.Forms;

namespace AgoraNavigator
{
    static class Configuration
    {
        public const string FirebaseEndpoint = "https://agora-ada18.firebaseio.com";
    }
    static class AgoraColor
    {
        public static Color Blue =     Color.FromHex("47c0ff");
        public static Color DarkBlue = Color.FromHex("061d3f");
        public static Color Dark =     Color.FromHex("3c3c3c");
        public static Color Gray =     Color.FromHex("979797");
        public static Color LightGray =Color.FromHex("eaeaea");
    }

    static class AgoraFonts
    {
        public static string GetPoppinsRegular()
        {
            return Device.RuntimePlatform == Device.iOS ? "Poppins-Regular" : "Poppins-Regular.ttf#Poppins-Regular";
        }
        public static string GetPoppinsMedium()
        {
            return Device.RuntimePlatform == Device.iOS ? "Poppins-Medium" : "Poppins-Medium.ttf#Poppins-Medium";
        }
        public static string GetPoppinsBold()
        {
            return Device.RuntimePlatform == Device.iOS ? "Poppins-Bold" : "Poppins-SemiBold.ttf#Poppins-SemiBold";
        }
        public static string GetFontAwesome()
        {
            return Device.RuntimePlatform == Device.iOS ? "Font-Awesome" : "Font-Awesome.ttf#Font-Awesome";
        }
    }
}

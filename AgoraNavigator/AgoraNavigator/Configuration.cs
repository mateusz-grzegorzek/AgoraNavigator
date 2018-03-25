using Xamarin.Forms;

namespace AgoraNavigator
{
    static class Configuration
    {
        public const string FirebaseEndpoint = "https://agora-ada18.firebaseio.com";
    }
    static class AgoraColor
    {
        public static Color Blue = Color.FromHex("47c0ff");
        public static Color DarkBlue = Color.FromHex("061d3f");
    }
    static class AgoraFonts
    {
        public static string GetPoppinsMedium()
        {
            return "Poppins-Medium.ttf#Poppins-Medium";
        }
        public static string GetPoppinsBold()
        {
            return "Poppins-Bold.ttf#Poppins-Bold";
        }
    }
}

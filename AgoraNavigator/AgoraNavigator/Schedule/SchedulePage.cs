using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{
    public class SchedulePage : NavigationPage
    {
        public static ScheduleDaysPage scheduleDaysPage;

        public SchedulePage()
        {
            BarTextColor = AgoraColor.Blue;
            scheduleDaysPage = new ScheduleDaysPage();
            Navigation.PushAsync(scheduleDaysPage);
        }
    }
}

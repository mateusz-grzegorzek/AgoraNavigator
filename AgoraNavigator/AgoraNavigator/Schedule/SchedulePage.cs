using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{
    public class SchedulePage : NavigationPage
    {
        private static ScheduleDaysPage scheduleDaysPage;

        public SchedulePage()
        {
            scheduleDaysPage = new ScheduleDaysPage();
            Navigation.PushAsync(scheduleDaysPage);
        }
    }
}

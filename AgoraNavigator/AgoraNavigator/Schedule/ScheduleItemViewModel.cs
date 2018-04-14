using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{
    public class ScheduleItemViewModel
    {
        public ScheduleItem scheduleItem;
        public string Time;

        public ScheduleItemViewModel(ScheduleItem item)
        {
            scheduleItem = item;
            if (scheduleItem.EndTime.Year != 1)
            {
                Time = scheduleItem.StartTime.ToShortTimeString() + " - " + scheduleItem.EndTime.ToShortTimeString();
            }
            else
            {
                Time = scheduleItem.StartTime.ToShortTimeString();
            }
        }

        public string TimeText => Time;

        public string Title => scheduleItem.Title;

        public Color Color => scheduleItem.Color;

    }
}

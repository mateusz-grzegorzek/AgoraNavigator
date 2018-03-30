using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{
    public class ScheduleItemViewModel
    {
        public ScheduleItem scheduleItem;

        public ScheduleItemViewModel(ScheduleItem item)
        {
            scheduleItem = item;
        }

        public ScheduleItemViewModel(ScheduleItemViewModel viewItem)
        {
            scheduleItem = viewItem.scheduleItem;
        }

        public string TimeText => scheduleItem.StartTime.ToShortTimeString() + " - " + scheduleItem.EndTime.ToShortTimeString();

        public string Title => scheduleItem.Title;

        public Color Color => scheduleItem.Color;

    }
}

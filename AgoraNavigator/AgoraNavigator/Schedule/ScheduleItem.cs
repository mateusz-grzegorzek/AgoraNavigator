using System;
using Xamarin.Forms;

namespace AgoraNavigator.Domain.Schedule
{
    public class ScheduleItem
    {
        public string Title { get; set; }

        public string Presenter { get; set; }

        public DateTime StartTime { get; set; }

        public Color Color { get; set; }
    }
}

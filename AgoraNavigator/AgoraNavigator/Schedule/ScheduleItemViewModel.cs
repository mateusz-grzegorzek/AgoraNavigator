using System;
using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{
    class ScheduleItemViewModel
    {
        public ScheduleItemViewModel(ScheduleItem item)
        {
            Title = item.Title;
            Presenter = item.Presenter;
            StartTime = item.StartTime;
            EndTime = item.EndTime;
            Color = item.Color;
        }

        public string Title { get; set; }

        public string Presenter { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string TimeText => StartTime.ToShortTimeString() + " - " + EndTime.ToShortTimeString();

        public Color Color { get; set; }
    }
}

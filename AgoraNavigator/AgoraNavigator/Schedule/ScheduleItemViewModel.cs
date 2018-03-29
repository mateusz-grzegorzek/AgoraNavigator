using System;
using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{
    public class ScheduleItemViewModel
    {
        public ScheduleItemViewModel(ScheduleItem item)
        {
            Title = item.Title;
            StartTime = item.StartTime;
            EndTime = item.EndTime;
            Color = item.Color;
            Place = item.Place; ;
            Address = item.Address;
            Description = item.Description;
            Coords = item.Coords;
        }

        public string Title { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string TimeText => StartTime.ToShortTimeString() + " - " + EndTime.ToShortTimeString();

        public Color Color { get; set; }

        public string Place { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public double[] Coords { get; set; }
    }
}

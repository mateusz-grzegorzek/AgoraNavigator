using System;
using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{
    public class ScheduleItem
    {
        public string Title { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Color Color { get; set; }

        public string Place { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public double[] Coords { get; set; }
    }
}

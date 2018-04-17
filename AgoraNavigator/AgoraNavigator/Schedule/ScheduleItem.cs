using System;
using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{
    public class ScheduleItem
    {
        public int EventId { get; set; }

        public string Title { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Color Color { get; set; }

        public string Place { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public double CoordX { get; set; }

        public double CoordY { get; set; }
    }
}



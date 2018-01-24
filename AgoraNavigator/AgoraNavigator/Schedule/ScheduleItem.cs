using System;
using System.Collections.Generic;
using System.Text;

namespace AgoraNavigator.Schedule
{
    class ScheduleItem
    {
        public string Title { get; set; }

        public string Presenter { get; set; }

        public DateTime StartTime { get; set; }

        public string StartTimeText => StartTime.ToShortTimeString();
    }
}

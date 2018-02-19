using System;
using System.Collections.Generic;
using System.Text;
using AgoraNavigator.Domain.Schedule;

namespace AgoraNavigator.Schedule
{
    class ScheduleItemViewModel
    {
        public ScheduleItemViewModel()
        {

        }

        public ScheduleItemViewModel(ScheduleItem item)
        {
            Title = item.Title;
            Presenter = item.Presenter;
            StartTime = item.StartTime;
        }

        public string Title { get; set; }

        public string Presenter { get; set; }

        public DateTime StartTime { get; set; }

        public string StartTimeText => StartTime.ToShortTimeString();
    }
}

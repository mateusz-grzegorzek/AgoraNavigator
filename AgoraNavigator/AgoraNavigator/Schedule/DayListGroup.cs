using System;
using System.Collections.Generic;
using System.Text;

namespace AgoraNavigator.Schedule
{
    class DayListGroup : List<ScheduleItemViewModel>
    {
        public DayListGroup(DateTime date)
        {
            Date = date;
        }

        public DayListGroup(DateTime date, IEnumerable<ScheduleItemViewModel> items)
        {
            Date = date;
            AddRange(items);
        }

        public DateTime Date { get; set; }

        public string DayName => Date.ToString("dddd, d MMM");

        public IEnumerable<ScheduleItemViewModel> Items => this;
    }
}

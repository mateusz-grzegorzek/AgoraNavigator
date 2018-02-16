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

        public DateTime Date { get; set; }

        public string DayName => Date.ToString("M");
    }
}

using AgoraNavigator.Domain.Schedule;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{

    public class ScheduleDaysPage : CarouselPage
    {
        private const string _databaseScheduleKey = "schedule";

        public ScheduleDaysPage()
        {
            Title = "Schedule";

            Appearing += OnPageAppearing;

            ItemsSource = new ObservableCollection<DayListGroup>();

            ItemTemplate = new DataTemplate(typeof(ScheduleDayPage));
        }

        private async void OnPageAppearing(object sender, EventArgs e)
        {
            this.IsBusy = true;
            await FetchScheduleAsync();
            this.IsBusy = false;
        }

        private async Task FetchScheduleAsync()
        {
            var items = await FirebaseMessagingClient.SendQuery<ScheduleItem>(_databaseScheduleKey);

            var groupedItems = items
                .GroupBy(item => item.Object.StartTime.Date, item => item.Object)
                .Select(group => new DayListGroup(
                    group.Key,
                    group.Select(item => new ScheduleItemViewModel(item))
                ));

            var days = (ObservableCollection<DayListGroup>)ItemsSource;

            days.Clear();

            foreach (var scheduleItem in groupedItems)
            {
                days.Add(scheduleItem);
            }
        }
    }
}

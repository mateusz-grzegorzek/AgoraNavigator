using AgoraNavigator.Domain.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{

    public class ScheduleDaysPage : CarouselPage
    {
        private const string _databaseScheduleKey = "schedule";
        private static int counter;

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
            try
            {
                var items = await FirebaseMessagingClient.SendQuery<ScheduleItem>(_databaseScheduleKey);
                counter = 0;
                foreach (var groups in items)
                {
                    SaveFiles(groups.Object);
                }

                var groupedItems = items
                .GroupBy(item => item.Object.StartTime.Date, item => item.Object)
                .Select(group => new DayListGroup(
                    group.Key,
                    group.Select(item => new ScheduleItemViewModel(item))
                ));

                processDays(groupedItems);

            } catch(Exception e)
            {
                DependencyService.Get<INotification>().Notify("Brak połączenia z internetem", "Plan może być nie aktualny, włącz internet w celu aktualizacji");
                List<ScheduleItem> itemList = new List<ScheduleItem>();
                while (counter != 0)
                {
                    ScheduleItem item = new ScheduleItem();
                    item.Title = Plugin.Settings.CrossSettings.Current.GetValueOrDefault(String.Format("{0}", --counter), "");
                    item.StartTime = DateTime.Parse(Plugin.Settings.CrossSettings.Current.GetValueOrDefault(String.Format("{0}", --counter), ""));
                    item.Presenter = Plugin.Settings.CrossSettings.Current.GetValueOrDefault(String.Format("{0}", --counter), "");

                    itemList.Add(item);
                }

                var groupedItems = itemList.GroupBy(value => value.StartTime.Date, value => value)
                    .Select(group => new DayListGroup(
                group.Key,
                group.Select(it => new ScheduleItemViewModel(it))));

                processDays(groupedItems);
            }
        }

        private void processDays(IEnumerable<DayListGroup> groupedItems)
        {
            var days = (ObservableCollection<DayListGroup>)ItemsSource;

            days.Clear();
            foreach (var scheduleItem in groupedItems)
            {
                days.Add(scheduleItem);
            }
        }

        private void SaveFiles(ScheduleItem item)
        {
            Plugin.Settings.CrossSettings.Current.AddOrUpdateValue(String.Format("{0}", counter++), item.Presenter);
            Plugin.Settings.CrossSettings.Current.AddOrUpdateValue(String.Format("{0}", counter++), item.StartTime.Date.ToString());
            Plugin.Settings.CrossSettings.Current.AddOrUpdateValue(String.Format("{0}", counter++), item.Title);
        }
    }
}

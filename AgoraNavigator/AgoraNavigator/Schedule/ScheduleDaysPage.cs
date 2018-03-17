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
                List<ScheduleItem> itemList = new List<ScheduleItem>();
                counter = Plugin.Settings.CrossSettings.Current.GetValueOrDefault("counter", 0);
                if(counter > 0)
                {
                    while (counter != 0)
                    {
                        ScheduleItem item = new ScheduleItem();
                        item.Title = Plugin.Settings.CrossSettings.Current.GetValueOrDefault(String.Format("{0}", --counter), "");
                        item.StartTime = DateTime.Parse(Plugin.Settings.CrossSettings.Current.GetValueOrDefault(String.Format("{0}", --counter), ""));
                        item.Presenter = Plugin.Settings.CrossSettings.Current.GetValueOrDefault(String.Format("{0}", --counter), "");

                        itemList.Add(item);
                    }
                }
                else
                {
                    LoadDefaultData(itemList);
                }
                DependencyService.Get<INotification>().Notify("No internet connection", "The plan may be out of date, turn on the internet for updates");

                var groupedItems = itemList.GroupBy(value => value.StartTime.Date, value => value)
                    .Select(group => new DayListGroup(
                group.Key,
                group.Select(it => new ScheduleItemViewModel(it))));

                processDays(groupedItems);
            }
        }

        private static void LoadDefaultData(List<ScheduleItem> itemList)
        {
            AddItemToList(itemList, "Opening Ceremony", DateTime.Parse("2017-04-23T13:00:00"), "Chuck Norris");
            AddItemToList(itemList, "The Pierogi Workshop", DateTime.Parse("2017-04-24T13:00:00"), "Andrzej Duda");
            AddItemToList(itemList, "Melanż & Drinking Presentation", DateTime.Parse("2017-04-24T14:15:00"), "Owca");
            AddItemToList(itemList, "Another lecture", DateTime.Parse("2017-04-24T18:00:00"), "Michael Jackson");
            AddItemToList(itemList, "Lorem Ipsum", DateTime.Parse("2017-04-25T13:00:00"), "Bill Gates");
            AddItemToList(itemList, "Sid Domet", DateTime.Parse("2017-04-26"), "Franek Kimono");
            AddItemToList(itemList, "Lelum Polelum", DateTime.Parse("2017-04-27"), "Ferdynand Kiepski");
        }

        private static void AddItemToList(List<ScheduleItem> itemList, String title, DateTime date, String presenter)
        {
            ScheduleItem item = new ScheduleItem();
            item.Title = title;
            item.StartTime = date;
            item.Presenter = presenter;
            itemList.Add(item);
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
            Plugin.Settings.CrossSettings.Current.AddOrUpdateValue("counter", counter);
        }
    }
}

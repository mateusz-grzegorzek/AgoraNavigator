using AgoraNavigator.Popup;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using CrossSettings = Plugin.Settings.CrossSettings;

namespace AgoraNavigator.Schedule
{

    public class ScheduleDaysPage : CarouselPage
    {
        private const string databaseScheduleKey = "schedule";
        private bool userInformedAboutScheduleOutOfDate = false;

        public ScheduleDaysPage()
        {
            Title = "Schedule";
            Appearing += OnAppearing;
            ItemsSource = new ObservableCollection<DayListGroup>();
            ItemTemplate = new DataTemplate(typeof(ScheduleDayPage));

            int numberOfEvents = CrossSettings.Current.GetValueOrDefault("Schedule_numberOfEvents", 0);
            if(numberOfEvents > 0)
            {
                LoadEventsFromMemory(numberOfEvents);
            }
        }

        public async void OnAppearing(object sender, EventArgs e)
        {
            bool userInformedAboutUsage = CrossSettings.Current.GetValueOrDefault("userInformedAboutUsage", false);
            if (!userInformedAboutUsage)
            {
                DependencyService.Get<IPopup>().ShowPopup("Schedule usage", "Swipe left or right to change days!", true);
                CrossSettings.Current.AddOrUpdateValue("userInformedAboutUsage", true);
            }
            await FetchScheduleAsync();
        }

        public async Task FetchScheduleAsync(bool forceUpdate = false)
        {
            int versionInDb = await FirebaseMessagingClient.SendSingleQuery<int>(databaseScheduleKey + "/version");
            int versionInMemory = CrossSettings.Current.GetValueOrDefault("Schedule:version", 0);
            if ((versionInMemory < versionInDb) || forceUpdate)
            {
                try
                {
                    List<ScheduleItem> itemList = new List<ScheduleItem>();
                    IReadOnlyCollection<FirebaseObject<ScheduleItem>> items = await FirebaseMessagingClient.SendQuery<ScheduleItem>(databaseScheduleKey + "/events");
                    DependencyService.Get<IPopup>().ShowPopup("Schedule updating...", "It may take some time!", true);
                    int eventNumber = 1;
                    foreach (FirebaseObject<ScheduleItem> groups in items)
                    {
                        SaveEventToMemory(groups.Object, eventNumber);
                        itemList.Add(groups.Object);
                        eventNumber++;
                    }
                    ProcessDays(itemList);
                    CrossSettings.Current.AddOrUpdateValue("Schedule:version", versionInDb);
                    userInformedAboutScheduleOutOfDate = false;
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.ToString());
                    if(!userInformedAboutScheduleOutOfDate)
                    {
                        DependencyService.Get<IPopup>().ShowPopup("No internet connection", "Schedule may be out of date, turn on the internet for updates", false);
                        userInformedAboutScheduleOutOfDate = true;
                    }
                }
            }   
        }

        private void LoadEventsFromMemory(int numberOfEvents)
        {
            List<ScheduleItem> eventsList = new List<ScheduleItem>();
            while (numberOfEvents != 0)
            {
                ScheduleItem item = new ScheduleItem
                {
                    Title = CrossSettings.Current.GetValueOrDefault("Schedule_Title_" + numberOfEvents, ""),
                    StartTime = DateTime.Parse(CrossSettings.Current.GetValueOrDefault("Schedule_StartTime_" + numberOfEvents, "")),
                    EndTime = DateTime.Parse(CrossSettings.Current.GetValueOrDefault("Schedule_EndTime_" + numberOfEvents, "")),
                    Description = CrossSettings.Current.GetValueOrDefault("Schedule_Description_" + numberOfEvents, ""),
                    Place = CrossSettings.Current.GetValueOrDefault("Schedule_Place_" + numberOfEvents, ""),
                    Address = CrossSettings.Current.GetValueOrDefault("Schedule_Address_" + numberOfEvents, ""),
                    CoordX = CrossSettings.Current.GetValueOrDefault("Schedule_CoordX_" + numberOfEvents, 50.0608255),
                    CoordY = CrossSettings.Current.GetValueOrDefault("Schedule_CoordY_" + numberOfEvents, 19.9309346)
                };
                eventsList.Add(item);
                numberOfEvents--;

            }
            ProcessDays(eventsList);
        }

        private void ProcessDays(List<ScheduleItem> eventsList)
        {
            IEnumerable<DayListGroup> groupedItems = eventsList
                .OrderBy(item => item.StartTime)
                .GroupBy(item => item.StartTime.Date, item => item)
                .Select(group => new DayListGroup(
                    group.Key,
                    group.Select(item => new ScheduleItemViewModel(item))
                ));
            ObservableCollection<DayListGroup> days = (ObservableCollection<DayListGroup>)ItemsSource;
            days.Clear();
            foreach (DayListGroup scheduleItem in groupedItems)
            {
                foreach (ScheduleItemViewModel item in scheduleItem)
                {
                    if (item.scheduleItem.StartTime < DateTime.Now)
                    {
                        item.scheduleItem.Color = AgoraColor.LightGray;
                    }
                    else
                    {
                        item.scheduleItem.Color = Color.White;
                    }
                }
                days.Add(scheduleItem);
            }
        }

        private void SaveEventToMemory(ScheduleItem oneEvent, int eventNumber)
        {
            CrossSettings.Current.AddOrUpdateValue("Schedule_Title_" + eventNumber, oneEvent.Title);
            CrossSettings.Current.AddOrUpdateValue("Schedule_StartTime_" + eventNumber, oneEvent.StartTime.ToString());
            CrossSettings.Current.AddOrUpdateValue("Schedule_EndTime_" + eventNumber, oneEvent.EndTime.ToString());
            CrossSettings.Current.AddOrUpdateValue("Schedule_Description_" + eventNumber, oneEvent.Description);
            CrossSettings.Current.AddOrUpdateValue("Schedule_Place_" + eventNumber, oneEvent.Place);
            CrossSettings.Current.AddOrUpdateValue("Schedule_Address_" + eventNumber, oneEvent.Address);
            CrossSettings.Current.AddOrUpdateValue("Schedule_CoordX_" + eventNumber, oneEvent.CoordX);
            CrossSettings.Current.AddOrUpdateValue("Schedule_CoordY_" + eventNumber, oneEvent.CoordY);
            CrossSettings.Current.AddOrUpdateValue("Schedule_numberOfEvents", eventNumber);
        }
    }
}

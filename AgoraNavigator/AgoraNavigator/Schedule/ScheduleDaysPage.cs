using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using CrossSettings = Plugin.Settings.CrossSettings;

namespace AgoraNavigator.Schedule
{

    public class ScheduleDaysPage : CarouselPage
    {
        private const string _databaseScheduleKey = "schedule";
        public bool scheduleUpToDate = false;
        private bool userInformedAboutScheduleOutOfDate = false;

        public ScheduleDaysPage()
        {
            Title = "Schedule";
            ItemsSource = new ObservableCollection<DayListGroup>();
            ItemTemplate = new DataTemplate(typeof(ScheduleDayPage));

            int numberOfEvents = CrossSettings.Current.GetValueOrDefault("Schedule_numberOfEvents", 0);
            if(numberOfEvents > 0)
            {
                LoadEventsFromMemory(numberOfEvents);
            }
            else
            {
                LoadDefaultEvents();
            } 
        }

        public async void FetchScheduleAsync(bool forceUpdate = false)
        {
            if(!scheduleUpToDate || forceUpdate)
            {
                try
                {
                    List<ScheduleItem> itemList = new List<ScheduleItem>();
                    IReadOnlyCollection<FirebaseObject<ScheduleItem>> items = await FirebaseMessagingClient.SendQuery<ScheduleItem>(_databaseScheduleKey);
                    int eventNumber = 1;
                    foreach (FirebaseObject<ScheduleItem> groups in items)
                    {
                        SaveEventToMemory(groups.Object, eventNumber);
                        itemList.Add(groups.Object);
                        eventNumber++;
                    }
                    ProcessDays(itemList);
                    scheduleUpToDate = true;
                    userInformedAboutScheduleOutOfDate = false;
                }
                catch (Exception)
                {
                    if(!userInformedAboutScheduleOutOfDate)
                    {
                        DependencyService.Get<INotification>().Notify("No internet connection", "Schedule may be out of date, turn on the internet for updates");
                        userInformedAboutScheduleOutOfDate = true;
                    }
                }
            }   
        }

        private void LoadDefaultEvents()
        {
            List<ScheduleItem> eventsList = new List<ScheduleItem>();
            eventsList.Add(new ScheduleItem
            {
                Title = "Lelum Polelum",
                StartTime = DateTime.Parse("2018-03-10T10:00:00"),
                EndTime = DateTime.Parse("2018-03-10T11:00:00"),
                Description = "Event description",
                Place = "Collegium Novum",
                Address = "ul. Gołębia 21",
                Coords = new double[2]{ 50.0608255, 19.9309346 }
            });
            eventsList.Add(new ScheduleItem
            {
                Title = "Opening Ceremony",
                StartTime = DateTime.Parse("2018-04-23T20:00:00"),
                EndTime = DateTime.Parse("2018-04-23T23:00:00"),
                Description = "Event description",
                Place = "Collegium Novum",
                Address = "ul. Gołębia 21",
                Coords = new double[2] { 50.0608255, 19.9309346 }
            });
            eventsList.Add(new ScheduleItem
            {
                Title = "The Pierogi Workshop",
                StartTime = DateTime.Parse("2018-04-24T10:00:00"),
                EndTime = DateTime.Parse("2018-04-24T11:00:00"),
                Description = "Event description",
                Place = "Collegium Novum",
                Address = "ul. Gołębia 21",
                Coords = new double[2] { 50.0608255, 19.9309346 }
            });
            eventsList.Add(new ScheduleItem
            {
                Title = "Melanż & Drinking Presentation",
                StartTime = DateTime.Parse("2018-04-24T12:00:00"),
                EndTime = DateTime.Parse("2018-04-24T14:00:00"),
                Description = "Event description",
                Place = "Collegium Novum",
                Address = "ul. Gołębia 21",
                Coords = new double[2] { 50.0608255, 19.9309346 }
            });
            eventsList.Add(new ScheduleItem
            {
                Title = "Another lecture",
                StartTime = DateTime.Parse("2018-04-25T10:00:00"),
                EndTime = DateTime.Parse("2018-04-25T11:00:00"),
                Description = "Event description",
                Place = "Collegium Novum",
                Address = "ul. Gołębia 21",
                Coords = new double[2] { 50.0608255, 19.9309346 }
            });
            ProcessDays(eventsList);
        }

        private void LoadEventsFromMemory(int numberOfEvents)
        {
            List<ScheduleItem> eventsList = new List<ScheduleItem>();
            while (numberOfEvents != 0)
            {
                ScheduleItem item = new ScheduleItem();
                item.Title = CrossSettings.Current.GetValueOrDefault("Schedule_Title_" + numberOfEvents, "");
                item.StartTime = CrossSettings.Current.GetValueOrDefault("Schedule_StartTime_" + numberOfEvents, new DateTime());
                item.EndTime = CrossSettings.Current.GetValueOrDefault("Schedule_EndTime_" + numberOfEvents, new DateTime());
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
                    if (item.StartTime < DateTime.Now)
                    {
                        item.Color = AgoraColor.LightGray;
                    }
                    else
                    {
                        item.Color = Color.White;
                    }
                }
                days.Add(scheduleItem);
            }
        }

        private void SaveEventToMemory(ScheduleItem oneEvent, int eventNumber)
        {
            CrossSettings.Current.AddOrUpdateValue("Schedule_Title_" + eventNumber, oneEvent.Title);
            CrossSettings.Current.AddOrUpdateValue("Schedule_StartTime_" + eventNumber, oneEvent.StartTime);
            CrossSettings.Current.AddOrUpdateValue("Schedule_EndTime_" + eventNumber, oneEvent.EndTime);
            CrossSettings.Current.AddOrUpdateValue("Schedule_numberOfEvents", eventNumber);
        }
    }
}

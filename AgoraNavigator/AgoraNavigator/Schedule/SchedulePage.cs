using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{
    public class SchedulePage : ContentPage
    {
        private ObservableCollection<DayListGroup> _scheduleItems { get; set; }
        private ListView _scheduleItemsListView;

        public SchedulePage()
        {
            _scheduleItems = new ObservableCollection<DayListGroup>
            {
                new DayListGroup(new DateTime(2017, 4, 23))
                {
                    new ScheduleItem()
                    {
                        Title = "Opening Ceremony",
                        Presenter = "Chuck Norris",
                        StartTime = new DateTime(2017, 4, 23, 12, 00, 00)
                    }
                },
                new DayListGroup(new DateTime(2017, 4, 24))
                {
                    new ScheduleItem()
                    {
                        Title = "The Pierogi Workshop",
                        Presenter = "Andrzej Duda",
                        StartTime = new DateTime(2017, 4, 24, 13, 00, 00)
                    },
                    new ScheduleItem()
                    {
                        Title = "Melanż & Drinking Presentation",
                        Presenter = "Owca",
                        StartTime = new DateTime(2017, 4, 24, 14, 15, 00)
                    }
                }
            };

            _scheduleItemsListView = new ListView()
            {
                ItemsSource = _scheduleItems,
                ItemTemplate = new DataTemplate(typeof(ScheduleItemCell)),
                IsGroupingEnabled = true,
                GroupDisplayBinding = new Binding("DayName"),
                HasUnevenRows = true
            };

            _scheduleItemsListView.ItemSelected += OnScheduleItemSelected;

            Content = _scheduleItemsListView;
        }

        private void OnScheduleItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}

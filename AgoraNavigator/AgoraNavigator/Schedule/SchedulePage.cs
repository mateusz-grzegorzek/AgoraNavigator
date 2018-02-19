using AgoraNavigator.Domain.Schedule;
using Firebase.Database;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{
    public class SchedulePage : NavigationPage
    {
        public static ScheduleMasterPage scheduleMasterPage;

        public SchedulePage()
        {
            scheduleMasterPage = new ScheduleMasterPage();
            Navigation.PushAsync(scheduleMasterPage);
        }
    }

    public class ScheduleMasterPage : ContentPage
    {
        private ObservableCollection<DayListGroup> _scheduleItems { get; set; }
        private ListView _scheduleItemsListView;
        private const string _databaseScheduleKey = "schedule";

        public ScheduleMasterPage()
        {
            Title = "Schedule";

            _scheduleItems = new ObservableCollection<DayListGroup>();

            _scheduleItemsListView = new ListView()
            {
                ItemsSource = _scheduleItems,
                ItemTemplate = new DataTemplate(typeof(ScheduleItemCell)),
                IsGroupingEnabled = true,
                GroupDisplayBinding = new Binding("DayName"),
                HasUnevenRows = true,
            };

            _scheduleItemsListView.ItemSelected += OnScheduleItemSelected;

            Appearing += OnPageAppearing;

            var stack = new StackLayout();
            stack.Children.Add(_scheduleItemsListView);
            Content = stack;
        }

        private async void OnPageAppearing(object sender, EventArgs e)
        {
            _scheduleItemsListView.IsRefreshing = true;
            await FetchScheduleAsync();
            _scheduleItemsListView.IsRefreshing = false;
        }

        private async Task FetchScheduleAsync()
        {
            var firebaseClient = new FirebaseClient(Configuration.FirebaseEndpoint);

            var items = await firebaseClient
                .Child(_databaseScheduleKey)
                .OnceAsync<ScheduleItem>();

            var groupedItems = items
                .GroupBy(item => item.Object.StartTime.Date, item => item.Object)
                .Select(group => new DayListGroup(
                    group.Key, 
                    group.Select(item => new ScheduleItemViewModel(item))
                ));

            _scheduleItems.Clear();
            foreach (var scheduleItem in groupedItems)
            {
                _scheduleItems.Add(scheduleItem);
            }
        }

        private void OnScheduleItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}

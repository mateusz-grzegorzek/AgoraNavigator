using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{

    public class ScheduleDayPage : ContentPage
    {
        private ListView _scheduleItemsListView;
        private Label _dayTitleLabel;

        public ScheduleDayPage()
        {
            _dayTitleLabel = new Label()
            {
                Style = Device.Styles.SubtitleStyle,
                FontSize = 24
            };
            _dayTitleLabel.SetBinding(Label.TextProperty, "DayName");

            _scheduleItemsListView = new ListView()
            {
                ItemTemplate = new DataTemplate(typeof(ScheduleItemCell)),
                HasUnevenRows = true,
            };
            _scheduleItemsListView.SetBinding(ListView.ItemsSourceProperty, "Items");
            _scheduleItemsListView.ItemSelected += OnScheduleItemSelected;

            var stack = new StackLayout();
            stack.Children.Add(_dayTitleLabel);
            stack.Children.Add(_scheduleItemsListView);
            Content = stack;
        }

        private void OnScheduleItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}

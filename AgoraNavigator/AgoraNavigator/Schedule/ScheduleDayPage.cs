using System;
using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{

    public class ScheduleDayPage : ContentPage
    {

        public ScheduleDayPage()
        {
            Label dayNameLabel = new Label()
            {
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = AgoraColor.Dark,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };
            dayNameLabel.SetBinding(Label.TextProperty, "DayName");

            Label dayLabel = new Label()
            {
                FontFamily = AgoraFonts.GetPoppinsRegular(),
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = AgoraColor.Dark,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };
            dayLabel.SetBinding(Label.TextProperty, "Day");

            ListView scheduleItemsListView = new ListView()
            {
                ItemTemplate = new DataTemplate(typeof(ScheduleItemCell)),
                HasUnevenRows = true,
            };
            scheduleItemsListView.SetBinding(ListView.ItemsSourceProperty, "Items");
            scheduleItemsListView.ItemSelected += OnScheduleItemSelected;

            StackLayout stack = new StackLayout();
            stack.Children.Add(dayNameLabel);
            stack.Children.Add(dayLabel);
            stack.Children.Add(scheduleItemsListView);
            Content = stack;
        }

        private void OnScheduleItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as ScheduleItemViewModel;

            if (item != null)
            {
                ScheduleEventDetails eventDetails = new ScheduleEventDetails(item);
                Navigation.PushAsync(eventDetails);
            }
        }
    }
}

using System;
using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{
    class ScheduleItemCell : ViewCell
    {
        public ScheduleItemCell()
        {
            StackLayout cellWrapper = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,

            };
            StackLayout eventDescriptionWrapper = new StackLayout()
            {
                Orientation = StackOrientation.Vertical
            };

            Label titleLabel = new Label()
            {
                Style = Device.Styles.ListItemTextStyle
            };
            Label presenterLabel = new Label()
            {
                Style = Device.Styles.BodyStyle
            };
            Label timeLabel = new Label()
            {
                Style = Device.Styles.ListItemTextStyle
            };

            titleLabel.SetBinding(Label.TextProperty, "Title");
            presenterLabel.SetBinding(Label.TextProperty, "Presenter");
            timeLabel.SetBinding(Label.TextProperty, "StartTimeText");

            eventDescriptionWrapper.Children.Add(titleLabel);
            eventDescriptionWrapper.Children.Add(presenterLabel);

            cellWrapper.Children.Add(timeLabel);
            cellWrapper.Children.Add(eventDescriptionWrapper);
            cellWrapper.SetBinding(VisualElement.BackgroundColorProperty, "Color");

            View = cellWrapper;
        }
        
    }
}

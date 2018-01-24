using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{
    class ScheduleItemCell : ViewCell
    {
        public ScheduleItemCell()
        {
            StackLayout cellWrapper = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal
            };
            StackLayout eventDescriptionWrapper = new StackLayout()
            {
                Orientation = StackOrientation.Vertical
            };

            Label titleLabel = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
            };
            Label presenterLabel = new Label();
            Label timeLabel = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
            };

            titleLabel.SetBinding(Label.TextProperty, "Title");
            presenterLabel.SetBinding(Label.TextProperty, "Presenter");
            timeLabel.SetBinding(Label.TextProperty, "StartTimeText");

            eventDescriptionWrapper.Children.Add(titleLabel);
            eventDescriptionWrapper.Children.Add(presenterLabel);

            cellWrapper.Children.Add(timeLabel);
            cellWrapper.Children.Add(eventDescriptionWrapper);

            View = cellWrapper;
        }
        
    }
}

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

            Grid grid = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(10, 10)
            };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });

            Label timeLabel = new Label()
            {
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = AgoraColor.Blue,
                HorizontalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            Label titleLabel = new Label()
            {
                FontFamily = AgoraFonts.GetPoppinsRegular(),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = AgoraColor.Dark,
                HorizontalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            Image arrow = new Image
            {
                Source = "Arrow.png",
                HorizontalOptions = LayoutOptions.End,
                Margin = new Thickness(10, 10)
            };

            timeLabel.SetBinding(Label.TextProperty, "TimeText");
            titleLabel.SetBinding(Label.TextProperty, "Title");

            grid.Children.Add(timeLabel, 0, 0);
            grid.Children.Add(titleLabel, 0, 1);

            Grid.SetRowSpan(arrow, 2);
            grid.Children.Add(arrow, 1, 2, 0, 2);

            cellWrapper.Children.Add(grid);
            cellWrapper.SetBinding(VisualElement.BackgroundColorProperty, "Color");

            View = cellWrapper;
        }
        
    }
}

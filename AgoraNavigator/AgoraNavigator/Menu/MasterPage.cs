using AgoraNavigator.Schedule;
using AgoraNavigator.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;
using AgoraNavigator.GoogleMap;
using AgoraNavigator.Contact;
using AgoraNavigator.Info;
using AgoraNavigator.Downloads;

namespace AgoraNavigator.Menu
{
    public class MasterPage : ContentPage
    {
        public ListView getListView { get { return listView; } }

        ListView listView;

        public MasterPage()
        {
            var masterPageItems = new List<MasterPageItem>();
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Map",
                IconSource = "hamburger.png",
                TargetType = typeof(MapPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Schedule",
                IconSource = "hamburger.png",
                TargetType = typeof(SchedulePage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Tasks",
                IconSource = "hamburger.png",
                TargetType = typeof(TasksPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Contact",
                TargetType = typeof(ContactPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Important info",
                TargetType = typeof(InfoPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Downloads",
                TargetType = typeof(DownloadsPage)
            });

            listView = new ListView
            {
                ItemsSource = masterPageItems,
                ItemTemplate = new DataTemplate(() =>
                {
                    Grid grid = new Grid
                    {
                        Padding = new Thickness(1, 1),
                    };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150)});

                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Star) });
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                    Image icon = new Image
                    {
                        Source = "menu_icon.png",
                        HorizontalOptions = LayoutOptions.End,
                        Margin = new Thickness(4, 4)
                    };
                    Label menuOption = new Label
                    {
                        VerticalOptions = LayoutOptions.Center
                    };
                    menuOption.SetBinding(Label.TextProperty, "Title");
                    menuOption.FontFamily = AgoraFonts.GetPoppinsMedium();
                    menuOption.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    menuOption.TextColor = AgoraColor.DarkBlue;
                    Image separator = new Image { Source = "menu_separator.png" };

                    grid.Children.Add(menuOption);
                    grid.Children.Add(separator, 0, 1);
                    return new ViewCell { View = grid };
                }),
                SeparatorVisibility = SeparatorVisibility.None
            };

            Icon = "menu_separator.png";
            Title = "Agora Navigator";
            BackgroundColor = AgoraColor.Blue;
            Padding = new Thickness(20, 40, 0, 0);

            var stack = new StackLayout();
            stack.Children.Add(listView);
            Content = stack;
        }
    }
}

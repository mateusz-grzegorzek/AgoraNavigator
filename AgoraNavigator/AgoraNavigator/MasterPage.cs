using System.Collections.Generic;
using Xamarin.Forms;

namespace AgoraNavigator
{
    public class MasterPage : ContentPage
    {
        public ListView getListView { get { return listView; } }

        public void ShowMenu()
        {
            listView.IsVisible = true;
            listView.IsEnabled = true;
        }

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
                Title = "Timeline",
                IconSource = "hamburger.png",
                TargetType = typeof(TimelinePage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Tasks",
                IconSource = "hamburger.png",
                TargetType = typeof(TasksPage)
            });

            listView = new ListView
            {
                ItemsSource = masterPageItems,
                ItemTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid { Padding = new Thickness(5, 10) };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                    var image = new Image();
                    image.SetBinding(Image.SourceProperty, "IconSource");
                    var label = new Label { VerticalOptions = LayoutOptions.FillAndExpand };
                    label.SetBinding(Label.TextProperty, "Title");

                    grid.Children.Add(image);
                    grid.Children.Add(label, 1, 0);

                    return new ViewCell { View = grid };
                }),
                SeparatorVisibility = SeparatorVisibility.None
            };
            listView.IsVisible = false;
            listView.IsEnabled = false;

            Icon = "hamburger.png";
            Title = "Agora Application";
            Padding = new Thickness(0, 40, 0, 0);
            Content = new StackLayout
            {
                Children = { listView }
            };
        }
    }
}

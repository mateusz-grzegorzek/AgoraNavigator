using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AgoraNavigator.Tasks
{
    public class TasksMasterView : ContentPage
    {
        ListView tasksListView;

        public TasksMasterView(ObservableCollection<GameTask> tasks)
        {
            tasksListView = new ListView
            {
                ItemsSource = tasks,
                ItemTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid { Padding = new Thickness(5, 10) };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                    var image = new Image();
                    image.SetBinding(Image.SourceProperty, "iconSource");
                    var label = new Label { VerticalOptions = LayoutOptions.FillAndExpand };
                    label.SetBinding(Label.TextProperty, "title");

                    grid.Children.Add(image);
                    grid.Children.Add(label, 1, 0);

                    return new ViewCell { View = grid };
                }),
                SeparatorVisibility = SeparatorVisibility.None
            };
            tasksListView.ItemTapped += OnTaskTitleClick;

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(tasksListView);
            Content = stack;
        }

        public void OnTaskTitleClick(object sender, ItemTappedEventArgs e)
        {
            GameTask task = (GameTask)e.Item;
            if (task.completed == false)
            {
                Navigation.PushAsync(new TaskDetailView(task));
            }
        }
    }
}
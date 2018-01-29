using AgoraNavigator.Menu;
using System.Collections.ObjectModel;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using System;

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

        public async void OnTaskTitleClick(object sender, ItemTappedEventArgs e)
        {        
            bool showTaskDetails = false;
            GameTask task = (GameTask)e.Item;
            Console.WriteLine("TasksMasterView:OnTaskTitleClick:task.id=" + task.id + ", completed=" + task.completed);
            if (task.completed == false)
            {
                Console.WriteLine("TasksMasterView:OnTaskTitleClick:task.needBluetoothAndLocation=" + task.needBluetoothAndLocation);
                if (task.needBluetoothAndLocation)
                {
                    bool permissionGranted = await Permissions.GetRuntimePermission(Permission.Location);
                    Console.WriteLine("TasksMasterView:OnTaskTitleClick:permissionGranted=" + permissionGranted);
                    if (Beacons.IsBluetoothOn() && permissionGranted)
                    {
                        showTaskDetails = true;
                    }
                    else
                    {
                        await DisplayAlert("Bluetooth", "Turn on bluetooth and accept location permission to start this task!", "Ok");
                    }
                }
                else
                {
                    showTaskDetails = true;
                }
            }
            if(showTaskDetails)
            {
                await Navigation.PushAsync(new TaskDetailView(task));
            }
        }
    }
}
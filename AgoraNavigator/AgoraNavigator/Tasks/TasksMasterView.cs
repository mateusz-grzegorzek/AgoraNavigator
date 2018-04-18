using AgoraNavigator.Menu;
using System.Collections.ObjectModel;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using System;
using AgoraNavigator.Popup;
using static AgoraNavigator.Tasks.GameTask;

namespace AgoraNavigator.Tasks
{
    public class TasksMasterView : ContentPage
    {
        ListView tasksListView;

        public TasksMasterView(ObservableCollection<GameTask> tasks, bool isOpenTasks)
        {
            tasksListView = new ListView
            {
                BackgroundColor = AgoraColor.DarkBlue,
                SeparatorVisibility = SeparatorVisibility.Default,
                SeparatorColor = AgoraColor.LightGray,
                HasUnevenRows = true,
                ItemsSource = tasks,
                ItemTemplate = new DataTemplate(() =>
                {
                    Grid grid = new Grid
                    {
                        Padding = new Thickness(10, 2),
                        RowSpacing = 1
                    };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
                    if (!isOpenTasks)
                    {
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
                    }

                    Label taskTitleLabel = new Label
                    {
                        TextColor = AgoraColor.Blue,
                        FontFamily = AgoraFonts.GetPoppinsBold(),
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        VerticalTextAlignment = TextAlignment.Center
                    };
                    taskTitleLabel.SetBinding(Label.TextProperty, "taskMasterViewText");
                    Image arrow = new Image
                    {
                        Source = "Arrow.png",
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.End,
                        Margin = new Thickness(10, 10)
                    };

                    grid.Children.Add(taskTitleLabel);
                    if (isOpenTasks)
                    {
                        grid.Children.Add(arrow, 1, 0);
                    }
                    grid.BackgroundColor = AgoraColor.DarkBlue;
                    return new ViewCell { View = grid };
                }),
            };
            tasksListView.ItemTapped += OnTaskTitleClick;

            Content = tasksListView;
            BackgroundColor = AgoraColor.DarkBlue;
            IsEnabled = isOpenTasks ? true : false;
        }

        public async void OnTaskTitleClick(object sender, ItemTappedEventArgs e)
        {
            bool showTaskDetails = false;
            GameTask task = (GameTask)e.Item;
            Console.WriteLine("TasksMasterView:OnTaskTitleClick:task.id=" + task.id + ", completed=" + task.taskStatus);
            if (task.taskStatus == TaskStatus.NotStarted)
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
                        DependencyService.Get<IPopup>().ShowPopup("Bluetooth needed", "Turn on bluetooth and accept location permission to start this task!", false);
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
            ((ListView)sender).SelectedItem = null;
        }
    }
}
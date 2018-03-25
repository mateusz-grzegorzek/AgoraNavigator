﻿using AgoraNavigator.Menu;
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
                    Grid grid = new Grid
                    {
                        Padding = new Thickness(10, 2),
                        RowSpacing = 10
                    };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(35) });

                    Label taskTitle = new Label
                    {
                        TextColor = AgoraColor.Blue,
                        FontFamily = AgoraFonts.GetPoppinsBold(),
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        VerticalOptions = LayoutOptions.Start,
                    };
                    taskTitle.SetBinding(Label.TextProperty, "title");
                    Image arrow = new Image
                    {
                        Source = "arrow.png",
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.End,
                        Margin = new Thickness(2,2)
                    };
                    Image separator1 = new Image
                    {
                        Source = "menu_separator.png",
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.StartAndExpand
                    };
                    Image separator2 = new Image
                    {
                        Source = "menu_separator.png",
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.Start
                    };

                    grid.Children.Add(taskTitle);
                    grid.Children.Add(arrow, 1, 0);
                    grid.Children.Add(separator1, 0, 1);
                    //grid.Children.Add(separator2, 1, 1);
                    Grid.SetColumnSpan(separator1, 2);
                    return new ViewCell { View = grid };
                }),
                SeparatorVisibility = SeparatorVisibility.None
            };
            tasksListView.ItemTapped += OnTaskTitleClick;

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(tasksListView);
            Content = stack;
            BackgroundColor = AgoraColor.DarkBlue;
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
            ((ListView)sender).SelectedItem = null;
        }
    }
}
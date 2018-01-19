using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AgoraNavigator
{
    public class TasksPage : ContentPage
    {
        Label tasks;

        public TasksPage()
        {
            tasks = new Label { Text = "Tasks" };

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(tasks);
            Content = stack;
        }
    }
}
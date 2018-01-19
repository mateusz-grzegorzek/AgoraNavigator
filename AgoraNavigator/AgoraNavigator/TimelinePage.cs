using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AgoraNavigator
{
    public class TimelinePage : ContentPage
    {
        Label timeline;

        public TimelinePage()
        {
            timeline = new Label { Text = "Timeline" };

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(timeline);
            Content = stack;
        }
    }
}
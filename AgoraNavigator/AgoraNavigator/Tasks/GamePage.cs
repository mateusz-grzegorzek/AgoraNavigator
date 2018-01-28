using AgoraNavigator.Login;
using PCLStorage;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace AgoraNavigator.Tasks
{
    public class GamePage : ContentPage, INotifyPropertyChanged
    {
        public static Label totalPointsLabel;

        public GamePage()
        {
            Title = "Game of Tasks";
            totalPointsLabel = new Label();
            totalPointsLabel.Text = "Total points: " + Users.loggedUser.TotalPoints.ToString();
            Button goToTasksButton = new Button { Text = "Go to Tasks!" };
            goToTasksButton.Clicked += OnGoToTasksButtonClick;
            var stack = new StackLayout { Spacing = 12 };
            stack.Children.Add(totalPointsLabel);
            stack.Children.Add(goToTasksButton);
            Content = stack;
        }

        async public void OnGoToTasksButtonClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(TasksPage.tasksMasterPage);
        }
    }
}
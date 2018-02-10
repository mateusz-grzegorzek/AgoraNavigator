using AgoraNavigator.Login;
using System;
using Xamarin.Forms;

namespace AgoraNavigator.Tasks
{
    public class GamePage : ContentPage
    {
        public static TasksMasterPage tasksMasterPage;
        public static Label totalPointsLabel;

        public GamePage()
        {
            tasksMasterPage = new TasksMasterPage();
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
            await Navigation.PushAsync(tasksMasterPage);
        }
    }
}
using AgoraNavigator.Login;
using AgoraNavigator.Menu;
using System;
using Xamarin.Forms;

namespace AgoraNavigator.Tasks
{
    public class TasksMasterPage : TabbedPage
    {
        public static TasksMasterView closedTasksView;
        public static TasksMasterView openedTasksView;

        public async void closeTask(GameTask task)
        {
            await DisplayAlert("Task", "Succes!", "Ok");
            GameTask.CloseTask(task.id);
            GamePage.totalPointsLabel.Text = "Total points: " + Users.loggedUser.TotalPoints.ToString();
            await MainPage.tasksPage.Navigation.PopAsync();
        }

        public TasksMasterPage()
        {
            Console.WriteLine("TasksMasterPage");
            Title = "Tasks";
            BackgroundColor = Color.AliceBlue;

            openedTasksView = new TasksMasterView(Users.loggedUser.openedTasks);
            openedTasksView.Title = "Open";
            this.Children.Add(openedTasksView);
            closedTasksView = new TasksMasterView(Users.loggedUser.closedTasks);
            closedTasksView.Title = "Closed";
            this.Children.Add(closedTasksView);
        }
    }
}
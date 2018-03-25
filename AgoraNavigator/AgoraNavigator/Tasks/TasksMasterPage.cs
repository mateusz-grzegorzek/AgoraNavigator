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
            GamePage.totalPoints.Text = Users.loggedUser.TotalPoints.ToString();
            await MainPage.tasksPage.Navigation.PopAsync();
        }

        public TasksMasterPage()
        {
            Console.WriteLine("TasksMasterPage");
            Title = "Tasks";
            BackgroundColor = AgoraColor.DarkBlue;
            BarTextColor = AgoraColor.Blue;

            openedTasksView = new TasksMasterView(Users.loggedUser.openedTasks);
            openedTasksView.Title = "OPEN";
            this.Children.Add(openedTasksView);
            closedTasksView = new TasksMasterView(Users.loggedUser.closedTasks);
            closedTasksView.Title = "CLOSED";
            this.Children.Add(closedTasksView);
        }
    }
}
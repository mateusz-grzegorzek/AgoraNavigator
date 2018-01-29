using AgoraNavigator.Login;
using AgoraNavigator.Menu;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using static AgoraNavigator.Tasks.GameTask;

namespace AgoraNavigator.Tasks
{
    public class TasksMasterPage : TabbedPage
    {
        static public TasksMasterView closedTasksView;
        static public TasksMasterView openedTasksView;

        public async void closeTask(GameTask task)
        {
            await DisplayAlert("Task", "Succes!", "Ok");
            await Users.closeTask(task);
            GamePage.totalPointsLabel.Text = "Total points: " + Users.loggedUser.TotalPoints.ToString();
            await MainPage.tasksPage.Navigation.PopAsync();
        }

        public TasksMasterPage()
        {
            Title = "Tasks";
            BackgroundColor = Color.AliceBlue;

            closedTasksView = new TasksMasterView(Users.loggedUser.closedTasks);
            closedTasksView.Title = "Closed";
            this.Children.Add(closedTasksView);
            openedTasksView = new TasksMasterView(Users.loggedUser.openedTasks);
            openedTasksView.Title = "Open";
            this.Children.Add(openedTasksView);
        }
    }
}
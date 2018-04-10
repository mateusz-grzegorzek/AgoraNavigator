using AgoraNavigator.Login;
using AgoraNavigator.Popup;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AgoraNavigator.Tasks
{
    public class TasksMasterPage : TabbedPage
    {
        public static TasksMasterView closedTasksView;
        public static TasksMasterView openedTasksView;

        public async Task closeTask(GameTask task)
        {
            if(GameTask.CloseTask(task.id))
            {
                GamePage.totalPoints.Text = Users.loggedUser.TotalPoints.ToString();
                String description = task.scorePoints + " point for you. " + "You have " + Users.loggedUser.TotalPoints
                            + " points totally. Check more tasks to get more points and win the competition!";
                SimplePopup popup = new SimplePopup("Good answer!", description, true);                
                await Navigation.PushPopupAsync(popup);
                await App.mainPage.Detail.Navigation.PopAsync();
            }    
        }

        public TasksMasterPage()
        {
            Console.WriteLine("TasksMasterPage");
            Title = "Tasks";
            BackgroundColor = AgoraColor.DarkBlue;
            BarTextColor = AgoraColor.Blue;

            openedTasksView = new TasksMasterView(Users.loggedUser.OpenedTasks, true);
            openedTasksView.Title = "OPEN";
            this.Children.Add(openedTasksView);
            closedTasksView = new TasksMasterView(Users.loggedUser.ClosedTasks, false);
            closedTasksView.Title = "CLOSED";
            this.Children.Add(closedTasksView);
        }
    }
}
using AgoraNavigator.Login;
using AgoraNavigator.Menu;
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
                SimplePopup popup = new SimplePopup("Good answer!", description)
                {
                    ColorBackground = Color.Green,
                    ColorBody = Color.White,
                    ColorTitle = Color.White,
                };
                popup.SetColors();
                popup.buttonOk.Clicked += async (object sender, EventArgs e) =>
                {
                    await PopupNavigation.RemovePageAsync(popup);
                    await App.mainPage.Detail.Navigation.PopAsync();
                };
                await Navigation.PushPopupAsync(popup);
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
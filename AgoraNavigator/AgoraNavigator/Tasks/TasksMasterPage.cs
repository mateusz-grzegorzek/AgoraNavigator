using System.Collections.ObjectModel;
using Xamarin.Forms;
using static AgoraNavigator.Tasks.GameTask;

namespace AgoraNavigator.Tasks
{
    public class TasksMasterPage : TabbedPage
    {
        static public ObservableCollection<GameTask> openedTasks;
        static public ObservableCollection<GameTask> closedTasks;
        static public TasksMasterView closedTasksView;
        static public TasksMasterView openedTasksView;

        public static void closeTask(GameTask task)
        {
            TasksPage.gamePage.addScorePoints(task.scorePoints);
            TasksMasterPage.openedTasks.Remove(task);
            closedTasks.Add(task);
            MainPage.tasksPage.Navigation.PopAsync();
        }

        public TasksMasterPage()
        {
            Title = "Tasks";
            BackgroundColor = Color.AliceBlue;

            closedTasks = new ObservableCollection<GameTask>();
            openedTasks = new ObservableCollection<GameTask>();
            openedTasks.Add(new GameTask
            {
                id = 1,
                title = "Adventurer quest",
                iconSource = "hamburger.png",
                description = "Find all beacons at gym",
                taskType = TaskType.Button,
                correctAnswer = null,
                scorePoints = 1,
                completed = false
            });
            openedTasks.Add(new GameTask
            {
                id = 2,
                title = "Gym history",
                iconSource = "hamburger.png",
                description = "Find how long (in km) is the river which name is the name of the gym",
                taskType = TaskType.Text,
                correctAnswer = "1047",
                scorePoints = 1,
                completed = false
            });

            closedTasksView = new TasksMasterView(closedTasks);
            closedTasksView.Title = "Closed";
            this.Children.Add(closedTasksView);
            openedTasksView = new TasksMasterView(openedTasks);
            openedTasksView.Title = "Open";
            this.Children.Add(openedTasksView);
        }
    }
}
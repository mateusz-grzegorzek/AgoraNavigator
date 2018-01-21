using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AgoraNavigator
{
    public enum TaskType
    {
        Text = 0,
        Button = 1
    }

    public class Task
    {
        public int id { get; set; }

        public string title { get; set; }

        public string iconSource { get; set; }

        public string description { get; set; }

        public TaskType taskType { get; set; }

        public string correctAnswer { get; set; }

        public int scorePoints { get; set; }

        public bool completed { get; set; }

        static public bool ProcessTask(Task task)
        {
            switch(task.id)
            {
                case 1:
                    Console.WriteLine("Processing task 1!");
                    break;
            }
            return true;
        }
    }

    public class TasksPage : NavigationPage
    {
        public static TasksMasterPage tasksMasterPage;
        public static GamePage gamePage;

        public TasksPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BarTextColor = Color.Red;
            BackgroundColor = Color.Azure;
            tasksMasterPage = new TasksMasterPage();
            gamePage = new GamePage();
            Navigation.PushAsync(gamePage);
        }
    }

    public class GamePage : ContentPage, INotifyPropertyChanged
    {
        public static int totalPoints = 0;
        Label totalPointsLabel;

        public void addScorePoints(int scorePoints)
        {
            totalPoints += scorePoints;
            totalPointsLabel.Text = "Total points: " + totalPoints.ToString();
        }

        public GamePage()
        {
            Title = "Game of Tasks";
            totalPointsLabel = new Label();
            addScorePoints(0);
            Button goToTasksButton = new Button { Text = "Go to Tasks!" };
            goToTasksButton.Clicked += OnGoToTasksButtonClick;
            var stack = new StackLayout { Spacing = 12 };
            stack.Children.Add(totalPointsLabel);
            stack.Children.Add(goToTasksButton);
            Content = stack;
        }

        public void OnGoToTasksButtonClick(object sender, EventArgs e)
        {
            Navigation.PushAsync(TasksPage.tasksMasterPage);
        }
    }

    public class TasksMasterPage : TabbedPage
    {
        static public ObservableCollection<Task> openedTasks;
        static public ObservableCollection<Task> closedTasks;
        static public TasksMasterView closedTasksView;
        static public TasksMasterView openedTasksView;
      
        public static void closeTask(Task task)
        {
            Console.WriteLine("TasksMasterPage:closeTask:Closing task with id=" + task.id);
            TasksPage.gamePage.addScorePoints(task.scorePoints);
            Console.WriteLine("TasksMasterPage:closeTask:GamePage.totalPoints=" + GamePage.totalPoints);
            Console.WriteLine("TasksMasterPage.openedTasks.Count=" + TasksMasterPage.openedTasks.Count);
            TasksMasterPage.openedTasks.Remove(task);   
            Console.WriteLine("TasksMasterPage.openedTasks.Count=" + TasksMasterPage.openedTasks.Count);
            closedTasks.Add(task);
            MainPage.tasksPage.Navigation.PopAsync();
        }

        public TasksMasterPage()
        {
            Title = "Tasks";
            BackgroundColor = Color.AliceBlue;

            closedTasks = new ObservableCollection<Task>();
            openedTasks = new ObservableCollection<Task>();
            openedTasks.Add(new Task
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
            openedTasks.Add(new Task
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

    public class TasksMasterView : ContentPage
    {
        ListView tasksListView;

        public TasksMasterView(ObservableCollection<Task> tasks)
        {
            tasksListView = new ListView
            {
                ItemsSource = tasks,
                ItemTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid { Padding = new Thickness(5, 10) };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                    var image = new Image();
                    image.SetBinding(Image.SourceProperty, "iconSource");
                    var label = new Label { VerticalOptions = LayoutOptions.FillAndExpand };
                    label.SetBinding(Label.TextProperty, "title");

                    grid.Children.Add(image);
                    grid.Children.Add(label, 1, 0);

                    return new ViewCell { View = grid };
                }),
                SeparatorVisibility = SeparatorVisibility.None
            };
            tasksListView.ItemTapped += OnTaskTitleClick;

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(tasksListView);
            Content = stack;
        }

        public void OnTaskTitleClick(object sender, ItemTappedEventArgs e)
        {
            Task task = (Task)e.Item;
            if (task.completed == false)
            {
                Navigation.PushAsync(new TaskDetailView(task));
            }
        }
    }

    public class TaskDetailView : ContentPage
    {
        Entry answerEntry;
        Task actualTask;

        public TaskDetailView(Task task)
        {
            BackgroundColor = Color.Aquamarine;
            actualTask = task;
            var stack = new StackLayout { Spacing = 0 };
            Label description = new Label { Text = task.description };
            stack.Children.Add(description);
            if (task.taskType == TaskType.Text)
            {
                answerEntry = new Entry { };
                stack.Children.Add(answerEntry);
            }
            Button answerButton = new Button { Text = "Send answer" };
            answerButton.Clicked += OnAnswerButtonClick;
            stack.Children.Add(answerButton);
            Content = stack;
        }

        public void OnAnswerButtonClick(object sender, EventArgs e)
        {
            switch(actualTask.taskType)
            {
                case TaskType.Text:
                    Console.WriteLine("answerEntry.Text=" + answerEntry.Text);
                    Console.WriteLine("actualTask.correctAnswer" + actualTask.correctAnswer);
                    //if (actualTask.correctAnswer == answerEntry.Text)
                    //{
                        Console.WriteLine("Yeah! Correct answer!");
                        TasksMasterPage.closeTask(actualTask);
                    //}
                    break;
                case TaskType.Button:
                    if(Task.ProcessTask(actualTask))
                    {
                        TasksMasterPage.closeTask(actualTask);
                    }
                    break;
                default:
                    Console.WriteLine("Error!");
                    break;
            }
        }
    }
}
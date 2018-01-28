using System;
using Xamarin.Forms;

namespace AgoraNavigator.Tasks
{ 
    public class TasksPage : NavigationPage
    {
        public static TasksMasterPage tasksMasterPage;
        public static GamePage gamePage;

        public TasksPage()
        {
            Console.WriteLine("TasksPage");
            NavigationPage.SetHasNavigationBar(this, false);
            BarTextColor = Color.Red;
            BackgroundColor = Color.Azure;
            tasksMasterPage = new TasksMasterPage();
            gamePage = new GamePage();
            Navigation.PushAsync(gamePage);
        }
    }
}
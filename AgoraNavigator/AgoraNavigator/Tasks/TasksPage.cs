using System;
using Xamarin.Forms;

namespace AgoraNavigator.Tasks
{ 
    public class TasksPage : NavigationPage
    {
        GamePage gamePage;

        public TasksPage()
        {
            Console.WriteLine("TasksPage");
            SetHasNavigationBar(this, false);
            BarTextColor = Color.Red;
            BackgroundColor = Color.Azure;
            gamePage = new GamePage();
            Console.WriteLine("Navigation.PushAsync(gameLoginPage)");
            Navigation.PushAsync(gamePage);
        }
    }
}
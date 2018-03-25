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
            BarBackgroundColor = AgoraColor.DarkBlue;
            BarTextColor = AgoraColor.Blue;
            gamePage = new GamePage();
            Console.WriteLine("Navigation.PushAsync(gameLoginPage)");
            Navigation.PushAsync(gamePage);
        }
    }
}
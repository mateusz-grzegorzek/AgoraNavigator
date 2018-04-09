using AgoraNavigator.Login;
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
            if (!Users.isUserLogged)
            {
                Console.WriteLine("TasksPage:User not logged in!");
                App.mainPage.ShowLoginScreen(typeof(TasksPage));
                return;
            }
            BarBackgroundColor = AgoraColor.DarkBlue;
            BarTextColor = AgoraColor.Blue;
            gamePage = new GamePage();
            Console.WriteLine("Navigation.PushAsync(gamePage)");
            Navigation.PushAsync(gamePage);
        }
    }
}
using AgoraNavigator.Login;
using AgoraNavigator.Menu;
using System;
using Xamarin.Forms;

namespace AgoraNavigator
{
    public class App : Application
    {
        public App()
        {
            Console.WriteLine("Application started!");
            MainPage = new StartingPage();
        }
    }
}


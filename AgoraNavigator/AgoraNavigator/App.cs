using AgoraNavigator.Login;
using AgoraNavigator.Menu;
using System;
using Xamarin.Forms;
//using Com.OneSignal;

namespace AgoraNavigator
{
    public class App : Application
    {
        public App()
        {
            Console.WriteLine("Application started!");
            MainPage = new StartingPage();
            //OneSignal.Current.StartInit("c4f53afa-e275-4d90-9a9c-82f94d5e40bc").EndInit();
        }
    }
}


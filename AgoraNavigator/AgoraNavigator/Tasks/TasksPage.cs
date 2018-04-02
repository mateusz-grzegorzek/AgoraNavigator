﻿using AgoraNavigator.Login;
using System;
using Xamarin.Forms;

namespace AgoraNavigator.Tasks
{ 
    public class TasksPage : NavigationPage
    {
        GamePage gamePage;

        public TasksPage()
        {
            if (!Users.isUserLogged)
            {
                Navigation.PushAsync(new GameLoginNavPage());
                return;
            }
            BarBackgroundColor = AgoraColor.DarkBlue;
            BarTextColor = AgoraColor.Blue;
            gamePage = new GamePage();
            Console.WriteLine("Navigation.PushAsync(gameLoginPage)");
            Navigation.PushAsync(gamePage);
        }
    }
}
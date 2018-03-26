using AgoraNavigator.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AgoraNavigator.Login
{
    class Antena
    {
        public int Id { get; set; }
        public List<User> members { get; set; }
    }

    public class UserTasks
    {
        public int TotalPoints { get; set; }
        public ObservableCollection<GameTask> OpenedTasks;
        public ObservableCollection<GameTask> ClosedTasks;
    }

    public class User
    {
        public int Id { get; set; }
        public int Pin { get; set; }
        public int AntenaId { get; set; }
        public UserTasks Tasks { get; set; }
    }

    class Users
    {
        public static User loggedUser;
        public static bool isUserLogged;

        public static void InitDefaultUser()
        {
            loggedUser = new User();
            loggedUser.Id = 1;
            loggedUser.Pin = 1234;
            loggedUser.AntenaId = 1;
            loggedUser.Tasks.TotalPoints = 0;
            loggedUser.Tasks.ClosedTasks = new ObservableCollection<GameTask>();
            loggedUser.Tasks.OpenedTasks = new ObservableCollection<GameTask>(GameTask.allTasks);
            isUserLogged = true;
        }

        public static void InitUserData(int userId, int pin, UserInfo userInfo)
        {
            loggedUser = new User();
            loggedUser.Id = userId;
            loggedUser.Pin = pin;
            loggedUser.AntenaId = userInfo.antenaId;
            loggedUser.Tasks = new UserTasks();
            try
            {
                UserTasks userTasks = JsonConvert.DeserializeObject<UserTasks>(userInfo.tasks);
                loggedUser.Tasks.TotalPoints = userInfo.totalPoints;
                loggedUser.Tasks.ClosedTasks = new ObservableCollection<GameTask>(userTasks.ClosedTasks);
                loggedUser.Tasks.OpenedTasks = new ObservableCollection<GameTask>(userTasks.OpenedTasks);
            }
            catch(Exception)
            {
                loggedUser.Tasks.TotalPoints = 0;
                loggedUser.Tasks.ClosedTasks = new ObservableCollection<GameTask>();
                loggedUser.Tasks.OpenedTasks = new ObservableCollection<GameTask>(GameTask.allTasks);
            }
            
            isUserLogged = true;
            Console.WriteLine("Users:InitUserData:loggedUser.Id=" + loggedUser.Id);
            Console.WriteLine("Users:InitUserData:loggedUser.AntenaId=" + loggedUser.AntenaId);
            Console.WriteLine("Users:InitUserData:loggedUser.TotalPoints=" + loggedUser.Tasks.TotalPoints);
            Console.WriteLine("Users:InitUserData:loggedUser.openedTasks.Count=" + loggedUser.Tasks.OpenedTasks.Count);
            Console.WriteLine("Users:InitUserData:loggedUser.closedTasks.Count=" + loggedUser.Tasks.ClosedTasks.Count);
        }
    }
}

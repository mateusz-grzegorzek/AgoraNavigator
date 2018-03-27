using AgoraNavigator.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

    public class UserTasksInDb
    {
        public int totalPoints { get; set; }
        public List<int> closedTasks;   
    }

    public class User
    {
        public int Id { get; set; }
        public int Pin { get; set; }
        public int AntenaId { get; set; }
        public int TotalPoints { get; set; }
        public ObservableCollection<GameTask> OpenedTasks;
        public ObservableCollection<GameTask> ClosedTasks;
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
            loggedUser.TotalPoints = 0;
            loggedUser.ClosedTasks = new ObservableCollection<GameTask>();
            loggedUser.OpenedTasks = new ObservableCollection<GameTask>(GameTask.allTasks);
            isUserLogged = true;
        }

        public static void InitUserData(int userId, int pin, JObject userInfo)
        {
            loggedUser = new User();
            loggedUser.Id = userId;
            loggedUser.Pin = pin;
            loggedUser.AntenaId = int.Parse(userInfo["antenaId"].ToString());

            try
            {
                JToken tasks = userInfo["tasks"];
                List<int> closedTasks = tasks["closedTasks"].ToObject<List<int>>();
                loggedUser.TotalPoints = int.Parse(tasks["totalPoints"].ToString());
                loggedUser.ClosedTasks = new ObservableCollection<GameTask>();
                loggedUser.OpenedTasks = new ObservableCollection<GameTask>();
                foreach (GameTask task in GameTask.allTasks)
                {
                    if(closedTasks.Contains(task.id))
                    {
                        task.taskStatus = GameTask.TaskStatus.Completed;
                        loggedUser.ClosedTasks.Add(task);
                    }
                    else
                    {
                        loggedUser.OpenedTasks.Add(task);
                    }
                }
            }
            catch(Exception)
            {
                loggedUser.TotalPoints = 0;
                loggedUser.ClosedTasks = new ObservableCollection<GameTask>();
                loggedUser.OpenedTasks = new ObservableCollection<GameTask>(GameTask.allTasks);
            }
            
            isUserLogged = true;
            Console.WriteLine("Users:InitUserData:loggedUser.Id=" + loggedUser.Id);
            Console.WriteLine("Users:InitUserData:loggedUser.AntenaId=" + loggedUser.AntenaId);
            Console.WriteLine("Users:InitUserData:loggedUser.TotalPoints=" + loggedUser.TotalPoints);
            Console.WriteLine("Users:InitUserData:loggedUser.openedTasks.Count=" + loggedUser.OpenedTasks.Count);
            Console.WriteLine("Users:InitUserData:loggedUser.closedTasks.Count=" + loggedUser.ClosedTasks.Count);
        }
    }
}

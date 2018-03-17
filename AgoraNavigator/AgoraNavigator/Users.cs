using AgoraNavigator.Tasks;
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

    public class User
    {
        public String firebaseToken;
        public int Id { get; set; }
        public int AntenaId { get; set; }
        public int TotalPoints { get; set; }
        public ObservableCollection<GameTask> openedTasks;
        public ObservableCollection<GameTask> closedTasks;
    }

    class Users
    {
        public static User loggedUser;
        public static bool isUserLogged;

        public static void InitUserData(IDictionary<string, object> userInfo)
        {
            loggedUser = new User();
            loggedUser.Id = Convert.ToInt32(userInfo["userId"]);
            loggedUser.AntenaId = Convert.ToInt32(userInfo["antenaId"]);
            loggedUser.firebaseToken = FirebaseMessagingClient.firebaseToken;
            loggedUser.TotalPoints = Convert.ToInt32(userInfo["totalPoints"]);
            loggedUser.closedTasks = new ObservableCollection<GameTask>();
            loggedUser.openedTasks = new ObservableCollection<GameTask>();
            JArray closedTasks = (JArray)userInfo["closedTasks"];
            foreach (GameTask task in GameTask.allTasks)
            {
                bool closed = false;
                foreach (JToken token in closedTasks)
                {
                    try
                    {
                        if (Convert.ToUInt32(token) == task.id)
                        {
                            closed = true;
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    
                }
                if (closed)
                {
                    loggedUser.closedTasks.Add(task);
                }
                else
                {
                    loggedUser.openedTasks.Add(task);
                }
            }

            isUserLogged = true;
            Console.WriteLine("Users:InitUserData:loggedUser.Id=" + loggedUser.Id);
            Console.WriteLine("Users:InitUserData:loggedUser.AntenaId=" + loggedUser.AntenaId);
            Console.WriteLine("Users:InitUserData:loggedUser.TotalPoints=" + loggedUser.TotalPoints);
            Console.WriteLine("Users:InitUserData:loggedUser.openedTasks.Count=" + loggedUser.openedTasks.Count);
            Console.WriteLine("Users:InitUserData:loggedUser.closedTasks.Count=" + loggedUser.closedTasks.Count);
        }
    }
}

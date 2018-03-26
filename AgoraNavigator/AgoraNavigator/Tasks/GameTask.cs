using AgoraNavigator.Login;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AgoraNavigator.Tasks
{
    public class GameTask
    {
        public static List<GameTask> allTasks;

        public enum TaskType
        {
            Text = 0,
            Button = 1
        }

        public int id { get; set; }

        public string title { get; set; }

        public string iconSource { get; set; }

        public string description { get; set; }

        public TaskType taskType { get; set; }

        public string correctAnswer { get; set; }

        public int scorePoints { get; set; }

        public bool completed { get; set; }

        public bool needBluetoothAndLocation { get; set; }

        public string dbName { get; set; }

        public static async Task<bool> ProcessTask(GameTask task)
        {
            String tasksPath = "tasks/";
            Console.WriteLine("GameTask:ProcessTask:task.id=" + task.id);
            bool result = false;
            String databasePath;
            switch (task.title)
            {
                case "Adventurer quest":
                    result = await Beacons.ScanForBeacon(Beacons.beaconFHNJ);
                    break;
                case "AEGEE Army":
                    result = await Beacons.ScanForBeacon(Beacons.beaconFHNJ);
                    if(result)
                    {
                        CrossFirebasePushNotification.Current.Subscribe("AEGEE_Army_" + Users.loggedUser.AntenaId);
                        databasePath = tasksPath + task.dbName + "/Active/" + Users.loggedUser.AntenaId + "/" + Users.loggedUser.Id;
                        if(FirebaseMessagingClient.SendMessage(databasePath, "1"))
                        {
                            DependencyService.Get<INotification>().Notify("Task state", "Great! Now wait for your friends!");
                        }
                        else
                        {
                            DependencyService.Get<INotification>().Notify("No internet connection", "You need internet connection to complete this task!");
                        }
                        result = false;
                    }
                    break;
                case "Plenary photo":
                case "Redbull give you the wings":
                case "Selfie with friends!":
                    databasePath = tasksPath + task.dbName + "/" + Users.loggedUser.Id;
                    bool succes = await FirebaseMessagingClient.SendSingleQuery<bool>(databasePath);
                    if(succes)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        public static bool CloseTask(int taskId)
        {
            bool result = false;
            GameTask task = allTasks[taskId];
            Console.WriteLine("Users:closeTask:task.id=" + task.id);
            String databasePath = "/users/" + Users.loggedUser.Id + "/closedTasks/" + taskId;
            if(FirebaseMessagingClient.SendMessage(databasePath, taskId.ToString()))
            {
                databasePath = "/users/" + Users.loggedUser.Id + "/totalPoints/";
                String totalPoints = (Users.loggedUser.TotalPoints + task.scorePoints).ToString();
                if (FirebaseMessagingClient.SendMessage(databasePath, totalPoints))
                {
                    task.completed = true;
                    Users.loggedUser.TotalPoints += task.scorePoints;
                    Users.loggedUser.openedTasks.Remove(task);
                    Users.loggedUser.closedTasks.Add(task);
                    result = true;
                }
            }
            Console.WriteLine("Users:closeTask:loggedUser.TotalPoints=" + Users.loggedUser.TotalPoints);
            Console.WriteLine("Users:closeTask:loggedUser.openedTasks.Count=" + Users.loggedUser.openedTasks.Count);
            Console.WriteLine("Users:closeTask:loggedUser.closedTasks.Count=" + Users.loggedUser.closedTasks.Count);
            return result;
        }

        public static void AddTasks()
        {
            allTasks = new List<GameTask>();
            allTasks.Add(new GameTask
            {
                id = 0,
                title = "Adventurer quest",
                description = "Find beacon at gym",
                taskType = TaskType.Button,
                correctAnswer = null,
                scorePoints = 1,
                completed = false,
                needBluetoothAndLocation = true,
            });
            allTasks.Add(new GameTask
            {
                id = 1,
                title = "Gym history",
                description = "Find how long (in km) is the river which name is the name of the gym",
                taskType = TaskType.Text,
                correctAnswer = "1047",
                scorePoints = 1,
                completed = false,
                needBluetoothAndLocation = false
            });
            allTasks.Add(new GameTask
            {
                id = 2,
                title = "AEGEE Army",
                description = "Gather more than half of your antena members near beacon at gym",
                taskType = TaskType.Button,
                correctAnswer = null,
                scorePoints = 3,
                completed = false,
                needBluetoothAndLocation = true,
                dbName = "AEGEE_Army"
            });
            allTasks.Add(new GameTask
            {
                id = 3,
                title = "Nearby places",
                description = "Find in which year was founded university across the street from the gym",
                taskType = TaskType.Text,
                correctAnswer = "1919",
                scorePoints = 1,
                completed = false,
                needBluetoothAndLocation = false
            });
            allTasks.Add(new GameTask
            {
                id = 4,
                title = "More than one animal is?",
                description = "Ask Agora organizer what is the name of the AEGEE-Cracow sheep",
                taskType = TaskType.Text,
                correctAnswer = "Mateusz",
                scorePoints = 2,
                completed = false,
                needBluetoothAndLocation = false
            });
            allTasks.Add(new GameTask
            {
                id = 5,
                title = "Plenary photo",
                description = "Take a photo on plenary and send on Instagram with #",
                taskType = TaskType.Button,
                correctAnswer = null,
                scorePoints = 2,
                completed = false,
                needBluetoothAndLocation = false,
                dbName = "Plenary_photo"
            });
            allTasks.Add(new GameTask
            {
                id = 6,
                title = "Redbull give you the wings",
                description = "Take a photo with redbull and send on Instagram with #",
                taskType = TaskType.Button,
                correctAnswer = null,
                scorePoints = 2,
                completed = false,
                needBluetoothAndLocation = false,
                dbName = "Redbull"
            });
            allTasks.Add(new GameTask
            {
                id = 7,
                title = "The first are the best",
                description = "Be the one of first persons on morning plenary",
                taskType = TaskType.Button,
                correctAnswer = null,
                scorePoints = 1,
                completed = false,
                needBluetoothAndLocation = true
            });
            allTasks.Add(new GameTask
            {
                id = 8,
                title = "Gunnar",
                description = "Ask Gunnar for task",
                taskType = TaskType.Text,
                correctAnswer = "GUNNAR",
                scorePoints = 2,
                completed = false,
                needBluetoothAndLocation = false
            });
            allTasks.Add(new GameTask
            {
                id = 9,
                title = "Randevu",
                description = "Talk with AEGEE-Cracow president and ask him for secret password",
                taskType = TaskType.Text,
                correctAnswer = "Buka",
                scorePoints = 2,
                completed = false,
                needBluetoothAndLocation = false
            });
            allTasks.Add(new GameTask
            {
                id = 10,
                title = "Women WC",
                description = "Enter code from Women's WC",
                taskType = TaskType.Text,
                correctAnswer = "Closed",
                scorePoints = 2,
                completed = false,
                needBluetoothAndLocation = false
            });
            allTasks.Add(new GameTask
            {
                id = 11,
                title = "Plenary code",
                description = "Enter code from Plenary",
                taskType = TaskType.Text,
                correctAnswer = "Code",
                scorePoints = 2,
                completed = false,
                needBluetoothAndLocation = false
            });
            allTasks.Add(new GameTask
            {
                id = 12,
                title = "Polish favourite program",
                description = "Find out what Polish people watch on sunday at dinner time",
                taskType = TaskType.Text,
                correctAnswer = "Familiada",
                scorePoints = 2,
                completed = false,
                needBluetoothAndLocation = false
            });
            allTasks.Add(new GameTask
            {
                id = 13,
                title = "Selfie with friends!",
                description = "Take a photo with your friends, send it on Facebook participants group and tag your friends",
                taskType = TaskType.Button,
                scorePoints = 2,
                completed = false,
                needBluetoothAndLocation = false,
                dbName = "Selfie"
            });
        }
    }
}

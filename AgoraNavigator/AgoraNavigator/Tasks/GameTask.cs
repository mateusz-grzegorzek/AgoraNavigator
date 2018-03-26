using AgoraNavigator.Login;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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

        public enum TaskStatus
        {
            NotStarted = 0,
            Checking = 1,
            Processing = 2,
            Completed = 3
        }

        public int id { get; set; }

        public string title { get; set; }

        public string iconSource { get; set; }

        public string description { get; set; }

        public TaskType taskType { get; set; }

        public string correctAnswer { get; set; }

        public int scorePoints { get; set; }

        public TaskStatus taskStatus { get; set; }

        public bool needBluetoothAndLocation { get; set; }

        public string dbName { get; set; }

        public static bool CloseTask(int taskId)
        {
            bool result = false;
            GameTask task = allTasks[taskId];
            Console.WriteLine("Users:closeTask:task.id=" + task.id);
            String databasePath = "/users/" + Users.loggedUser.Id + "/" + Users.loggedUser.Pin + "/tasks/";
            String tasks = JsonConvert.SerializeObject(Users.loggedUser.Tasks);
            if(FirebaseMessagingClient.SendMessage(databasePath, tasks))
            {
                task.taskStatus = TaskStatus.Completed;
                Users.loggedUser.Tasks.TotalPoints += task.scorePoints;
                Users.loggedUser.Tasks.OpenedTasks.Remove(task);
                Users.loggedUser.Tasks.ClosedTasks.Add(task);
                result = true;
            }
            Console.WriteLine("Users:closeTask:loggedUser.TotalPoints=" + Users.loggedUser.Tasks.TotalPoints);
            Console.WriteLine("Users:closeTask:loggedUser.openedTasks.Count=" + Users.loggedUser.Tasks.OpenedTasks.Count);
            Console.WriteLine("Users:closeTask:loggedUser.closedTasks.Count=" + Users.loggedUser.Tasks.ClosedTasks.Count);
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
                taskStatus = TaskStatus.NotStarted,
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
                taskStatus = TaskStatus.NotStarted,
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
                taskStatus = TaskStatus.NotStarted,
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
                taskStatus = TaskStatus.NotStarted,
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
                taskStatus = TaskStatus.NotStarted,
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
                taskStatus = TaskStatus.NotStarted,
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
                taskStatus = TaskStatus.NotStarted,
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
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = true,
                dbName = "First_Come_First_Served"
            });
            allTasks.Add(new GameTask
            {
                id = 8,
                title = "Gunnar",
                description = "Ask Gunnar for task",
                taskType = TaskType.Text,
                correctAnswer = "GUNNAR",
                scorePoints = 2,
                taskStatus = TaskStatus.NotStarted,
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
                taskStatus = TaskStatus.NotStarted,
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
                taskStatus = TaskStatus.NotStarted,
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
                taskStatus = TaskStatus.NotStarted,
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
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false
            });
            allTasks.Add(new GameTask
            {
                id = 13,
                title = "Selfie with friends!",
                description = "Take a photo with your friends, send it on Facebook participants group and tag your friends",
                taskType = TaskType.Button,
                scorePoints = 2,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                dbName = "Selfie"
            });
            allTasks.Add(new GameTask
            {
                id = 13,
                title = "I need a dollar dollar...",
                description = "Lend from AEGEEans five different currency in banknote or coin and show them to local organizer",
                taskType = TaskType.Button,
                scorePoints = 3,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                dbName = "Dollar"
            });
        }
    }
}

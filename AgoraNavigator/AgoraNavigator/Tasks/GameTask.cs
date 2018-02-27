using AgoraNavigator.Login;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AgoraNavigator.Tasks
{
    public class GameTask
    {
        public enum TaskType
        {
            Text = 0,
            Button = 1,
            PreBLE = 2
        }

        public int ownerId { get; set; }

        public int id { get; set; }

        public string title { get; set; }

        public string iconSource { get; set; }

        public string description { get; set; }

        public TaskType taskType { get; set; }

        public string correctAnswer { get; set; }

        public int scorePoints { get; set; }

        public bool completed { get; set; }

        public bool needBluetoothAndLocation { get; set; }

        public static async Task<bool> ProcessTask(GameTask task)
        {
            Console.WriteLine("GameTask:ProcessTask:task.id=" + task.id);
            bool result = false;
            switch (task.id)
            {
                case 1:
                    result = await Beacons.ScanForBeacon(Beacons.beaconFHNJ);
                    break;
                case 3:
                    result = await Beacons.ScanForBeacon(Beacons.beaconFHNJ);
                    if(result)
                    {
                        task.ownerId = Users.loggedUser.Id;
                        String msg = JsonConvert.SerializeObject(task);
                        Console.WriteLine("GameTask:ProcessTask:resp=");
                    }
                    break;
            }
            return result;
        }

        public static void AddTasks()
        {
            Users.loggedUser.openedTasks.Add(new GameTask
            {
                id = 1,
                title = "Adventurer quest",
                iconSource = "hamburger.png",
                description = "Find beacon at gym",
                taskType = TaskType.PreBLE,
                correctAnswer = null,
                scorePoints = 1,
                completed = false,
                needBluetoothAndLocation = true
            });
            Users.loggedUser.openedTasks.Add(new GameTask
            {
                id = 2,
                title = "Gym history",
                iconSource = "hamburger.png",
                description = "Find how long (in km) is the river which name is the name of the gym",
                taskType = TaskType.Text,
                correctAnswer = "1047",
                scorePoints = 1,
                completed = false,
                needBluetoothAndLocation = false
            });
            Users.loggedUser.openedTasks.Add(new GameTask
            {
                id = 3,
                title = "AEGEE Army",
                iconSource = "hamburger.png",
                description = "Gather more than half of your antena members near beacon at gym",
                taskType = TaskType.PreBLE,
                correctAnswer = null,
                scorePoints = 3,
                completed = false,
                needBluetoothAndLocation = true
            });
            Users.loggedUser.openedTasks.Add(new GameTask
            {
                id = 4,
                title = "Nearby places",
                iconSource = "hamburger.png",
                description = "Find in which year was founded university across the street from the gym",
                taskType = TaskType.Text,
                correctAnswer = "1919",
                scorePoints = 1,
                completed = false,
                needBluetoothAndLocation = false
            });
            Users.loggedUser.openedTasks.Add(new GameTask
            {
                id = 5,
                title = "More than one animal is?",
                iconSource = "hamburger.png",
                description = "Ask Agora organizer what is the name of the AEGEE-Cracow sheep",
                taskType = TaskType.Text,
                correctAnswer = "Mateusz",
                scorePoints = 2,
                completed = false,
                needBluetoothAndLocation = false
            });
            Users.loggedUser.openedTasks.Add(new GameTask
            {
                id = 6,
                title = "Plenary photo",
                iconSource = "hamburger.png",
                description = "Take a photo on plenary and send on Instagram with #",
                taskType = TaskType.Button,
                correctAnswer = null,
                scorePoints = 2,
                completed = false,
                needBluetoothAndLocation = false
            });
            Users.loggedUser.openedTasks.Add(new GameTask
            {
                id = 7,
                title = "Redbull give you the wings",
                iconSource = "hamburger.png",
                description = "Take a photo with redbull and send on Instagram with #",
                taskType = TaskType.Button,
                correctAnswer = null,
                scorePoints = 2,
                completed = false,
                needBluetoothAndLocation = false
            });
            Users.loggedUser.openedTasks.Add(new GameTask
            {
                id = 8,
                title = "The first are the best",
                iconSource = "hamburger.png",
                description = "Be the one of first persons on morning plenary",
                taskType = TaskType.PreBLE,
                correctAnswer = null,
                scorePoints = 1,
                completed = false,
                needBluetoothAndLocation = true
            });
            Users.loggedUser.openedTasks.Add(new GameTask
            {
                id = 9,
                title = "Gunnar",
                iconSource = "hamburger.png",
                description = "Ask Gunnar for task",
                taskType = TaskType.Text,
                correctAnswer = "GUNNAR",
                scorePoints = 2,
                completed = false,
                needBluetoothAndLocation = false
            });
            Users.loggedUser.openedTasks.Add(new GameTask
            {
                id = 10,
                title = "Randevu",
                iconSource = "hamburger.png",
                description = "Talk with AEGEE-Cracow president and ask him for secret password",
                taskType = TaskType.Text,
                correctAnswer = "Buka",
                scorePoints = 2,
                completed = false,
                needBluetoothAndLocation = false
            });
            Users.loggedUser.openedTasks.Add(new GameTask
            {
                id = 11,
                title = "Women WC",
                iconSource = "hamburger.png",
                description = "Enter code from Women's WC",
                taskType = TaskType.Text,
                correctAnswer = "Closed",
                scorePoints = 2,
                completed = false,
                needBluetoothAndLocation = false
            });
            Users.loggedUser.openedTasks.Add(new GameTask
            {
                id = 12,
                title = "Plenary code",
                iconSource = "hamburger.png",
                description = "Enter code from Plenary",
                taskType = TaskType.Text,
                correctAnswer = "Code",
                scorePoints = 2,
                completed = false,
                needBluetoothAndLocation = false
            });
            Users.loggedUser.openedTasks.Add(new GameTask
            {
                id = 13,
                title = "Polish favourite program",
                iconSource = "hamburger.png",
                description = "Find out what Polish people watch on sunday at dinner time",
                taskType = TaskType.Text,
                correctAnswer = "Familiada",
                scorePoints = 2,
                completed = false,
                needBluetoothAndLocation = false
            });
        }
    }
}

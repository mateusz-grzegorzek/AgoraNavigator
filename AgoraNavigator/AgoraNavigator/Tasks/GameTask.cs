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
                        AgoraTcpClient.ClientSend(msg);
                        String resp = AgoraTcpClient.ClientReceive();
                        Console.WriteLine("GameTask:ProcessTask:resp=" + resp);
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
        }
    }
}

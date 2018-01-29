using AgoraNavigator.Login;
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

        public int id { get; set; }

        public string title { get; set; }

        public string iconSource { get; set; }

        public string description { get; set; }

        public TaskType taskType { get; set; }

        public string correctAnswer { get; set; }

        public int scorePoints { get; set; }

        public bool completed { get; set; }

        public bool needBluetoothAndLocation { get; set; }

        static public async Task<bool> ProcessTask(GameTask task)
        {
            Console.WriteLine("ProcessTask:task.id=" + task.id);
            bool result = false;
            switch (task.id)
            {
                case 1:
                    result = await Beacons.ScanForBeacon(Beacons.beaconFHNJ);
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
                description = "Find all beacons at gym",
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
        }
    }
}

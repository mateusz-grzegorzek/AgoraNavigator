using System;
using System.Threading.Tasks;

namespace AgoraNavigator.Tasks
{
    public class GameTask
    {
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

        static public async Task<bool> ProcessTask(GameTask task)
        {
            bool result = false;
            switch (task.id)
            {
                case 1:
                    Console.WriteLine("Processing task 1!");
                    result = await Beacons.ScanForBeacon(Beacons.beaconFHNJ);
                    break;
            }
            return result;
        }
    }
}

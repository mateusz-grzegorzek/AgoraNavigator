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
            Button = 1,
            LongText = 2
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

        public List<string> correctAnswers { get; set; }

        public int minimumCorrectAnswers { get; set; }

        public int scorePoints { get; set; }

        public TaskStatus taskStatus { get; set; }

        public bool needBluetoothAndLocation { get; set; }

        public string dbName { get; set; }

        public static bool CloseTask(int taskId)
        {
            bool result = false;
            GameTask closedTask = allTasks[taskId];
            Console.WriteLine("GameTask:closeTask:task.id=" + closedTask.id);
            String idDbPath = Users.loggedUser.Id.ToString().PadLeft(4, '0');
            String pinDbPath = Users.loggedUser.Pin.ToString().PadLeft(4, '0');
            String databasePath = "/users/" + idDbPath + "/" + pinDbPath + "/tasks/";
            UserTasksInDb userTasksInDb = new UserTasksInDb();
            userTasksInDb.totalPoints = Users.loggedUser.TotalPoints + closedTask.scorePoints;
            userTasksInDb.closedTasks = new List<int>();
            foreach (GameTask task in Users.loggedUser.ClosedTasks)
            {
                userTasksInDb.closedTasks.Add(task.id);
            }
            userTasksInDb.closedTasks.Add(taskId);
            String tasks = JsonConvert.SerializeObject(userTasksInDb);
            if(FirebaseMessagingClient.SendMessage(databasePath, tasks))
            {
                closedTask.taskStatus = TaskStatus.Completed;
                Users.loggedUser.TotalPoints += closedTask.scorePoints;
                Users.loggedUser.OpenedTasks.Remove(closedTask);
                Users.loggedUser.ClosedTasks.Add(closedTask);
                result = true;
            }
            Console.WriteLine("GameTask:closeTask:loggedUser.TotalPoints=" + Users.loggedUser.TotalPoints);
            Console.WriteLine("GameTask:closeTask:loggedUser.openedTasks.Count=" + Users.loggedUser.OpenedTasks.Count);
            Console.WriteLine("GameTask:closeTask:loggedUser.closedTasks.Count=" + Users.loggedUser.ClosedTasks.Count);
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
                correctAnswer = "mateusz",
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
                correctAnswer = "buka",
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
                correctAnswer = "closed",
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
                correctAnswer = "code",
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
                correctAnswer = "familiada",
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
                id = 14,
                title = "I need a dollar dollar...",
                description = "Lend from AEGEEans five different currency in banknote or coin and show them to local organizer",
                taskType = TaskType.Button,
                scorePoints = 3,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                dbName = "Dollar"
            });
            allTasks.Add(new GameTask
            {
                id = 15,
                title = "Gunnar Quiz - question #1",
                description = "How many times did AEGEE-Kraków sign the CdA?",
                taskType = TaskType.LongText,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswers = new List<string>
                {
                    "2",
                    "twice",
                    "two"
                },
                minimumCorrectAnswers = 1
            });
            allTasks.Add(new GameTask
            {
                id = 16,
                title = "Gunnar Quiz - question #2",
                description = "When and where did the last Agora in Poland take place?",
                taskType = TaskType.LongText,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswers = new List<string>
                {
                    "2006",
                    "warsaw"
                },
                minimumCorrectAnswers = 2
            });
            allTasks.Add(new GameTask
            {
                id = 17,
                title = "Gunnar Quiz - question #3",
                description = "How many Agoras took place so far, including this one?",
                taskType = TaskType.Text,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswer = "65"
            });
            allTasks.Add(new GameTask
            {
                id = 18,
                title = "Gunnar Quiz - question #4",
                description = "Which antenna has been the only one to organise four Agoras?",
                taskType = TaskType.Text,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswer = "aegee-enschede"
            });
            allTasks.Add(new GameTask
            {
                id = 19,
                title = "Gunnar Quiz - question #5",
                description = "How many AEGEE members applied for the Agora?",
                taskType = TaskType.Text,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswer = "1207"
            });
            allTasks.Add(new GameTask
            {
                id = 20,
                title = "Gunnar Quiz - question #6",
                description = "How many members had the CDs maximum until 1996?",
                taskType = TaskType.Text,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswer = "16"
            });
            allTasks.Add(new GameTask
            {
                id = 21,
                title = "Gunnar Quiz - question #7",
                description = "How many antennae and contact antennae ever existed?",
                taskType = TaskType.Text,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswer = "400"
            });
            allTasks.Add(new GameTask
            {
                id = 22,
                title = "Gunnar Quiz - question #8",
                description = "What is the mascot of AEGEE-Aachen?",
                taskType = TaskType.Text,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswer = "polar bear"
            });
            allTasks.Add(new GameTask
            {
                id = 23,
                title = "Gunnar Quiz - question #9",
                description = "What was the name of the project that used to present funny films at the Agora?",
                taskType = TaskType.Text,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswer = "aegee.tv"
            });
            allTasks.Add(new GameTask
            {
                id = 24,
                title = "Gunnar Quiz - question #10",
                description = "What did participants get during Agora Zaragoza 2013?",
                taskType = TaskType.LongText,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswers = new List<string>
                {
                    "lindt",
                    "chocolate"
                },
                minimumCorrectAnswers = 2
            });
            allTasks.Add(new GameTask
            {
                id = 25,
                title = "Gunnar Quiz - question #11",
                description = "What did AEGEE vote in favour of introducing at Agora Poznan 1999?",
                taskType = TaskType.Text,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswer = "enlightened despotism"
            });
            allTasks.Add(new GameTask
            {
                id = 26,
                title = "Gunnar Quiz - question #12",
                description = "Which AEGEE-Europe member married his Secretary?",
                taskType = TaskType.Text,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswer = "christoph strohm"
            });
            allTasks.Add(new GameTask
            {
                id = 27,
                title = "Gunnar Quiz - question #13",
                description = "What was Agora Montpellier 1994 famous for?",
                taskType = TaskType.LongText,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswers = new List<string>
                {
                    "elected",
                    "president",
                    "outside"
                },
                minimumCorrectAnswers = 2
            });
            allTasks.Add(new GameTask
            {
                id = 28,
                title = "Gunnar Quiz - question #14",
                description = "Which drastic life change happened to former AEGEE-Europe President Georg von der Gablentz?",
                taskType = TaskType.LongText,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswers = new List<string>
                {
                    "changed",
                    "gender",
                    "woman"
                },
                minimumCorrectAnswers = 2
            });
            allTasks.Add(new GameTask
            {
                id = 29,
                title = "Gunnar Quiz - question #15",
                description = "What was the third AEGEE-Europe office originally?",
                taskType = TaskType.LongText,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswers = new List<string>
                {
                    "museum",
                    "underwear"
                },
                minimumCorrectAnswers = 2
            });
            allTasks.Add(new GameTask
            {
                id = 30,
                title = "Gunnar Quiz - question #16",
                description = "Which animals lived in the backyard of that office?",
                taskType = TaskType.Text,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswer = "chickens"
            });
            allTasks.Add(new GameTask
            {
                id = 31,
                title = "Gunnar Quiz - question #17",
                description = "Which AEGEE antenna has its own pub?",
                taskType = TaskType.Text,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswer = "aegee-enschede"
            });
            allTasks.Add(new GameTask
            {
                id = 32,
                title = "Gunnar Quiz - question #18",
                description = "How old was the oldest President of AEGEE - Europe when he was elected?",
                taskType = TaskType.LongText,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswers = new List<string>
                {
                    "30"
                },
                minimumCorrectAnswers = 1
            });
            allTasks.Add(new GameTask
            {
                id = 33,
                title = "Gunnar Quiz - question #19",
                description = "Which Working Group moved a lot of people when it was founded in 2001?",
                taskType = TaskType.LongText,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswers = new List<string>
                {
                    "dance"
                },
                minimumCorrectAnswers = 1
            });
            allTasks.Add(new GameTask
            {
                id = 34,
                title = "Gunnar Quiz - question #20",
                description = "How did AEGEE-Bucuresti transport 500 Agora participants to their accommodation?",
                taskType = TaskType.LongText,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswers = new List<string>
                {
                    "train",
                    "constanta"
                },
                minimumCorrectAnswers = 2
            });
            allTasks.Add(new GameTask
            {
                id = 35,
                title = "Gunnar Quiz - question #21",
                description = "Where were the participants of Agora Izmir lodged in 2005?",
                taskType = TaskType.LongText,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswers = new List<string>
                {
                    "five",
                    "hotel"
                },
                minimumCorrectAnswers = 2
            });
            allTasks.Add(new GameTask
            {
                id = 36,
                title = "Gunnar Quiz - question #22",
                description = "Where were AEGEE-Leiden's Agora participants lodged in 2010?",
                taskType = TaskType.LongText,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswers = new List<string>
                {
                    "outside",
                    "tents"
                },
                minimumCorrectAnswers = 2
            });
            allTasks.Add(new GameTask
            {
                id = 37,
                title = "Gunnar Quiz - question #23",
                description = "What happened in the garage next to the Agora gym in Warsaw in 2006?",
                taskType = TaskType.LongText,
                scorePoints = 1,
                taskStatus = TaskStatus.NotStarted,
                needBluetoothAndLocation = false,
                correctAnswers = new List<string>
                {
                    "sex",
                    "camera"
                },
                minimumCorrectAnswers = 2
            });
        }
    }
}

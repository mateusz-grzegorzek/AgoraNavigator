using AgoraNavigator.Login;
using AgoraNavigator.Popup;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using static AgoraNavigator.Tasks.GameTask;

namespace AgoraNavigator.Tasks
{
    public class TaskDetailView : ContentPage
    {
        Entry answerEntry;
        GameTask actualTask;
        String tasksPath = "tasks/";
        Button answerButton;
        bool isAnswerButtonClicked = false;

        public TaskDetailView(GameTask task)
        {
            Title = "Task details";
            BackgroundColor = AgoraColor.DarkBlue;
            actualTask = task;

            StackLayout layout = new StackLayout
            {
                Margin = new Thickness(0, 0),
                BackgroundColor = AgoraColor.Blue
            };

            StackLayout layoutTop = new StackLayout
            {
                Margin = new Thickness(20, 20),
                BackgroundColor = AgoraColor.Blue
            };
            layout.Children.Add(layoutTop);

            Label titleLabel = new Label
            {
                Text = task.title,
                TextColor = AgoraColor.DarkBlue,
                BackgroundColor = AgoraColor.Blue,
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            };
            Label descriptionLabel = new Label
            {
                Text = task.description,
                TextColor = Color.White,
                BackgroundColor = AgoraColor.Blue,
                FontFamily = AgoraFonts.GetPoppinsMedium(),
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
            };
            layoutTop.Children.Add(titleLabel);
            layoutTop.Children.Add(descriptionLabel);

            Grid gridBottom = new Grid
            {
                Margin = new Thickness(10, 30),
            };

            gridBottom.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridBottom.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) });
            gridBottom.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            gridBottom.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            gridBottom.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            gridBottom.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });

            answerButton = new Button
            {
                BackgroundColor = AgoraColor.Blue,
                TextColor = AgoraColor.DarkBlue,
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };
            answerButton.Clicked += OnAnswerButtonClick;
            if ((task.taskType == TaskType.Text) || (task.taskType == TaskType.LongText))
            {
                answerButton.Text = "SEND ANSWER";
                Label answerLabel = new Label
                {
                    Text = "Put your answer below:",
                    TextColor = Color.White,
                    FontFamily = AgoraFonts.GetPoppinsMedium(),
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    HorizontalOptions = LayoutOptions.Center
                };
                answerEntry = new Entry
                {
                    TextColor = Color.White,
                    FontFamily = AgoraFonts.GetPoppinsMedium(),
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    VerticalOptions = LayoutOptions.End,
                    HorizontalTextAlignment = TextAlignment.Center,
                    PlaceholderColor = Color.LightGray,
                    Placeholder = "Answer"
                };

                gridBottom.Children.Add(answerLabel, 1, 0);
                gridBottom.Children.Add(answerEntry, 1, 1);
                gridBottom.Children.Add(answerButton, 1, 2);
            }
            else
            {
                if (actualTask.taskStatus == GameTask.TaskStatus.NotStarted &&
                                (actualTask.title == "The first are the best" || actualTask.title == "AEGEE Army"))
                {
                    answerButton.Text = "START TASK";
                }
                else
                {
                    answerButton.Text = "CHECK TASK";
                }

                gridBottom.Children.Add(answerButton, 1, 2);
            }
            Content = new StackLayout
            {
                Margin = new Thickness(0, 0),
                Children =
                {
                    layout,
                    gridBottom
                }
            };
        }

        public async void OnAnswerButtonClick(object sender, EventArgs e)
        {
            Console.WriteLine("OnAnswerButtonClick");
            if(!isAnswerButtonClicked)
            {
                isAnswerButtonClicked = true;
                if ((actualTask.taskStatus == GameTask.TaskStatus.NotStarted) ||
                    (actualTask.taskStatus == GameTask.TaskStatus.Processing))
                {
                    if (actualTask.taskStatus == GameTask.TaskStatus.NotStarted)
                    {
                        actualTask.taskStatus = GameTask.TaskStatus.Checking;
                    }

                    if (FirebaseMessagingClient.IsNetworkAvailable())
                    {
                        string userAnswer = "";
                        switch (actualTask.taskType)
                        {
                            case TaskType.Text:
                                if (answerEntry.Text != null)
                                {
                                    userAnswer = answerEntry.Text.ToLower();
                                }
                                Console.WriteLine("answerEntry.Text=" + userAnswer);
                                Console.WriteLine("actualTask.correctAnswer" + actualTask.correctAnswer);
                                if (actualTask.correctAnswer == userAnswer)
                                {
                                    Console.WriteLine("Yeah! Correct answer!");
                                    await GamePage.tasksMasterPage.closeTask(actualTask);
                                }
                                else
                                {
                                    DependencyService.Get<IPopup>().ShowPopup("Bad answer!", "Try one more time!", false);
                                    actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                                }
                                break;
                            case TaskType.LongText:
                                if (answerEntry.Text != null)
                                {
                                    userAnswer = answerEntry.Text.ToLower();
                                }
                                Console.WriteLine("answerEntry.LongText=" + userAnswer);
                                int correctAnswers = 0;
                                foreach (string answer in actualTask.correctAnswers)
                                {
                                    if (userAnswer.Contains(answer))
                                    {
                                        correctAnswers++;
                                    }
                                }
                                if (correctAnswers >= actualTask.minimumCorrectAnswers)
                                {
                                    Console.WriteLine("Yeah! Correct answer!");
                                    await GamePage.tasksMasterPage.closeTask(actualTask);
                                }
                                else
                                {
                                    DependencyService.Get<IPopup>().ShowPopup("Bad answer!", "Try one more time!", false);
                                    actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                                }
                                break;
                            case TaskType.Button:
                                bool result;
                                if (actualTask.title == "Adventurer quest")
                                {
                                    result = await Beacons.ScanForBeacon(Beacons.beaconFHNj);
                                    if (result)
                                    {
                                        await GamePage.tasksMasterPage.closeTask(actualTask);
                                    }
                                    else
                                    {
                                        DependencyService.Get<IPopup>().ShowPopup("You're not near beacon!", "Come closer to beacon to complete this task!", false);
                                        actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                                    }
                                }
                                else if (actualTask.taskStatus == GameTask.TaskStatus.Checking &&
                                    (actualTask.title == "The first are the best" || actualTask.title == "AEGEE Army"))
                                {
                                    result = await StartTask();
                                    if (result)
                                    {
                                        actualTask.taskStatus = GameTask.TaskStatus.Processing;
                                        answerButton.Text = "CHECK TASK";
                                        ForceLayout();
                                    }
                                }
                                else
                                {
                                    bool? processResult = await ProcessTask();
                                    if (processResult == true)
                                    {
                                        await GamePage.tasksMasterPage.closeTask(actualTask);
                                    }
                                    else if (processResult == null)
                                    {
                                        DependencyService.Get<IPopup>().ShowPopup("Task isn't completed yet.", "If you already completed it, please try again after some time.", false);
                                        if ((actualTask.title != "AEGEE Army") && (actualTask.title != "The first are the best"))
                                        {
                                            actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                                        }
                                    }
                                    else
                                    {
                                        if (actualTask.title == "The first are the best")
                                        {
                                            DependencyService.Get<IPopup>().ShowPopup("Sorry you're late :(", "Try once again next time!", false);
                                            answerButton.Text = "START TASK";
                                            ForceLayout();
                                            actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                                        }
                                        else if (actualTask.title == "AEGEE Army")
                                        {
                                            DependencyService.Get<IPopup>().ShowPopup("Too few friends :(", "Gather more friends and try once again!", false);
                                        }
                                    }
                                }
                                break;
                            default:
                                Console.WriteLine("Error!");
                                break;
                        }
                    }
                    else
                    {
                        DependencyService.Get<IPopup>().ShowPopup("No internet connection!", "Turn on network to complete task!", false);
                        actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                    }
                }
                isAnswerButtonClicked = false;
            }
        }

        public async Task<bool> StartTask()
        {
            bool result = false;
            String databasePath;
            Console.WriteLine("GameTask:StartTask:task.id=" + actualTask.id);

            switch (actualTask.title)
            {
                case "AEGEE Army":
                    result = await Beacons.ScanForBeacon(Beacons.beaconFHNj);
                    if (result)
                    {
                        databasePath = tasksPath + actualTask.dbName + "/Active/" + Users.loggedUser.AntenaId + "/" + Users.loggedUser.Id.ToString().PadLeft(4, '0');
                        if (FirebaseMessagingClient.SendMessage(databasePath, "1"))
                        {
                            DependencyService.Get<IPopup>().ShowPopup("You're near beacon!", "Great! Now wait for your friends!", true);                            
                            result = true;

                            Task delayTask = Task.Run(async () =>
                            {
                                await Task.Delay(30 * 1000);
                                if (actualTask.taskStatus != GameTask.TaskStatus.Completed)
                                {
                                    DependencyService.Get<IPopup>().ShowPopup("AEGEE Army", "Too few friends were with you :( Gather more and try again!", false);
                                    actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                                    answerButton.Text = "START TASK";
                                    ForceLayout();
                                }
                            });
                        }
                        else
                        {
                            DependencyService.Get<IPopup>().ShowPopup("No internet connection", "You need internet connection to complete this task!", false);
                            actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                        }
                    }
                    else
                    {
                        DependencyService.Get<IPopup>().ShowPopup("You're not near beacon!", "Come closer to beacon to complete this task!", false);
                        actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                    }
                    break;
                case "The first are the best":
                    databasePath = tasksPath + actualTask.dbName + "/Active/" + Users.loggedUser.Id.ToString().PadLeft(4, '0');
                    if (FirebaseMessagingClient.SendMessage(databasePath, "1"))
                    {
                        DependencyService.Get<IPopup>().ShowPopup("Great!", "Now check if you were first!", true);
                        result = true;

                        Task delayTask = Task.Run(async () =>
                        {
                            await Task.Delay(30 * 1000);
                            if (actualTask.taskStatus != GameTask.TaskStatus.Completed)
                            {
                                actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                                answerButton.Text = "START TASK";
                                ForceLayout();
                            }
                        });
                    }
                    else
                    {
                        DependencyService.Get<IPopup>().ShowPopup("No internet connection", "You need internet connection to complete this task!", false);
                        actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                    }
                    break;
            }
            return result;
        }

        public async Task<bool?> ProcessTask()
        {
            bool? result = false;
            try
            {
                String idDbPath = Users.loggedUser.Id.ToString().PadLeft(4, '0');
                result = await FirebaseMessagingClient.SendSingleQuery<bool>(tasksPath + actualTask.dbName + "/" + idDbPath);
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }
    }
}
using AgoraNavigator.Login;
using AgoraNavigator.Popup;
using Plugin.FirebasePushNotification;
using Rg.Plugins.Popup.Extensions;
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

        public TaskDetailView(GameTask task)
        {
            Title = "Task details";
            BackgroundColor = AgoraColor.DarkBlue;
            actualTask = task;

            Grid grid = new Grid
            {
                Padding = 0,
                RowSpacing = 0
            };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(11, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            BoxView boxView = new BoxView
            {
                BackgroundColor = AgoraColor.Blue
            };
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

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // 0 - Field above Title
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Star) }); // 1 - Title
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(5, GridUnitType.Star) }); // 2 - Description
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(4, GridUnitType.Star) }); // 3 - Field bet Desc and Answer Label

            Grid.SetRowSpan(boxView, 3);
            Grid.SetColumnSpan(boxView, 5);
            Grid.SetColumnSpan(titleLabel, 3);
            Grid.SetColumnSpan(descriptionLabel, 3);

            grid.Children.Add(boxView, 0, 5, 0, 3);
            grid.Children.Add(titleLabel, 1, 4, 1, 2);
            grid.Children.Add(descriptionLabel, 1, 4, 2, 3);

            answerButton = new Button
            {
                BackgroundColor = AgoraColor.Blue,
                TextColor = AgoraColor.DarkBlue,
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
            };
            answerButton.Clicked += OnAnswerButtonClick;
            if (task.taskType == TaskType.Text)
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

                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Star) }); // 4 - Answer Label
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Star) }); // 5 - Entry
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // 6 - Field bet Entry and Button
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(4, GridUnitType.Star) }); // 7 - Answer Button
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(8, GridUnitType.Star) }); // 8 - Rest field

                BoxView boxView1 = new BoxView
                {
                    BackgroundColor = Color.Red
                };
                grid.Children.Add(answerLabel, 2, 4);
                grid.Children.Add(answerEntry, 2, 5);
                grid.Children.Add(answerButton, 2, 7);
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
                    
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(4, GridUnitType.Star) }); // 6 - Answer Button
                grid.Children.Add(answerButton, 2, 4);
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(8, GridUnitType.Star) }); // 7 - Rest field              
            }
            Content = grid;
        }

        public async void OnAnswerButtonClick(object sender, EventArgs e)
        {
            Console.WriteLine("OnAnswerButtonClick");
            if ((actualTask.taskStatus == GameTask.TaskStatus.NotStarted) || 
                (actualTask.taskStatus == GameTask.TaskStatus.Processing))
            {
                if(actualTask.taskStatus == GameTask.TaskStatus.NotStarted)
                {
                    actualTask.taskStatus = GameTask.TaskStatus.Checking;
                }
                
                if (FirebaseMessagingClient.IsNetworkAvailable())
                {
                    switch (actualTask.taskType)
                    {
                        case TaskType.Text:
                            Console.WriteLine("answerEntry.Text=" + answerEntry.Text);
                            Console.WriteLine("actualTask.correctAnswer" + actualTask.correctAnswer);
                            if (actualTask.correctAnswer == answerEntry.Text)
                            {
                                Console.WriteLine("Yeah! Correct answer!");
                                await GamePage.tasksMasterPage.closeTask(actualTask);
                            }
                            else
                            {
                                SimplePopup popup = new SimplePopup("Bad answer!", "Try one more time!")
                                {
                                    ColorBackground = Color.Red,
                                    ColorBody = Color.White,
                                    ColorTitle = Color.White,
                                };
                                popup.SetColors();
                                await Navigation.PushPopupAsync(popup);
                                actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                            }
                            break;
                        case TaskType.Button:
                            bool result;
                            if(actualTask.title == "Adventurer quest")
                            {
                                result = await Beacons.ScanForBeacon(Beacons.beaconFHNJ);
                                if (result)
                                {
                                    await GamePage.tasksMasterPage.closeTask(actualTask);
                                }
                                else
                                {
                                    SimplePopup popup = new SimplePopup("You're not near beacon!", "Come closer to beacon to complete this task!")
                                    {
                                        ColorBackground = Color.Red,
                                        ColorBody = Color.White,
                                        ColorTitle = Color.White,
                                    };
                                    popup.SetColors();
                                    await Navigation.PushPopupAsync(popup);
                                    actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                                }
                            }
                            else if (actualTask.taskStatus == GameTask.TaskStatus.Checking && 
                                (actualTask.title == "The first are the best" || actualTask.title == "AEGEE Army"))
                            {
                                result = await StartTask();
                                if(result)
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
                                else if(processResult == null)
                                {
                                    SimplePopup popup = new SimplePopup("Task isn't completed yet.", "If you already completed it, please try again after some time.")
                                    {
                                        ColorBackground = Color.Red,
                                        ColorBody = Color.White,
                                        ColorTitle = Color.White,
                                    };
                                    popup.SetColors();
                                    await Navigation.PushPopupAsync(popup);
                                    if ((actualTask.title != "AEGEE Army") && (actualTask.title != "The first are the best")) 
                                    {
                                        actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                                    }
                                }
                                else
                                {
                                    if(actualTask.title == "The first are the best")
                                    {
                                        SimplePopup popup = new SimplePopup("Sorry you're late :(", "Try once again next time!")
                                        {
                                            ColorBackground = Color.Red,
                                            ColorBody = Color.White,
                                            ColorTitle = Color.White,
                                        };
                                        popup.SetColors();
                                        await Navigation.PushPopupAsync(popup);
                                        answerButton.Text = "START TASK";
                                        ForceLayout();
                                        actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                                    }
                                    else if(actualTask.title == "AEGEE Army")
                                    {
                                        SimplePopup popup = new SimplePopup("Too few friends :(", "Gather more friends and try once again!")
                                        {
                                            ColorBackground = Color.Red,
                                            ColorBody = Color.White,
                                            ColorTitle = Color.White,
                                        };
                                        popup.SetColors();
                                        await Navigation.PushPopupAsync(popup);
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
                    SimplePopup popup = new SimplePopup("No internet connection!", "Turn on network to complete task!")
                    {
                        ColorBackground = Color.Red,
                        ColorBody = Color.White,
                        ColorTitle = Color.White,
                    };
                    popup.SetColors();
                    await Navigation.PushPopupAsync(popup);
                    actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                }
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
                    result = await Beacons.ScanForBeacon(Beacons.beaconFHNJ);
                    if (result)
                    {
                        CrossFirebasePushNotification.Current.Subscribe("AEGEE_Army_" + Users.loggedUser.AntenaId);
                        databasePath = tasksPath + actualTask.dbName + "/Active/" + Users.loggedUser.AntenaId + "/" + Users.loggedUser.Id;
                        if (FirebaseMessagingClient.SendMessage(databasePath, "1"))
                        {
                            SimplePopup popup = new SimplePopup("You're near beacon!", "Great! Now wait for your friends!")
                            {
                                ColorBackground = Color.Green,
                                ColorBody = Color.White,
                                ColorTitle = Color.White,
                            };
                            popup.SetColors();
                            await Navigation.PushPopupAsync(popup);
                            result = true;

                            Task delayTask = Task.Run(async () =>
                            {
                                await Task.Delay(30 * 1000);
                                if (actualTask.taskStatus != GameTask.TaskStatus.Completed)
                                {
                                    DependencyService.Get<INotification>().Notify("AEGEE Army", "Too few friends were with you :( Gather more and try again!");
                                    actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                                    answerButton.Text = "START TASK";
                                    ForceLayout();
                                }
                            });
                        }
                        else
                        {
                            SimplePopup popup = new SimplePopup("No internet connection", "You need internet connection to complete this task!")
                            {
                                ColorBackground = Color.Green,
                                ColorBody = Color.White,
                                ColorTitle = Color.White,
                            };
                            popup.SetColors();
                            await Navigation.PushPopupAsync(popup);
                            actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                        }
                    }
                    else
                    {
                        SimplePopup popup = new SimplePopup("You're not near beacon!", "Come closer to beacon to complete this task!")
                        {
                            ColorBackground = Color.Red,
                            ColorBody = Color.White,
                            ColorTitle = Color.White,
                        };
                        popup.SetColors();
                        await Navigation.PushPopupAsync(popup);
                        actualTask.taskStatus = GameTask.TaskStatus.NotStarted;
                    }
                    break;
                case "The first are the best":
                    CrossFirebasePushNotification.Current.Subscribe("First_Come_First_Served_" + Users.loggedUser.Id);
                    databasePath = tasksPath + actualTask.dbName + "/Active/" + Users.loggedUser.Id;
                    if (FirebaseMessagingClient.SendMessage(databasePath, "1"))
                    {
                        SimplePopup popup = new SimplePopup("Great!", "Now check if you were first!")
                        {
                            ColorBackground = Color.Green,
                            ColorBody = Color.White,
                            ColorTitle = Color.White,
                        };
                        popup.SetColors();
                        await Navigation.PushPopupAsync(popup);
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
                        SimplePopup popup = new SimplePopup("No internet connection", "You need internet connection to complete this task!")
                        {
                            ColorBackground = Color.Green,
                            ColorBody = Color.White,
                            ColorTitle = Color.White,
                        };
                        popup.SetColors();
                        await Navigation.PushPopupAsync(popup);
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
                result = await FirebaseMessagingClient.SendSingleQuery<bool>(tasksPath + actualTask.dbName + "/" + Users.loggedUser.Id);
            }
            catch(Exception)
            {
                result = null;
            }
            return result;
        }
    }
}
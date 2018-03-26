using AgoraNavigator.Popup;
using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;
using static AgoraNavigator.Tasks.GameTask;

namespace AgoraNavigator.Tasks
{
    public class TaskDetailView : ContentPage
    {
        private bool isTaskProcessing = false;
        Entry answerEntry;
        GameTask actualTask;

        public TaskDetailView(GameTask task)
        {
            Title = "Task details";
            BackgroundColor = AgoraColor.DarkBlue;
            actualTask = task;

            Grid grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
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
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
            Label descriptionLabel = new Label
            {
                Text = task.description,
                TextColor = Color.White,
                BackgroundColor = AgoraColor.Blue,
                FontFamily = AgoraFonts.GetPoppinsMedium(),
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
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

            Button answerButton = new Button
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
            else if(task.taskType == TaskType.Button || task.taskType == TaskType.Button)
            {
                answerButton.Text = "CHECK TASK";
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(4, GridUnitType.Star) }); // 6 - Answer Button
                grid.Children.Add(answerButton, 2, 4);
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(8, GridUnitType.Star) }); // 7 - Rest field              
            }
            Content = grid;
        }

        public async void OnAnswerButtonClick(object sender, EventArgs e)
        {
            Console.WriteLine("OnAnswerButtonClick");
            if (!isTaskProcessing)
            {
                isTaskProcessing = true;
                bool result = false;
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
                                result = true;
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
                            }
                            break;
                        case TaskType.Button:
                            result = await ProcessTask(actualTask);
                            break;
                        default:
                            Console.WriteLine("Error!");
                            break;
                    }
                    if (result)
                    {
                        await GamePage.tasksMasterPage.closeTask(actualTask);
                    }
                    else
                    {
                        isTaskProcessing = false;
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
                    isTaskProcessing = false;
                }
            }              
        }
    }
}
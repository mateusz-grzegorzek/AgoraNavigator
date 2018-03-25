using AgoraNavigator.Login;
using AgoraNavigator.Popup;
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

        public TaskDetailView(GameTask task)
        {
            Title = "Task details";
            BackgroundColor = AgoraColor.DarkBlue;
            actualTask = task;
            AbsoluteLayout layout = new AbsoluteLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            BoxView boxView = new BoxView
            {
                Color = AgoraColor.Blue,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            Label titleLabel = new Label
            {
                Text = task.title,
                TextColor = AgoraColor.DarkBlue,
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            Label descriptionLabel = new Label
            {
                Text = task.description,
                TextColor = Color.White,
                FontFamily = AgoraFonts.GetPoppinsMedium(),
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            AbsoluteLayout.SetLayoutBounds(boxView,          new Rectangle(.0,  .0,   1, .30));
            AbsoluteLayout.SetLayoutBounds(titleLabel,       new Rectangle(.1, .05, .60, .10));
            AbsoluteLayout.SetLayoutBounds(descriptionLabel, new Rectangle(.2, .15, .90, .20));

            AbsoluteLayout.SetLayoutFlags(boxView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(titleLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(descriptionLabel, AbsoluteLayoutFlags.All);

            layout.Children.Add(boxView);
            layout.Children.Add(titleLabel);
            layout.Children.Add(descriptionLabel);

            Button answerButton = new Button
            {
                BackgroundColor = AgoraColor.Blue,
                TextColor = AgoraColor.DarkBlue,
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };

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
                Image separator = new Image
                {
                    Source = "entry_separator.png",
                    VerticalOptions = LayoutOptions.End
                };
                answerEntry = new Entry
                {
                    TextColor = Color.White,
                    FontFamily = AgoraFonts.GetPoppinsMedium(),
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    HorizontalTextAlignment = TextAlignment.Center
                };
                AbsoluteLayout.SetLayoutBounds(answerLabel, new Rectangle(.5, .40, .70, .10));
                AbsoluteLayout.SetLayoutBounds(separator,   new Rectangle(.5, .49, .50, .10));
                AbsoluteLayout.SetLayoutBounds(answerEntry, new Rectangle(.5, .50, .50, .10));

                AbsoluteLayout.SetLayoutFlags(answerLabel, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutFlags(separator, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutFlags(answerEntry, AbsoluteLayoutFlags.All);

                layout.Children.Add(answerLabel);
                layout.Children.Add(separator);
                layout.Children.Add(answerEntry);
            }

            answerButton.Pressed += OnAnswerButtonPressed;
            answerButton.Clicked += async (sender, e) =>
            {
                await OnAnswerButtonClick(sender, e);
            };
           
            AbsoluteLayout.SetLayoutBounds(answerButton, new Rectangle(.5, .8, .70, .20));
            AbsoluteLayout.SetLayoutFlags(answerButton, AbsoluteLayoutFlags.All);
            layout.Children.Add(answerButton);
            Content = layout;
        }
        public async void OnAnswerButtonPressed(object sender, EventArgs e)
        {
            Console.WriteLine("OnAnswerButtonPressed");
            if (actualTask.taskType == TaskType.PreBLE)
            {
                bool result = await ProcessTask(actualTask);
                if (result)
                {
                    GamePage.tasksMasterPage.closeTask(actualTask);
                }
            }
        }

        public async Task OnAnswerButtonClick(object sender, EventArgs e)
        {
            Console.WriteLine("OnAnswerButtonClick");
            bool result = false;
            if(FirebaseMessagingClient.IsNetworkAvailable())
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
                    case TaskType.PreBLE:
                        result = await ProcessTask(actualTask);
                        break;
                    default:
                        Console.WriteLine("Error!");
                        break;
                }
                if (result)
                {
                    GamePage.tasksMasterPage.closeTask(actualTask);
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
            }
        }
    }
}
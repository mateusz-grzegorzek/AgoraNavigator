using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            BackgroundColor = Color.Aquamarine;
            actualTask = task;
            var stack = new StackLayout { Spacing = 0 };
            Label description = new Label { Text = task.description };
            stack.Children.Add(description);
            if (task.taskType == TaskType.Text)
            {
                answerEntry = new Entry { };
                stack.Children.Add(answerEntry);
            }
            Button answerButton = new Button { Text = "Send answer" };
            answerButton.Pressed += OnAnswerButtonPressed;
            answerButton.Clicked += async (sender, e) =>
            {
                await OnAnswerButtonClick(sender, e);
            };
            stack.Children.Add(answerButton);
            Content = stack;
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
            switch (actualTask.taskType)
            {
                case TaskType.Text:
                    Console.WriteLine("answerEntry.Text=" + answerEntry.Text);
                    Console.WriteLine("actualTask.correctAnswer" + actualTask.correctAnswer);
                    if (actualTask.correctAnswer == answerEntry.Text)
                    {
                        Console.WriteLine("Yeah! Correct answer!");
                        GamePage.tasksMasterPage.closeTask(actualTask);
                    }
                    else
                    {
                        await DisplayAlert("Task", "Failed! Wrong answer!", "Ok");
                    }
                    break;
                case TaskType.Button:
                    bool result = await ProcessTask(actualTask);
                    if (result)
                    {
                        GamePage.tasksMasterPage.closeTask(actualTask);
                    }
                    break;
                default:
                    Console.WriteLine("Error!");
                    break;
            }
        }
    }
}
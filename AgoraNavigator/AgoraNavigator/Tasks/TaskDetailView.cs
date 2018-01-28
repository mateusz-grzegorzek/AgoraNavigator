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
            answerButton.Clicked += async (sender, e) =>
            {
                await OnAnswerButtonClick(sender, e);
            };
            stack.Children.Add(answerButton);
            Content = stack;
        }

        public async Task OnAnswerButtonClick(object sender, EventArgs e)
        {
            switch (actualTask.taskType)
            {
                case TaskType.Text:
                    Console.WriteLine("answerEntry.Text=" + answerEntry.Text);
                    Console.WriteLine("actualTask.correctAnswer" + actualTask.correctAnswer);
                    //if (actualTask.correctAnswer == answerEntry.Text)
                    //{
                        Console.WriteLine("Yeah! Correct answer!");
                        TasksMasterPage.closeTask(actualTask);
                    //}
                    break;
                case TaskType.Button:
                    bool result = await ProcessTask(actualTask);
                    if (result)
                    {
                        TasksMasterPage.closeTask(actualTask);
                    }
                    break;
                default:
                    Console.WriteLine("Error!");
                    break;
            }
        }
    }
}
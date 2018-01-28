using PCLStorage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AgoraNavigator.Tasks
{
    public class GamePage : ContentPage, INotifyPropertyChanged
    {
        public static int totalPoints = 0;
        Label totalPointsLabel;
        IFolder rootFolder;
        IFolder userDataFolder;
        IFile userDataFile;

        public void addScorePoints(int scorePoints)
        {
            totalPoints += scorePoints;
            totalPointsLabel.Text = "Total points: " + totalPoints.ToString();

        }

        public GamePage()
        {
            Title = "Game of Tasks";
            totalPointsLabel = new Label();
            addScorePoints(0);
            Button goToTasksButton = new Button { Text = "Go to Tasks!" };
            goToTasksButton.Clicked += OnGoToTasksButtonClick;
            var stack = new StackLayout { Spacing = 12 };
            stack.Children.Add(totalPointsLabel);
            stack.Children.Add(goToTasksButton);
            Content = stack;
        }

        async private void initUserData(int user_id)
        {
            rootFolder = FileSystem.Current.LocalStorage;
            userDataFolder = await rootFolder.CreateFolderAsync("user_data_" + user_id, CreationCollisionOption.OpenIfExists);
            userDataFile = await userDataFolder.CreateFileAsync("answer.txt", CreationCollisionOption.OpenIfExists);
            totalPoints = Int32.Parse(await userDataFile.ReadAllTextAsync());
        }

        async public void OnGoToTasksButtonClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(TasksPage.tasksMasterPage);
        }
    }
}
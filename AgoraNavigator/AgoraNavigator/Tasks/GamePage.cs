using AgoraNavigator.Login;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AgoraNavigator.Tasks
{
    public class TopScorers
    {
        public int userId { get; set; }
        public int totalPoints { get; set; }
    }

    public class GamePage : ContentPage
    {
        public static TasksMasterPage tasksMasterPage;
        public static Label totalPointsLabel;
        private const string databaseTopScorersKey = "tasks/TOP_Scorers";
        private ListView topScorersListView;

        public GamePage()
        {
            tasksMasterPage = new TasksMasterPage();
            Title = "Game of Tasks";
            totalPointsLabel = new Label();
            totalPointsLabel.Text = "Total points: " + Users.loggedUser.TotalPoints.ToString();
            Button goToTasksButton = new Button { Text = "Go to Tasks!" };
            goToTasksButton.Clicked += OnGoToTasksButtonClick;

            topScorersListView = new ListView
            {
                ItemTemplate = new DataTemplate(() =>
                {
                    Grid grid = new Grid { Padding = new Thickness(5, 10) };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                    Image image = new Image();
                    image.SetBinding(Image.SourceProperty, "iconSource");
                    Label userIdLabel = new Label { VerticalOptions = LayoutOptions.FillAndExpand, Text = "UserID: " };
                    Label userId = new Label { VerticalOptions = LayoutOptions.FillAndExpand };
                    userId.SetBinding(Label.TextProperty, "userId");
                    Label totalPointsLabel = new Label { VerticalOptions = LayoutOptions.FillAndExpand, Text = "TotalPoints: " };
                    Label totalPoints = new Label { VerticalOptions = LayoutOptions.FillAndExpand };
                    totalPoints.SetBinding(Label.TextProperty, "totalPoints");

                    grid.Children.Add(image);
                    grid.Children.Add(userIdLabel, 1, 0);
                    grid.Children.Add(userId, 2, 0);
                    grid.Children.Add(totalPointsLabel, 3, 0);
                    grid.Children.Add(totalPoints, 4, 0);

                    return new ViewCell { View = grid };
                }),
                SeparatorVisibility = SeparatorVisibility.None
            };

            var stack = new StackLayout { Spacing = 12 };
            stack.Children.Add(totalPointsLabel);
            stack.Children.Add(topScorersListView);
            stack.Children.Add(goToTasksButton);
            Content = stack;
            Appearing += OnPageAppearing;
        }

        private async void OnPageAppearing(object sender, EventArgs e)
        {
            this.IsBusy = true;
            await FetchTopScorersAsync();
            this.IsBusy = false;
        }

        private async Task FetchTopScorersAsync()
        {
            IReadOnlyCollection<FirebaseObject<TopScorers>> items = await FirebaseMessagingClient.SendQuery<TopScorers>(databaseTopScorersKey);
            ObservableCollection<TopScorers> topScorers = new ObservableCollection<TopScorers>();
            foreach (FirebaseObject<TopScorers> groups in items)
            {
                TopScorers topScorer = groups.Object;
                Console.WriteLine("FetchTopScorersAsync:userId=" + topScorer.userId + ", totalPoints=" + topScorer.totalPoints);
                topScorers.Add(topScorer);
            }
            topScorersListView.ItemsSource = topScorers;
        }

        async public void OnGoToTasksButtonClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(tasksMasterPage);
        }
    }
}
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
        public static Label totalPoints;
        private const string databaseTopScorersKey = "tasks/TOP_Scorers";
        private ListView topScorersListView;

        public GamePage()
        {
            Users.InitDefaultUser();
            tasksMasterPage = new TasksMasterPage();
            Title = "Game of Tasks";
            Label totalPointsLabel = new Label
            {
                Text = "Your Total Points: " ,
                FontFamily = AgoraFonts.GetPoppinsBold(),
                TextColor = AgoraColor.Blue,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalTextAlignment = TextAlignment.Center
            };
            totalPoints = new Label
            {
                Text = Users.loggedUser.TotalPoints.ToString(),
                FontFamily = AgoraFonts.GetPoppinsBold(),
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalTextAlignment = TextAlignment.Center
            };

            Button goToTasksButton = new Button
            {
                Text = "GO TO TASKS",
                BackgroundColor = AgoraColor.Blue,
                TextColor = AgoraColor.DarkBlue,
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            };
            goToTasksButton.Clicked += OnGoToTasksButtonClick;

            Label bestPlayersLabel = new Label
            {
                Text = "Best players:",
                FontFamily = AgoraFonts.GetPoppinsBold(),
                TextColor = AgoraColor.Blue,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalTextAlignment = TextAlignment.Center
            };

            topScorersListView = new ListView
            {
                ItemTemplate = new DataTemplate(() =>
                {
                    Grid grid = new Grid { Padding = new Thickness(1, 1) };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });

                    Label userIdLabel = new Label
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Text = "ID: ",
                        TextColor = AgoraColor.Blue,
                        FontFamily = AgoraFonts.GetPoppinsBold(),
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
                    };
                    Label userId = new Label
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        TextColor = AgoraColor.Blue,
                        FontFamily = AgoraFonts.GetPoppinsBold(),
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
                    };
                    userId.SetBinding(Label.TextProperty, "userId");
                    Label totalPoints = new Label
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        TextColor = Color.White,
                        FontFamily = AgoraFonts.GetPoppinsBold(),
                        HorizontalTextAlignment = TextAlignment.End,
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
                    };
                    totalPoints.SetBinding(Label.TextProperty, "totalPoints");

                    grid.Children.Add(userIdLabel);
                    grid.Children.Add(userId, 1, 0);
                    grid.Children.Add(totalPoints, 2, 0);

                    return new ViewCell { View = grid };
                }),
                SeparatorVisibility = SeparatorVisibility.None
            };

            AbsoluteLayout layout = new AbsoluteLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            AbsoluteLayout.SetLayoutBounds(totalPointsLabel,   new Rectangle(.35, .10, .60, .06));
            AbsoluteLayout.SetLayoutBounds(totalPoints,        new Rectangle(.85, .10, .10, .06));
            AbsoluteLayout.SetLayoutBounds(goToTasksButton,    new Rectangle(.50, .25, .60, .15));
            AbsoluteLayout.SetLayoutBounds(bestPlayersLabel,   new Rectangle(.50, .45, .50, .06));
            AbsoluteLayout.SetLayoutBounds(topScorersListView, new Rectangle(.50, .75, .60, .30));

            AbsoluteLayout.SetLayoutFlags(totalPointsLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(totalPoints, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(goToTasksButton, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(bestPlayersLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(topScorersListView, AbsoluteLayoutFlags.All);

            layout.Children.Add(totalPointsLabel);
            layout.Children.Add(totalPoints);
            layout.Children.Add(goToTasksButton);
            layout.Children.Add(bestPlayersLabel);
            layout.Children.Add(topScorersListView);

            Content = layout;
            Appearing += OnPageAppearing;
            BackgroundColor = AgoraColor.DarkBlue;
        }

        private async void OnPageAppearing(object sender, EventArgs e)
        {
            this.IsBusy = true;
            await FetchTopScorersAsync();
            this.IsBusy = false;
        }

        private async Task FetchTopScorersAsync()
        {
            try
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
            catch(Exception err)
            {
                Console.WriteLine(err.ToString());
            }
        }

        async public void OnGoToTasksButtonClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(tasksMasterPage);
        }
    }
}
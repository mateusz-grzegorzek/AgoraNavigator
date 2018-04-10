using AgoraNavigator.Login;
using AgoraNavigator.Menu;
using AgoraNavigator.Popup;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Extensions;

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
        private bool isScanNewTasksButtonClick = false;

        public GamePage()
        {
            Console.WriteLine("GamePage");
            tasksMasterPage = new TasksMasterPage();
            Title = "Game of Tasks";
            Label totalPointsLabel = new Label
            {
                Text = "Your Total Points: ",
                FontFamily = AgoraFonts.GetPoppinsBold(),
                TextColor = AgoraColor.Blue,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };
            totalPoints = new Label
            {
                Text = Users.loggedUser.TotalPoints.ToString(),
                FontFamily = AgoraFonts.GetPoppinsBold(),
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            Button goToTasksButton = new Button
            {
                Text = "GO TO TASKS",
                BackgroundColor = AgoraColor.Blue,
                TextColor = AgoraColor.DarkBlue,
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };
            goToTasksButton.Clicked += OnGoToTasksButtonClick;

            Button scanNewTasksButton = new Button
            {
                Text = "SCAN BEACON FOR NEW TASKS",
                BackgroundColor = AgoraColor.Blue,
                TextColor = AgoraColor.DarkBlue,
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };
            scanNewTasksButton.Clicked += OnScanNewTasksButtonClick;

            Label bestPlayersLabel = new Label
            {
                Text = "Best players:",
                FontFamily = AgoraFonts.GetPoppinsBold(),
                TextColor = AgoraColor.Blue,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            topScorersListView = new ListView
            {
                ItemTemplate = new DataTemplate(() =>
                {
                    Grid grid = new Grid { Padding = new Thickness(1, 1) };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });

                    Label userIdLabel = new Label
                    {
                        Text = "ID: ",
                        TextColor = AgoraColor.Blue,
                        FontFamily = AgoraFonts.GetPoppinsBold(),
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
                    };
                    Label userId = new Label
                    {
                        TextColor = AgoraColor.Blue,
                        FontFamily = AgoraFonts.GetPoppinsBold(),
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
                    };
                    userId.SetBinding(Label.TextProperty, "userId");
                    Label totalPoints = new Label
                    {
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

            StackLayout totalPointsLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10, 20)
            };
            totalPointsLayout.Children.Add(totalPointsLabel);
            totalPointsLayout.Children.Add(totalPoints);

            Grid gridLayout = new Grid();

            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            gridLayout.Children.Add(scanNewTasksButton, 1, 0);
            gridLayout.Children.Add(goToTasksButton, 1, 1);
            gridLayout.Children.Add(bestPlayersLabel, 1, 2);
            gridLayout.Children.Add(topScorersListView, 1, 3);

            StackLayout layout = new StackLayout
            {
                Margin = new Thickness(0, 0),
                Spacing = 5
            };

            layout.Children.Add(totalPointsLayout);
            layout.Children.Add(gridLayout);

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
                String items = await FirebaseMessagingClient.SendSingleQuery<String>(databaseTopScorersKey);
                JArray array = JsonConvert.DeserializeObject<JArray>(items);
                ObservableCollection<TopScorers> topScorers = new ObservableCollection<TopScorers>();
                foreach (JToken token in array)
                {
                    int userId = int.Parse(token["userId"].ToString());
                    int totalPoints = int.Parse(token["totalPoints"].ToString());
                    TopScorers topScorer = new TopScorers
                    {
                        userId = userId,
                        totalPoints = totalPoints
                    };
                    Console.WriteLine("FetchTopScorersAsync:userId=" + topScorer.userId + ", totalPoints=" + topScorer.totalPoints);
                    topScorers.Add(topScorer);
                }
                topScorersListView.ItemsSource = topScorers;

            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }
        }

        async public void OnGoToTasksButtonClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(tasksMasterPage);
        }

        async public void OnScanNewTasksButtonClick(object sender, EventArgs e)
        {
            if (!isScanNewTasksButtonClick)
            {
                isScanNewTasksButtonClick = true;
                bool permissionGranted = await Permissions.GetRuntimePermission(Permission.Location);
                Console.WriteLine("TasksMasterView:OnTaskTitleClick:permissionGranted=" + permissionGranted);
                if (Beacons.IsBluetoothOn() && permissionGranted)
                {
                    bool result = await Beacons.ScanBeaconForNewTasks();
                    if (result)
                    {
                        SimplePopup popup = new SimplePopup("New task's founded!", "Go to tasks and solved them all!", true);
                        await Navigation.PushPopupAsync(popup);
                        GameTask.ReloadOpenedTasks();
                    }
                    else
                    {
                        SimplePopup popup = new SimplePopup("No new task's...", "Keep looking!", false);
                        await Navigation.PushPopupAsync(popup);
                    }
                }
                else
                {
                    SimplePopup popup = new SimplePopup("Bluetooth needed", "Turn on bluetooth and accept location permission to start scanning!", false);
                    await Navigation.PushPopupAsync(popup);
                }
                isScanNewTasksButtonClick = false;
            }
        }
    }
}
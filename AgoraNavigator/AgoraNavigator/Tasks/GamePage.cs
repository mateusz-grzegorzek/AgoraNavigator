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

namespace AgoraNavigator.Tasks
{
    public class TopScorers
    {
        public String userId { get; set; }
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
            Label gameInfoLabel = new Label
            {
                Text = "Take the first place and get your fee back!",
                FontFamily = AgoraFonts.GetPoppinsBold(),
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10, 2),
                HorizontalTextAlignment = TextAlignment.Center
            };
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

            Grid gridBestPlayersLayout = new Grid();

            gridBestPlayersLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridBestPlayersLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Label userIdLabel = new Label
            {
                Text = "ID: ",
                TextColor = AgoraColor.Blue,
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            Label pointsLabel = new Label
            {
                Text = "Points: ",
                TextColor = Color.White,
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            gridBestPlayersLayout.Children.Add(userIdLabel, 0, 0);
            gridBestPlayersLayout.Children.Add(pointsLabel, 1, 0);

            topScorersListView = new ListView
            {
                BackgroundColor = Color.Transparent,
                IsEnabled = false,
                ItemTemplate = new DataTemplate(() =>
                {
                    Grid grid = new Grid { Padding = new Thickness(1, 1) };

                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
              
                    Label userId = new Label
                    {
                        TextColor = AgoraColor.Blue,
                        FontFamily = AgoraFonts.GetPoppinsBold(),
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                        HorizontalOptions = LayoutOptions.Center
                    };
                    userId.SetBinding(Label.TextProperty, "userId");
                    Label totalPoints = new Label
                    {
                        TextColor = Color.White,
                        FontFamily = AgoraFonts.GetPoppinsBold(),
                        HorizontalTextAlignment = TextAlignment.End,
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                        HorizontalOptions = LayoutOptions.Center
                    };
                    totalPoints.SetBinding(Label.TextProperty, "totalPoints");

                    grid.Children.Add(userId, 0, 0);
                    grid.Children.Add(totalPoints, 1, 0);
                    grid.BackgroundColor = AgoraColor.DarkBlue;
                    return new ViewCell { View = grid };
                }),
                SeparatorVisibility = SeparatorVisibility.None
            };

            StackLayout totalPointsLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10, 5)
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
            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            gridLayout.Children.Add(scanNewTasksButton, 1, 0);
            gridLayout.Children.Add(goToTasksButton, 1, 1);
            gridLayout.Children.Add(bestPlayersLabel, 1, 2);
            gridLayout.Children.Add(gridBestPlayersLayout, 1, 3);
            gridLayout.Children.Add(topScorersListView, 1, 4);

            StackLayout layout = new StackLayout
            {
                Margin = new Thickness(0, 0),
                Spacing = 2
            };

            layout.Children.Add(gameInfoLabel);
            layout.Children.Add(totalPointsLayout);
            layout.Children.Add(gridLayout);

            Appearing += OnPageAppearing;
            BackgroundColor = AgoraColor.DarkBlue;
            Content = new ScrollView { Content = layout };
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
                    String userId = token["userId"].ToString();
                    userId = userId.PadLeft(4, '0');
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
                bool permissionGranted = await Permissions.GetRuntimePermission(Permission.LocationWhenInUse);
                Console.WriteLine("TasksMasterView:OnTaskTitleClick:permissionGranted=" + permissionGranted);
                if (Beacons.IsBluetoothOn() && permissionGranted)
                {
                    bool result = await Beacons.ScanBeaconForNewTasks();
                    if (result)
                    {
                        GameTask.ReloadOpenedTasks();
                        int tasksLeft = GameTask.allTasks.Count - Users.loggedUser.OpenedTasks.Count;
                        string bodyMsg;
                        if(tasksLeft == 0)
                        {
                            bodyMsg = "You have found all the tasks!\nGreat job!";
                        }
                        else
                        {
                            bodyMsg = tasksLeft + " tasks still waiting for discover!";
                        }
                        DependencyService.Get<IPopup>().ShowPopup("New task's founded!", "Go to tasks and solved them all!\n" + bodyMsg, true);
                    }
                    else
                    {
                        int tasksLeft = GameTask.allTasks.Count - Users.loggedUser.OpenedTasks.Count;
                        string bodyMsg;
                        if (tasksLeft == 0)
                        {
                            bodyMsg = "You have already found all the tasks!";
                        }
                        else
                        {
                            bodyMsg = "Keep looking!";
                        }
                        DependencyService.Get<IPopup>().ShowPopup("No new task's...", bodyMsg, false); 
                    }
                }
                else
                {
                    DependencyService.Get<IPopup>().ShowPopup("Bluetooth needed", "Turn on bluetooth and accept location permission to start scanning!", false); 
                }
                isScanNewTasksButtonClick = false;
            }
        }
    }
}

using Firebase.Database;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AgoraNavigator.Info
{
    public class News
    {
        public string Title { get; set; }

        public string Body { get; set; }
    }

    public class NewsPage : NavigationPage
    {
        public static NewsMasterPage newsMasterPage;

        public NewsPage()
        {
            BarTextColor = AgoraColor.Blue;
            newsMasterPage = new NewsMasterPage();
            Navigation.PushAsync(newsMasterPage);
        }
    }

    public class NewsMasterPage : ContentPage
    {
        private const string newsScheduleKey = "agora_news";
        private int newsCounter;
        StackLayout stack;

        public NewsMasterPage()
        {
            Title = "News";
            BackgroundColor = Color.White;

            stack = new StackLayout
            {
                Spacing = 3,
                Margin = new Thickness(10, 10)
            };
            ReloadNews();
            Appearing += OnAppearing;
            Content = new ScrollView { Content = stack };
        }

        public async void OnAppearing(object sender, EventArgs e)
        {
            await FetchNewsAsync();
        }

        private void ReloadNews()
        {
            stack.Children.Clear();
            newsCounter = CrossSettings.Current.GetValueOrDefault("News:newsCounter", 0);
            int index = 1;
            while (index <= newsCounter)
            {
                StackLayout layoutNews = new StackLayout
                {
                    Spacing = 5
                };
                Label newsTitle = new Label
                {
                    Text = CrossSettings.Current.GetValueOrDefault("News:Title:" + index, ""),
                    TextColor = AgoraColor.DarkBlue,
                    FontFamily = AgoraFonts.GetPoppinsBold(),
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
                };
                Label newsBody = new Label
                {
                    Text = CrossSettings.Current.GetValueOrDefault("News:Body:" + index, ""),
                    TextColor = AgoraColor.Blue,
                    FontFamily = AgoraFonts.GetPoppinsRegular(),
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                };
                BoxView separator = new BoxView()
                {
                    HeightRequest = 1,
                    BackgroundColor = AgoraColor.Blue
                };
                layoutNews.Children.Add(newsTitle);
                layoutNews.Children.Add(newsBody);
                layoutNews.Children.Add(separator);
                index++;
                stack.Children.Add(layoutNews);
            }
        }

        public async Task FetchNewsAsync(bool forceUpdate = false)
        {
            try
            {
                int versionInDb = await FirebaseMessagingClient.SendSingleQuery<int>(newsScheduleKey + "/version");
                int versionInMemory = CrossSettings.Current.GetValueOrDefault("News:version", 0);
                if ((versionInMemory < versionInDb) || forceUpdate)
                {

                    IReadOnlyCollection<FirebaseObject<News>> newsFiles = await FirebaseMessagingClient.SendQuery<News>(newsScheduleKey + "/news");
                    newsCounter = 0;
                    foreach (FirebaseObject<News> newsFile in newsFiles)
                    {
                        News news = newsFile.Object;
                        CrossSettings.Current.AddOrUpdateValue("News:newsCounter", ++newsCounter);
                        CrossSettings.Current.AddOrUpdateValue("News:Title:" + newsCounter, news.Title);
                        CrossSettings.Current.AddOrUpdateValue("News:Body:" + newsCounter, news.Body);
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ReloadNews();
                        ForceLayout();
                    });

                    CrossSettings.Current.AddOrUpdateValue("News:version", versionInDb);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("FetchNewsAsync:" + err.ToString());
            }
        }
    }
}

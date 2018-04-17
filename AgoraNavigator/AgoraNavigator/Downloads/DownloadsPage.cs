using System;
using Xamarin.Forms;
using Plugin.Settings;
using Firebase.Database;
using System.Collections.Generic;
using AgoraNavigator.Popup;
using System.Threading.Tasks;

namespace AgoraNavigator.Downloads
{
    public class DownloadFile
    {
        public string Url { get; set; }

        public string FileName { get; set; }
    }

    public class DownloadButton : Button
    {
        public String Url;

        public DownloadButton()
        {
            BackgroundColor = AgoraColor.Blue;
            TextColor = AgoraColor.DarkBlue;
            FontFamily = AgoraFonts.GetPoppinsBold();
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));
            Clicked += OnOpenButtonClicked;
        }

        public void OnOpenButtonClicked(object sender, EventArgs e)
        {
            if (FirebaseMessagingClient.IsNetworkAvailable())
            {
                DownloadButton button = (DownloadButton)sender;
                Device.OpenUri(new Uri(button.Url));
            }
            else
            {
                DependencyService.Get<IPopup>().ShowPopup("No internet connection!", "Turn on network to download file!", false);
            }
        }
    }

    public class DownloadsPage : NavigationPage
    {
        public static DownloadsMasterPage downloadsMasterPage;

        public DownloadsPage()
        {
            BarTextColor = AgoraColor.Blue;
            downloadsMasterPage = new DownloadsMasterPage();
            Navigation.PushAsync(downloadsMasterPage);
        }
    }

    public class DownloadsMasterPage : ContentPage
    {
        private const string downloadsScheduleKey = "downloads";
        private int filesCounter;
        StackLayout stack;

        public DownloadsMasterPage()
        {
            Title = "Downloads";
            BackgroundColor = AgoraColor.DarkBlue;

            stack = new StackLayout { Spacing = 2 };
            ReloadDownloads();
            Appearing += OnAppearing;
            Content = new ScrollView { Content = stack };
        }

        public async void OnAppearing(object sender, EventArgs e)
        {
            await FetchDownloadFilesAsync();
        }

        private void ReloadDownloads()
        {
            stack.Children.Clear();
            filesCounter = CrossSettings.Current.GetValueOrDefault("Downloads:filesCounter", 0);
            int index = 1;
            while (index <= filesCounter)
            {
                DownloadButton button = new DownloadButton
                {
                    Text = CrossSettings.Current.GetValueOrDefault("Downloads:Text:" + index, ""),
                    Url = CrossSettings.Current.GetValueOrDefault("Downloads:Url:" + index, "")
                };
                index++;
                stack.Children.Add(button);
            }
        }

        public async Task FetchDownloadFilesAsync(bool forceUpdate = false)
        {
            try
            {
                int versionInDb = await FirebaseMessagingClient.SendSingleQuery<int>(downloadsScheduleKey + "/version");
                int versionInMemory = CrossSettings.Current.GetValueOrDefault("Downloads:version", 0);
                if ((versionInMemory < versionInDb) || forceUpdate)
                {

                    IReadOnlyCollection<FirebaseObject<DownloadFile>> downloadFiles = await FirebaseMessagingClient.SendQuery<DownloadFile>(downloadsScheduleKey + "/files");
                    filesCounter = 0;
                    foreach (FirebaseObject<DownloadFile> downloadFile in downloadFiles)
                    {
                        DownloadFile file = downloadFile.Object;
                        CrossSettings.Current.AddOrUpdateValue("Downloads:filesCounter", ++filesCounter);
                        CrossSettings.Current.AddOrUpdateValue("Downloads:Text:" + filesCounter, file.FileName);
                        CrossSettings.Current.AddOrUpdateValue("Downloads:Url:" + filesCounter, file.Url);
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ReloadDownloads();
                        ForceLayout();
                    });

                    CrossSettings.Current.AddOrUpdateValue("Downloads:version", versionInDb);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("FetchDownloadFilesAsync:" + err.ToString());
            }
        }
    }
}

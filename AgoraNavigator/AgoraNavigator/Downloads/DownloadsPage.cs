using System;
using Xamarin.Forms;
using Plugin.Settings;
using Firebase.Database;
using System.Collections.Generic;
using AgoraNavigator.Popup;
using Rg.Plugins.Popup.Extensions;

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
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button));
            Clicked += DownloadsPage.downloadsMasterPage.OnOpenButtonClickedAsync;
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
        bool downloadsUpToDate = false;
        bool downloadsLoaded = false;

        public DownloadsMasterPage()
        {
            Title = "Downloads";
            BackgroundColor = AgoraColor.DarkBlue;
            Appearing += OnAppearing;
        }

        public void OnAppearing(object sender, EventArgs e)
        {
            if(!downloadsLoaded)
            {
                ProcessDownloads();
                downloadsLoaded = true;
            }
        }

        private void ProcessDownloads()
        {
            StackLayout stack = new StackLayout { Spacing = 5 };
            filesCounter = CrossSettings.Current.GetValueOrDefault("Downloads:filesCounter", 0);
            int index = 1;
            while (index <= filesCounter)
            {
                DownloadButton button = new DownloadButton();
                button.Text = CrossSettings.Current.GetValueOrDefault("Downloads:Text:" + index, "");
                button.Url = CrossSettings.Current.GetValueOrDefault("Downloads:Url:" + index, "");
                index++;
                stack.Children.Add(button);
            }
            Content = stack;
        }

        public async void OnOpenButtonClickedAsync(object sender, EventArgs e)
        {
            if (FirebaseMessagingClient.IsNetworkAvailable())
            {
                DownloadButton button = (DownloadButton)sender;
                Device.OpenUri(new Uri(button.Url));
            }
            else
            {
                SimplePopup popup = new SimplePopup("No internet connection!", "Turn on network to download file!")
                {
                    ColorBackground = Color.Red,
                    ColorBody = Color.White,
                    ColorTitle = Color.White,
                };
                popup.SetColors();
                await Navigation.PushPopupAsync(popup);
            }
        }

        public async void FetchDownloadFilesAsync(bool forceUpdate = false)
        {
            if (!downloadsUpToDate || forceUpdate)
            {
                try
                {
                    IReadOnlyCollection<FirebaseObject<DownloadFile>> downloadFiles = await FirebaseMessagingClient.SendQuery<DownloadFile>(downloadsScheduleKey);
                    filesCounter = 0;
                    foreach (FirebaseObject<DownloadFile> downloadFile in downloadFiles)
                    {
                        DownloadFile file = downloadFile.Object;
                        CrossSettings.Current.AddOrUpdateValue("Downloads:filesCounter", ++filesCounter);
                        CrossSettings.Current.AddOrUpdateValue("Downloads:Text:" + filesCounter, file.FileName);
                        CrossSettings.Current.AddOrUpdateValue("Downloads:Url:" + filesCounter, file.Url);
                    }
                    ProcessDownloads();
                    downloadsUpToDate = true;
                }
                catch (Exception)
                {

                }
            }

        }
    }
}

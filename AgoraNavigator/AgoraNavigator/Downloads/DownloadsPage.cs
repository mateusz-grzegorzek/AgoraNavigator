using System;
using Xamarin.Forms;
using Plugin.Settings;

namespace AgoraNavigator.Downloads
{
    public class DownloadButton : Button
    {
        public String Url;
    }

    public class DownloadsPage : NavigationPage
    {
        public static DownloadsMasterPage downloadsMasterPage;

        public DownloadsPage()
        {
            downloadsMasterPage = new DownloadsMasterPage();
            Navigation.PushAsync(downloadsMasterPage);
        }
    }

    public class DownloadsMasterPage : ContentPage
    {
        private StackLayout stack;
        private int filesCounter;

        public DownloadsMasterPage()
        {
            Title = "Downloads";

            stack = new StackLayout { Spacing = 0 };
            filesCounter = CrossSettings.Current.GetValueOrDefault("filesCounter", 0);
            int index = 1;
            while(index <= filesCounter)
            {
                DownloadButton openButton = new DownloadButton();
                openButton.Text = CrossSettings.Current.GetValueOrDefault("filesCounter:Text:" + index, "").ToString();
                openButton.Url = CrossSettings.Current.GetValueOrDefault("filesCounter:Url:" + index, "").ToString();
                openButton.Clicked += OnOpenButtonClicked;
                index++;
                stack.Children.Add(openButton);
            }
            Content = new ScrollView { Content = stack };
        }

        public void AddNewFile(String url, String fileName)
        {
            bool isAlready = false;
            DownloadButton openButton = new DownloadButton();
            openButton.Text = "Open " + fileName;
            openButton.Url = url;
            openButton.Clicked += OnOpenButtonClicked;
            foreach (View view in stack.Children)
            {
                DownloadButton button = (DownloadButton)view;
                if(button.Url == url)
                {
                    isAlready = true;
                }
            }
            if(!isAlready)
            {
                stack.Children.Add(openButton);
                filesCounter = CrossSettings.Current.GetValueOrDefault("filesCounter", 0);
                CrossSettings.Current.AddOrUpdateValue("filesCounter", ++filesCounter);
                CrossSettings.Current.AddOrUpdateValue("filesCounter:Text:" + filesCounter, openButton.Text);
                CrossSettings.Current.AddOrUpdateValue("filesCounter:Url:" + filesCounter, openButton.Url);
            }   
        }

        public void OnOpenButtonClicked(object sender, EventArgs e)
        {
            DownloadButton button = (DownloadButton)sender;
            Device.OpenUri(new Uri(button.Url));
        }
    }
}

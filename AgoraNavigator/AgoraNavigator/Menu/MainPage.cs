using System;
using Xamarin.Forms;
using AgoraNavigator.Schedule;
using AgoraNavigator.Tasks;
using Plugin.Permissions.Abstractions;
using AgoraNavigator.Login;
using AgoraNavigator.Contact;
using AgoraNavigator.Info;
using AgoraNavigator.GoogleMap;
using AgoraNavigator.Downloads;

namespace AgoraNavigator.Menu
{
    public class MainPage : MasterDetailPage
    {
        public static MasterPage masterPage;
        public static WelcomePage welcomePage;
        public static MapPage mapPage;
        public static SchedulePage schedulePage;
        public static TasksPage tasksPage;
        public static GameLoginNavPage gameLoginNavPage;
        public static ContactPage contactPage;
        public static InfoPage infoPage;
        public static DownloadsPage downloadsPage;

        public MainPage()
        {
            Console.WriteLine("MainPage");
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetTitleIcon(this, "Hamburger_Icon.png");
            this.BackgroundColor = Color.FromHex("061d3f");
            masterPage = new MasterPage();
            welcomePage = new WelcomePage();
            mapPage = new MapPage();
            schedulePage = new SchedulePage();
            gameLoginNavPage = new GameLoginNavPage();
            contactPage = new ContactPage();
            infoPage = new InfoPage();
            downloadsPage = new DownloadsPage();
            masterPage.getListView.ItemSelected += OnItemSelected;
            Master = masterPage;
            Detail = welcomePage;
            SchedulePage.scheduleDaysPage.FetchScheduleAsync();
        }

        public void SetStartedPage(string pageName)
        {
            switch(pageName)
            {
                case "WelcomePage":
                    Detail = welcomePage;
                    break;
                case "MapPage":
                    Detail = mapPage;
                    break;
                case "SchedulePage":
                    Detail = schedulePage;
                    break;
                case "GameLoginNavPage":
                    Detail = gameLoginNavPage;
                    break;
                case "ContactPage":
                    Detail = contactPage;
                    break;
                case "InfoPage":
                    Detail = infoPage;
                    break;
                case "DownloadsPage":
                    Detail = downloadsPage;
                    break;
            }
        }

        public void UserLoggedSuccessfully()
        {
            tasksPage = new TasksPage();
            Detail = tasksPage;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is MasterPageItem item)
            {
                switch (item.Title)
                {
                    case "Map":
                        GoogleMapPage.map.MyLocationEnabled = await Permissions.GetRuntimePermission(Permission.Location);
                        Detail = mapPage;
                        break;
                    case "Schedule":
                        Detail = schedulePage;
                        break;
                    case "Tasks":
                        if (Users.isUserLogged)
                        {
                            UserLoggedSuccessfully();
                        }
                        else
                        {
                            Detail = gameLoginNavPage;
                        }
                        break;
                    case "Contact":
                        Detail = contactPage;
                        break;
                    case "Important info":
                        Detail = infoPage;
                        break;
                    case "Downloads":
                        Detail = downloadsPage;
                        break;
                    default:
                        break;
                }
                masterPage.getListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}

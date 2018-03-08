using System;
using Xamarin.Forms;
using AgoraNavigator.Schedule;
using AgoraNavigator.Tasks;
using Plugin.Permissions.Abstractions;
using AgoraNavigator.Login;
using AgoraNavigator.Contact;
using AgoraNavigator.Info;
using AgoraNavigator.GoogleMap;

namespace AgoraNavigator.Menu
{
    public class MainPage : MasterDetailPage
    {
        public static MasterPage masterPage;
        public static MapPage mapPage;
        public static SchedulePage schedulePage;
        public static TasksPage tasksPage;
        public static GameLoginNavPage gameLoginNavPage;
        public static ContactPage contactPage;
        public static InfoPage infoPage;

        public MainPage()
        {
            Console.WriteLine("MainPage");
            NavigationPage.SetHasNavigationBar(this, false);
            masterPage = new MasterPage();
            mapPage = new MapPage();
            schedulePage = new SchedulePage();
            gameLoginNavPage = new GameLoginNavPage();
            contactPage = new ContactPage();
            infoPage = new InfoPage();
            masterPage.getListView.ItemSelected += OnItemSelected;
            Master = masterPage;
            Detail = infoPage;
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
                    default:
                        break;
                }
                masterPage.getListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}

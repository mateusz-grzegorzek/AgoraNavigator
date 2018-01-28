using System;
using Xamarin.Forms;
using AgoraNavigator.Schedule;
using AgoraNavigator.Tasks;
using Plugin.Permissions.Abstractions;

namespace AgoraNavigator.Menu
{
    public class MainPage : MasterDetailPage
    {
        public MasterPage getMasterPage { get { return masterPage; } }

        MasterPage masterPage;
        public static MapPage mapPage;
        public static SchedulePage schedulePage;
        public static TasksPage tasksPage;

        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            Console.WriteLine("MainPage");
            masterPage = new MasterPage();
            masterPage.getListView.ItemSelected += OnItemSelected;
            Master = masterPage;
            mapPage = new MapPage();
            schedulePage = new SchedulePage();
            tasksPage = new TasksPage();
            Detail = tasksPage;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                switch(item.Title)
                {
                    case "Map":
                        GoogleMapPage.map.IsShowingUser = await Permissions.GetRuntimePermission(Permission.Location);
                        Detail = mapPage;
                        break;
                    case "Schedule":
                        Detail = schedulePage;
                        break;
                    case "Tasks":
                        Detail = tasksPage;
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

using System;
using Xamarin.Forms;
using AgoraNavigator.Schedule;
using AgoraNavigator.Tasks;
using Plugin.Permissions.Abstractions;
using AgoraNavigator.Login;
using System.Threading.Tasks;
using System.Threading;

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
            Task.Factory.StartNew(AgoraTcpClient.TcpClientThread,
                CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
            Users.InitUserData(new User { Id = 7 }); // ToDo: Remove in Release version
            NavigationPage.SetHasNavigationBar(this, false);
            Console.WriteLine("MainPage");
            masterPage = new MasterPage();
            masterPage.getListView.ItemSelected += OnItemSelected;
            Master = masterPage;
            mapPage = new MapPage();
            schedulePage = new SchedulePage();
            
            Detail = mapPage;
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
                        if(tasksPage == null) /* workaround for development -> ToDo: remove in Release version */
                        {
                            tasksPage = new TasksPage();
                        }
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

using System;
using Xamarin.Forms;
using AgoraNavigator.Schedule;
using AgoraNavigator.Tasks;
using Plugin.Permissions.Abstractions;
using AgoraNavigator.Login;
using System.Threading.Tasks;
using System.Threading;
using AgoraNavigator.Contact;
using AgoraNavigator.Info;
using AgoraNavigator.GoogleMap;

namespace AgoraNavigator.Menu
{
    public class MainPage : MasterDetailPage
    {
        public MasterPage getMasterPage { get { return masterPage; } }

        MasterPage masterPage;
        public static MapPage mapPage;
        public static SchedulePage schedulePage;
        public static TasksPage tasksPage;
        public static ContactPage contactPage;
        public static InfoPage infoPage;

        public MainPage()
        {
            Task.Factory.StartNew(AgoraTcpClient.TcpClientThread,
                CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
            //Users.InitUserData(new User { Id = 7 }); // ToDo: Remove in Release version
            NavigationPage.SetHasNavigationBar(this, false);
            Console.WriteLine("MainPage");
            masterPage = new MasterPage();
            masterPage.getListView.ItemSelected += OnItemSelected;
            Master = masterPage;
            infoPage = new InfoPage();

            Detail = infoPage;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                switch(item.Title)
                {
                    case "Map":
                        if (mapPage == null)
                        {
                            mapPage = new MapPage();
                        }
                        GoogleMapPage.map.IsShowingUser = await Permissions.GetRuntimePermission(Permission.Location);
                        Detail = mapPage;
                        break;
                    case "Schedule":
                        if (schedulePage == null)
                        {
                            schedulePage = new SchedulePage();
                        }
                        Detail = schedulePage;
                        break;
                    case "Tasks":
                        if(tasksPage == null)
                        {
                            tasksPage = new TasksPage();
                        }
                        Detail = tasksPage;
                        break;
                    case "Contact":
                        if (contactPage == null)
                        {
                            contactPage = new ContactPage();
                        }
                        Detail = contactPage;
                        break;
                    case "Important info":
                        if (infoPage == null)
                        {
                            infoPage = new InfoPage();
                        }
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

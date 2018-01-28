using System;
using Xamarin.Forms;
using AgoraNavigator.Schedule;
using AgoraNavigator.Tasks;

namespace AgoraNavigator
{
    public class MainPage : MasterDetailPage
    {
        public MasterPage getMasterPage { get { return masterPage; } }

        MasterPage masterPage;
        static public MapPage mapPage;
        static public SchedulePage schedulePage;
        static public TasksPage tasksPage;

        public MainPage()
        {
            Console.WriteLine("MainPage");
            masterPage = new MasterPage();
            Master = masterPage;
            mapPage = new MapPage();
            schedulePage = new SchedulePage();
            tasksPage = new TasksPage();
            Detail = tasksPage;

            masterPage.getListView.ItemSelected += OnItemSelected;
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                switch(item.Title)
                {
                    case "Map":
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

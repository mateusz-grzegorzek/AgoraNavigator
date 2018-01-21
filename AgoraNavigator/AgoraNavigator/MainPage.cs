using System;
using Xamarin.Forms;

namespace AgoraNavigator
{
    public class MainPage : MasterDetailPage
    {
        public MasterPage getMasterPage { get { return masterPage; } }

        MasterPage masterPage;
        static public TasksPage tasksPage;

        public MainPage()
        {
            masterPage = new MasterPage();
            Master = masterPage;
            tasksPage = new TasksPage();
            Detail = new NavigationPage(tasksPage);

            masterPage.getListView.ItemSelected += OnItemSelected;
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                masterPage.getListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}

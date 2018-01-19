using System;
using Xamarin.Forms;

namespace AgoraNavigator
{
    public class MainPage : MasterDetailPage
    {
        public MasterPage getMasterPage { get { return masterPage; } }

        MasterPage masterPage;

        public MainPage()
        {
            masterPage = new MasterPage();
            Master = masterPage;
            Detail = new NavigationPage(new StartingPage(this));

            masterPage.getListView.ItemSelected += OnItemSelected;
        }

        public void StartNavigator()
        {
            masterPage.ShowMenu();
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MapPage)));       
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

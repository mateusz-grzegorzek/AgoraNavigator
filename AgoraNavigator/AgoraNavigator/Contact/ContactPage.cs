using Xamarin.Forms;

namespace AgoraNavigator.Contact
{
    public class ContactPage : NavigationPage
    {
        public static ContactMasterPage contactMasterPage;

        public ContactPage()
        {
            contactMasterPage = new ContactMasterPage();
            Navigation.PushAsync(contactMasterPage);
        }
    }

    public class ContactMasterPage : ContentPage
    {
        public ContactMasterPage()
        {
            Title = "Contact";
            Content = new StackLayout
            {
                Children = {
               new Label { Text = "Welcome to Xamarin.Forms!" }
            }
            };
        }
    }
}
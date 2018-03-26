using System.Collections.Generic;
using Xamarin.Forms;

namespace AgoraNavigator.Contact
{
    public class Contact
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
    }

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
        ListView contactslistView;

        public ContactMasterPage()
        {
            Title = "Contact";
            List<Contact> contactsList = new List<Contact>();

            contactsList.Add(new Contact
            {
                Name = "Karolina Lapczyk",
                Position = "Main Cordinator",
                PhoneNumber = "+48123456789"
            });


            contactslistView = new ListView
            {
                ItemsSource = contactsList,
                ItemTemplate = new DataTemplate(() =>
                {
                    Grid grid = new Grid
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                    };
                    return new ViewCell { View = grid };
                })
            };
            StackLayout stack = new StackLayout();
            stack.Children.Add(contactslistView);
            Content = stack;
        }
    }
}
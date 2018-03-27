using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AgoraNavigator.Contact
{
    public class Contact
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
        public string Photo { get; set; }
    }

    public class ContactPage : NavigationPage
    {
        public static ContactMasterPage contactMasterPage;

        public ContactPage()
        {
            BarTextColor = AgoraColor.Blue;
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
                Photo = "Contact_Photo_Karolina_Lapczyk.png",
                Name = "Karolina Lapczyk",
                Position = "Main Cordinator",
                PhoneNumber = "+48 123 456 789"
            });

            contactslistView = new ListView
            {
                ItemsSource = contactsList,
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(() =>
                {
                    Grid grid = new Grid
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                    };
                    
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                    
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
                    
                    Image photo = new Image
                    {
                        
                    };
                    photo.SetBinding(Image.SourceProperty, "Photo");

                    Label nameLabel = new Label
                    {
                        FontFamily = AgoraFonts.GetPoppinsBold(),
                        TextColor = AgoraColor.Blue,
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                        VerticalOptions = LayoutOptions.Start,
                        VerticalTextAlignment = TextAlignment.Start
                    };
                    nameLabel.SetBinding(Label.TextProperty, "Name");
                    Label positionLabel = new Label
                    {
                        FontFamily = AgoraFonts.GetPoppinsBold(),
                        TextColor = AgoraColor.DarkBlue,
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                        VerticalOptions = LayoutOptions.Start,
                        VerticalTextAlignment = TextAlignment.Start
                    };
                    positionLabel.SetBinding(Label.TextProperty, "Position");
                    Label phoneNumberLabel = new Label
                    {
                        FontFamily = AgoraFonts.GetPoppinsRegular(),
                        TextColor = AgoraColor.Gray,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        VerticalOptions = LayoutOptions.Start,
                        VerticalTextAlignment = TextAlignment.Start
                    };
                    phoneNumberLabel.SetBinding(Label.TextProperty, "PhoneNumber");
                    Image phoneIcon = new Image
                    {
                        Source = "Contact_Phone_Icon.png"
                    };

                    Grid.SetRowSpan(photo, 3);
                    grid.Children.Add(photo, 0, 1, 0, 2);

                    grid.Children.Add(nameLabel, 1, 0);
                    grid.Children.Add(positionLabel, 1, 1);
                    grid.Children.Add(phoneNumberLabel, 1, 2);

                    Grid.SetRowSpan(phoneIcon, 3);
                    grid.Children.Add(phoneIcon, 2, 3, 0, 2);

                    return new ViewCell { View = grid };
                })
            };
            contactslistView.ItemSelected += OnContactItemSelected;

            StackLayout stack = new StackLayout();
            stack.Children.Add(contactslistView);
            Content = stack;
        }

        private void OnContactItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Contact item = (Contact)((ListView)sender).SelectedItem;
            Device.OpenUri(new Uri("tel:" + item.PhoneNumber));
        }
    }
}
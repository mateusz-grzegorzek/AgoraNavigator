﻿using System;
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

            Label infoLabel = new Label
            {
                Text = "You can find here contact info for organizers. During AGORA, we are really busy, so please contact with us only if it is necessary.",
                FontFamily = AgoraFonts.GetPoppinsBold(),
                TextColor = AgoraColor.Gray,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(10, 10)
            };

            List<Contact> contactsList = new List<Contact>();
            contactsList.Add(new Contact
            {
                Photo = "Contact_Photo_Karolina_Lapczyk.png",
                Name = "Karolina Lapczyk",
                Position = "Main Cordinator",
                PhoneNumber = "+48 123 456 789"
            });
            contactsList.Add(new Contact
            {
                Photo = "Contact_Photo_Mateusz_Gierczak.png",
                Name = "Mateusz Gierczak",
                Position = "Sleep Responsible",
                PhoneNumber = "+48 789 555 123"
            });

            contactslistView = new ListView
            {
                ItemsSource = contactsList,
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(() =>
                {
                    Grid grid = new Grid
                    {
                        Margin = new Thickness(5, 0)
                    };

                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });

                    Image separatorStart = new Image
                    {
                        Source = "Contact_Separator.png",
                        VerticalOptions = LayoutOptions.Start
                    };
                    Image photo = new Image
                    {
                        VerticalOptions = LayoutOptions.Center
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
                        Source = "Contact_Phone_Icon.png",
                        VerticalOptions = LayoutOptions.Center,
                        Margin = new Thickness(10, 10)
                    };
                    Image separatorEnd = new Image
                    {
                        Source = "Contact_Separator.png",
                        VerticalOptions = LayoutOptions.End
                    };

                    Grid.SetRowSpan(photo, 3);
                    grid.Children.Add(photo, 0, 1, 0, 3);

                    grid.Children.Add(nameLabel, 1, 0);
                    grid.Children.Add(positionLabel, 1, 1);
                    grid.Children.Add(phoneNumberLabel, 1, 2);

                    Grid.SetColumnSpan(separatorStart, 3);
                    Grid.SetColumnSpan(separatorEnd, 3);
                    grid.Children.Add(separatorStart, 0, 3, 0, 1);
                    grid.Children.Add(separatorEnd, 0, 3, 2, 3);

                    Grid.SetRowSpan(phoneIcon, 3);
                    grid.Children.Add(phoneIcon, 2, 3, 0, 3);

                    return new ViewCell { View = grid };
                })
            };
            contactslistView.ItemSelected += OnContactItemSelected;

            StackLayout stack = new StackLayout();
            stack.Children.Add(infoLabel);
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
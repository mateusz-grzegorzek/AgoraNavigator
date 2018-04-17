using AgoraNavigator.Popup;
using Plugin.LocalNotifications;
using Plugin.Settings;
using System;
using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{
    public class ScheduleEventDetails : ContentPage
    {
        private ScheduleItemViewModel item;
        private bool favouriteBtnClicked = false;

        public ScheduleEventDetails(ScheduleItemViewModel item)
        {
            this.item = item;

            Title = "Schedule";

            StackLayout stackLayout = new StackLayout
            {
                Margin = new Thickness(10, 10),
                Spacing = 10,
                Padding = new Thickness(5, 5)
            };

            StackLayout stackLayoutTop = new StackLayout
            {
                Spacing = 5
            };

            Label timeTextLabel = new Label()
            {
                Text = item.TimeText,
                TextColor = AgoraColor.Blue,
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };

            Label titleLabel = new Label()
            {
                Text = item.scheduleItem.Title,
                TextColor = AgoraColor.Dark,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                FontFamily = AgoraFonts.GetPoppinsRegular(),
                HorizontalOptions = LayoutOptions.Center
            };

            stackLayoutTop.Children.Add(timeTextLabel);
            stackLayoutTop.Children.Add(titleLabel);

            stackLayout.Children.Add(stackLayoutTop);

            BoxView boxViewTop = new BoxView()
            {
                HeightRequest = 1,
                BackgroundColor = AgoraColor.Blue
            };

            Label descriptionLabel = new Label()
            {
                Text = item.scheduleItem.Description,
                FontFamily = AgoraFonts.GetPoppinsRegular(),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                Style = Device.Styles.BodyStyle
            };

            BoxView boxViewMiddle = new BoxView()
            {
                HeightRequest = 1,
                BackgroundColor = AgoraColor.Blue
            };

            Image schedulePin = new Image()
            {
                Source = "Schedule_Pin.png",
                HeightRequest = 90
            };

            Label placeLabel = new Label()
            {
                Text = item.scheduleItem.Place,
                TextColor = AgoraColor.DarkBlue,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                FontFamily = AgoraFonts.GetPoppinsBold()
            };

            Label addressLabel = new Label()
            {
                Text = item.scheduleItem.Address,
                TextColor = AgoraColor.Blue,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                FontFamily = AgoraFonts.GetPoppinsBold()
            };

            Button showOnMapBtn = new Button()
            {
                Text = "Show on map",
                BackgroundColor = AgoraColor.Blue,
                TextColor = Color.White,
            };
            showOnMapBtn.Clicked += ShowOnMapBtn_Clicked;

            BoxView boxViewBottom = new BoxView()
            {
                HeightRequest = 1,
                BackgroundColor = AgoraColor.Blue
            };

            Button addToFavourite = new Button()
            {
                BackgroundColor = AgoraColor.Blue,
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 100
            };
            bool favourite = CrossSettings.Current.GetValueOrDefault("Schedule_Favourite_" + item.scheduleItem.EventId, false);
            if (favourite)
            {
                addToFavourite.Text = "Remove event from favourite";
            }
            else
            {
                addToFavourite.Text = "Add event to favourite";
            }
            addToFavourite.Clicked += AddToFavouriteBtn_Clicked;

            stackLayout.Children.Add(boxViewTop);
            stackLayout.Children.Add(descriptionLabel);
            stackLayout.Children.Add(boxViewMiddle);

            StackLayout stackLayoutMiddle = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(10, 5),
                Spacing = 20
            };

            stackLayoutMiddle.Children.Add(schedulePin);

            StackLayout stackLayoutMiddleRight = new StackLayout
            {
                HorizontalOptions = LayoutOptions.End
            };

            stackLayoutMiddleRight.Children.Add(placeLabel);
            stackLayoutMiddleRight.Children.Add(addressLabel);
            stackLayoutMiddleRight.Children.Add(showOnMapBtn);

            stackLayoutMiddle.Children.Add(stackLayoutMiddleRight);

            stackLayout.Children.Add(stackLayoutMiddle);
            stackLayout.Children.Add(boxViewBottom);
            stackLayout.Children.Add(addToFavourite);

            Content = new ScrollView { Content = stackLayout };
        }

        private void ShowOnMapBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
            App.mainPage.OpenMapAtAsync(item.scheduleItem.CoordX, item.scheduleItem.CoordY);
        }

        private void AddToFavouriteBtn_Clicked(object sender, EventArgs e)
        {
            if (!favouriteBtnClicked)
            {
                favouriteBtnClicked = true;
                Button button = (Button)sender;
                bool favourite = CrossSettings.Current.GetValueOrDefault("Schedule_Favourite_" + item.scheduleItem.EventId, false);
                if (favourite)
                {
                    button.Text = "Add event to favourite";
                    CrossSettings.Current.AddOrUpdateValue("Schedule_Favourite_" + item.scheduleItem.EventId, false);
                    DependencyService.Get<IPopup>().ShowPopup("Removed from favourite!", "You will not receive notification :(\n", false);
                    CrossLocalNotifications.Current.Cancel(item.scheduleItem.EventId);
                }
                else
                {
                    button.Text = "Remove event from favourite";
                    CrossSettings.Current.AddOrUpdateValue("Schedule_Favourite_" + item.scheduleItem.EventId, true);
                    DependencyService.Get<IPopup>().ShowPopup("Added to favourite!", "You will be notified 15 min before event start! :)\n", true);
                    DateTime notifyTime = item.scheduleItem.StartTime - new TimeSpan(0, 15, 0);
                    CrossLocalNotifications.Current.Show(item.scheduleItem.Title, "Event will start in 15 min!", item.scheduleItem.EventId, notifyTime);
                }
                ForceLayout();
                SchedulePage.scheduleDaysPage.LoadEventsFromMemory();
                favouriteBtnClicked = false;
            }
        }
    }
}
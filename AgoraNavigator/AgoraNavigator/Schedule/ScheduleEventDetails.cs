using System;
using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{
    public class ScheduleEventDetails : ContentPage
    {
        private ScheduleItemViewModel item;

        public ScheduleEventDetails(ScheduleItemViewModel item)
        {
            this.item = item;

            Title = "Schedule";

            StackLayout header = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(20, 0),
                Spacing = 0,
                Children =
                {
                    new Label()
                    {
                        Text = item.TimeText,
                        TextColor = AgoraColor.Blue,
                        FontFamily = AgoraFonts.GetPoppinsBold(),
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    },
                    new Label()
                    {
                        Text = item.scheduleItem.Title,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        FontFamily = AgoraFonts.GetPoppinsRegular()
                    }
                }
            };


            StackLayout body = new StackLayout()
            {
                Padding = new Thickness(20, 0),
                Children =
                {
                    new Label()
                    {
                        Text = item.scheduleItem.Description,
                        FontFamily = AgoraFonts.GetPoppinsRegular(),
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        Style = Device.Styles.BodyStyle
                    }
                }
            };

            Button showOnMapBtn = new Button()
            {
                Text = "Show on map",
                BackgroundColor = AgoraColor.Blue,
                TextColor = Color.White
            };
            showOnMapBtn.Clicked += ShowOnMapBtn_Clicked;


            StackLayout address = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(20, 0),
                Children =
                {
                    new Image()
                    {
                        Source = "schedulePin.png",
                        HeightRequest = 90
                    },
                    new StackLayout()
                    {
                        Orientation = StackOrientation.Vertical,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Children =
                        {
                            new Label()
                            {
                                Text = item.scheduleItem.Place,
                                TextColor = AgoraColor.Blue,
                                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                                FontFamily = AgoraFonts.GetPoppinsBold()
                            },
                            new Label()
                            {
                                Text = item.scheduleItem.Address,
                                TextColor = AgoraColor.Blue,
                                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                                FontFamily = AgoraFonts.GetPoppinsBold()
                            },
                            showOnMapBtn
                        }
                    }
                }
            };

            Content = new ScrollView
            {
                Content = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    Children =
                    {
                        header,

                        new StackLayout()
                         {
                            Padding = new Thickness(20),
                            Children =
                            {
                                new BoxView()
                                {
                                    HeightRequest = 1,
                                    BackgroundColor = AgoraColor.Gray
                                }
                            }
                        },

                        body,

                        new StackLayout()
                         {
                            Padding = new Thickness(20),
                            Children =
                            {
                                new BoxView()
                                {
                                    HeightRequest = 1,
                                    BackgroundColor = AgoraColor.Blue
                                }
                            }
                        },

                        address,

                        new StackLayout()
                         {
                            Padding = new Thickness(20),
                            Children =
                            {
                                new BoxView()
                                {
                                    HeightRequest = 1,
                                    BackgroundColor = AgoraColor.Blue
                                }
                            }
                        },
                    }
                }
            };
        }

        private void ShowOnMapBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
            App.mainPage.OpenMapAtAsync(item.scheduleItem.CoordX, item.scheduleItem.CoordY);
        }
    }
}
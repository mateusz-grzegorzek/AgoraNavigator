using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AgoraNavigator.Schedule
{
	public class ScheduleEventDetails : ContentPage
	{
        private ScheduleItemViewModel item;

		public ScheduleEventDetails (ScheduleItemViewModel item)
		{
            this.item = item;

            Title = "Schedule";

            var header = new StackLayout()
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
                        FontSize = 20,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    },
                    new Label()
                    {
                        Text = item.Title,
                        FontSize = 18,
                        FontFamily = AgoraFonts.GetPoppinsRegular()
                    }
                }
            };


            var body = new StackLayout()
            {
                Padding = new Thickness(20, 0),
                Children =
                {
                    new Label()
                    {
                        Text = item.Description,
                        FontFamily = AgoraFonts.GetPoppinsRegular(),
                        FontSize = 18,
                        Style = Device.Styles.BodyStyle
                    }
                }
            };

            var showOnMapBtn = new Button()
            {
                Text = "Show on map",
                BackgroundColor = AgoraColor.Blue,
                TextColor = Color.White
            };
            showOnMapBtn.Clicked += ShowOnMapBtn_Clicked;


            var address = new StackLayout()
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
                                Text = item.Place,
                                TextColor = AgoraColor.Blue,
                                FontSize = 20,
                                FontFamily = AgoraFonts.GetPoppinsBold()
                            },
                            new Label()
                            {
                                Text = item.Address,
                                TextColor = AgoraColor.Blue,
                                FontSize = 20,
                                FontFamily = AgoraFonts.GetPoppinsBold()
                            },
                            showOnMapBtn
                        }
                    }
                }
            };

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
            };
		}

        private void ShowOnMapBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
            App.mainPage.OpenMapAt(50, 50);
        }
    }
}
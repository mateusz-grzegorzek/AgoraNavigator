using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AgoraNavigator.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimplePopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private Label labelTitle;
        private Label labelBody;
        public Button buttonOk;

        public Color ColorBackground = Color.White;
        public Color ColorTitle = Color.Black;
        public Color ColorBody = Color.Black;
        public Color ColorButtonBackground = Color.White;
        public Color ColorButtonBorder = Color.Black;
        public Color ColorButtonText = Color.Black;

        public SimplePopup(string title, string body)
        {
            InitializeComponent();

            labelTitle = new Label
            {
                Text = title,
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Center
            };
            labelBody = new Label
            {
                Text = body,
                FontFamily = AgoraFonts.GetPoppinsMedium(),
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            buttonOk = new Button
            {
                Text = "OK",
                FontFamily = AgoraFonts.GetPoppinsMedium(),
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.End
            };
            buttonOk.Clicked += buttonOk_Clicked;

            AbsoluteLayout.SetLayoutBounds(labelTitle, new Rectangle(.5,.05, .9, .25));
            AbsoluteLayout.SetLayoutBounds(labelBody,  new Rectangle(.5,.45, .9, .50));
            AbsoluteLayout.SetLayoutBounds(buttonOk,   new Rectangle(.5,.95,.45, .25));

            AbsoluteLayout.SetLayoutFlags(labelTitle, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(labelBody, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(buttonOk, AbsoluteLayoutFlags.All);

            layout.Children.Add(labelTitle);
            layout.Children.Add(labelBody);
            layout.Children.Add(buttonOk);
        }

        public void SetColors()
        {
            layout.BackgroundColor = ColorBackground;

            labelTitle.TextColor = ColorTitle;
            labelBody.TextColor = ColorBody;

            buttonOk.TextColor = ColorButtonText;
            buttonOk.BackgroundColor = ColorButtonBackground;
            buttonOk.BorderColor = ColorButtonBorder;
        }

        private async void buttonOk_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.RemovePageAsync(this);
        }
    }
}
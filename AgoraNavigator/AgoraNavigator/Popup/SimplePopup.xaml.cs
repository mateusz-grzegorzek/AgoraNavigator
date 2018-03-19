using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AgoraNavigator.Popup
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SimplePopup : Rg.Plugins.Popup.Pages.PopupPage
	{
        public Color ColorBackground = Color.BlueViolet;
        public Color ColorTitle = Color.Black;
        public Color ColorBody = Color.Black;
        public Color ColorButtonBackground = Color.White;
        public Color ColorButtonBorder = Color.Black;
        public Color ColorButtonText = Color.Black;

		public SimplePopup (string title, string body)
		{
            InitializeComponent();
            labelTitle.Text = title;
            labelBody.Text = body;
            buttonOk.Text = "OK";
        }

        public void SetColors()
        {
            stackLayout.BackgroundColor = ColorBackground;
            labelTitle.TextColor = ColorTitle; 
            labelBody.TextColor = ColorBody;
            buttonOk.Text = "OK";
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
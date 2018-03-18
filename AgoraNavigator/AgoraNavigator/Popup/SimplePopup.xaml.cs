using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AgoraNavigator.Popup
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SimplePopup : Rg.Plugins.Popup.Pages.PopupPage
	{
        public Color ColorBackground = Color.White;
        public Color ColorTitle = Color.Black;
        public Color ColorBody = Color.Black;
        public Color ColorButtonBackground = Color.White;
        public Color ColorButtonBorder = Color.Black;
        public Color ColorButtonText = Color.Black;

		public SimplePopup (string title, string body)
		{
            InitializeComponent();

            stackLayout.BackgroundColor = ColorBackground;

            labelTitle.Text = title;
            labelTitle.TextColor = ColorTitle;

            labelBody.Text = body;
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
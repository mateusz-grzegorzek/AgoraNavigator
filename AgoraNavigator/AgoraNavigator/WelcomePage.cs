using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AgoraNavigator
{
	public class WelcomePage : NavigationPage
	{
		public WelcomePage ()
		{
            Navigation.PushAsync(new StartingPage());
      }
	}
}
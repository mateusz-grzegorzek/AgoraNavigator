Instrukcja postawienia projektu:
1. 
Zainstalowa� pakiety SDK tak jak na obrazkach z folderu SdkSettingsPictures
2. 
Po otwarciu Rozwi�zania upewni� si�, �e zainstalowane s� nast�puj�ce pakiety NuGet na Solucji:
a) Xamarin.Forms - 2.4.0.282
b) Plugin.Permissions - 1.2.1
c) Xamarin.Forms.GoogleMaps - 1.8.1
d) Com.OneSignal - 3.0.1
e) Rg.Plugins.Popup - 1.0.4
Inne wersje tych pakiet�w mog� powodowa� b��dy kompilacji!
3. 
Pozyska� SHA-1 st�d:
https://developer.xamarin.com/guides/android/platform_features/maps_and_location/maps/obtaining_a_google_maps_api_key/
i podes�a� mi
4. Skompilowa�, wgra� i modli� si� �eby dzia�a�o ;)


### Powiadomienia lokalne (z kodu)

	DependencyService.Get<INotification>().Notify("Title", "Message");


### Custom popups (ex. SimplePopup)
	SimplePopup popup = new SimplePopup("title", "content")
	{
		// options
		ColorBackground = Color.White
		// ...
	};
	Navigation.PushPopupAsync(popup);
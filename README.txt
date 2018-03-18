Instrukcja postawienia projektu:
1. 
Zainstalowaæ pakiety SDK tak jak na obrazkach z folderu SdkSettingsPictures
2. 
Po otwarciu Rozwi¹zania upewniæ siê, ¿e zainstalowane s¹ nastêpuj¹ce pakiety NuGet na Solucji:
a) Xamarin.Forms - 2.4.0.282
b) Plugin.Permissions - 1.2.1
c) Xamarin.Forms.GoogleMaps - 1.8.1
d) Com.OneSignal - 3.0.1
e) Rg.Plugins.Popup - 1.0.4
Inne wersje tych pakietów mog¹ powodowaæ b³êdy kompilacji!
3. 
Pozyskaæ SHA-1 st¹d:
https://developer.xamarin.com/guides/android/platform_features/maps_and_location/maps/obtaining_a_google_maps_api_key/
i podes³aæ mi
4. Skompilowaæ, wgraæ i modliæ siê ¿eby dzia³a³o ;)


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
Instrukcja postawienia projektu:
1. 
Zainstalowa� pakiety SDK tak jak na obrazkach z folderu SdkSettingsPictures
2. 
Zainstalowa� pakiety NuGet takie jak w zdefiniowane w pliku AgoraNavigator.Android.csproj/AgoraNavigator.iOS.csproj
3. 
Pozyska� SHA-1 st�d:
https://developer.xamarin.com/guides/android/platform_features/maps_and_location/maps/obtaining_a_google_maps_api_key/
i podes�a� mi

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
Instrukcja postawienia projektu:
1. 
Zainstalowaæ pakiety SDK tak jak na obrazkach z folderu SdkSettingsPictures
2. 
Zainstalowaæ pakiety NuGet takie jak w zdefiniowane w pliku AgoraNavigator.Android.csproj/AgoraNavigator.iOS.csproj
3. 
Pozyskaæ SHA-1 st¹d:
https://developer.xamarin.com/guides/android/platform_features/maps_and_location/maps/obtaining_a_google_maps_api_key/
i podes³aæ mi

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
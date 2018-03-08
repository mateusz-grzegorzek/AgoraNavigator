using AgoraNavigator.Menu;
using System;
using Xamarin.Forms;
using Firebase.Database;
using Newtonsoft.Json;
using Plugin.FirebasePushNotification;

namespace AgoraNavigator.Login
{
    class StartingPage : NavigationPage
    {
        WelcomePage welcomePage;

        public StartingPage()
        {
            Console.WriteLine("StartingPage");
            Users.InitUsers();
            welcomePage = new WelcomePage();
            Navigation.PushAsync(welcomePage);
        }
    }

    public class WelcomePage : ContentPage
    {
        public static MainPage mainPage;

        public WelcomePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            Label welcomeLabel = new Label {Text = "Welcome in Agora Navigator!"};

            Button welcomeButton = new Button{Text = "Enter Agora Navigator!"};
            welcomeButton.Clicked += OnWelcomeButtonClickedAsync;
            welcomeButton.BackgroundColor = Color.Transparent;
            welcomeButton.TextColor = Color.Blue;
            welcomeButton.BorderWidth = 1;
            welcomeButton.BorderColor = Color.Black;

            AbsoluteLayout.SetLayoutBounds(welcomeLabel, new Rectangle(.5, .1, .5, .1));
            AbsoluteLayout.SetLayoutBounds(welcomeButton, new Rectangle(.5, .9, .25, .2));
            AbsoluteLayout.SetLayoutFlags(welcomeLabel, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutFlags(welcomeButton, AbsoluteLayoutFlags.All);

            AbsoluteLayout simpleLayout = new AbsoluteLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            simpleLayout.Children.Add(welcomeLabel);
            simpleLayout.Children.Add(welcomeButton);
            Content = simpleLayout;
        }
        
        public async void OnWelcomeButtonClickedAsync(object sender, EventArgs e)
        {
            Console.WriteLine("OnWelcomeButtonClicked");
            var firebaseClient = new FirebaseClient(Configuration.FirebaseEndpoint);

            //FirebaseMessaging.Instance.SubscribeToTopic("AEGEE_Army");
            CrossFirebasePushNotification.Current.Subscribe("AEGEE_Army");
            Message msg = new Message
            {
                userId = Users.users[0].Id,
                antenaId = Users.users[0].AntenaId,
                taskId = 3,
                body = ""
            };
            String jMsg = JsonConvert.SerializeObject(msg);
            String path = "tasks/3/" + Users.users[0].Id;
            await firebaseClient.Child(path).PutAsync("2");
            //mainPage = new MainPage();
            //Navigation.PushAsync(mainPage);
        }
    }
}
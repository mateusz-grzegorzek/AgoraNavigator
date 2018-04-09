using Plugin.SimpleAudioPlayer.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace AgoraNavigator.Info
{
    public class BonusInfoPage : NavigationPage
    {
        public static BonusInfoMasterPage bonusInfoMasterPage;

        public BonusInfoPage()
        {
            BarTextColor = AgoraColor.Blue;
            bonusInfoMasterPage = new BonusInfoMasterPage();
            Navigation.PushAsync(bonusInfoMasterPage);
        }
    }

    public class TopicLabel : Label
    {
        public TopicLabel(string text)
        {
            Text = text;
            TextColor = AgoraColor.DarkBlue;
            FontFamily = AgoraFonts.GetPoppinsBold();
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));
            VerticalOptions = LayoutOptions.Center;
        }
    }

    public class EngPhraseLabel : Label
    {
        public EngPhraseLabel(string text)
        {
            Text = text;
            TextColor = AgoraColor.Blue;
            FontFamily = AgoraFonts.GetPoppinsRegular();
            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
        }
    }

    public class PolPhraseLabel : Label
    {
        public PolPhraseLabel(string text)
        {
            Text = text;
            TextColor = AgoraColor.Blue;
            FontFamily = AgoraFonts.GetPoppinsBold();
            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
        }
    }

    public class SpeakerImage : Image
    {
        public string audioFileName;
        public SpeakerImage(string _audioFileName)
        {
            audioFileName = _audioFileName;
            Source = "Speaker_Icon.png";
            VerticalOptions = LayoutOptions.Start;

            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += OnTapGestureRecognizerTapped;
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            SpeakerImage speakerImage = (SpeakerImage)sender;
            ISimpleAudioPlayer player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            try
            {
                Stream stream = GetStreamFromFile(speakerImage.audioFileName);
                player.Load(stream);
                player.Play();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }
        }

        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("AgoraNavigator.Droid.Audio." + filename);

            return stream;
        }
    }

    public class PhraseStackLayout : StackLayout
    {
        public PhraseStackLayout(string engText, string polText)
        {
            Orientation = StackOrientation.Horizontal;
            EngPhraseLabel engPhraseLabel = new EngPhraseLabel(engText);
            PolPhraseLabel polPhraseLabel = new PolPhraseLabel(polText);
            Children.Add(engPhraseLabel);
            Children.Add(polPhraseLabel);
            Margin = new Thickness(0, 0);
            VerticalOptions = LayoutOptions.Center;
        }
    }

    public class BonusInfoMasterPage : ContentPage
    {
        StackLayout layout;

        public void AddPhraseToLayout(string engText, string polText, bool audioEnabled = true)
        {
            Grid grid = new Grid
            {
                Margin = new Thickness(0, 0),
                RowSpacing = 5
            };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
            PhraseStackLayout phraseStackLayout = new PhraseStackLayout(engText, polText);
            
            Image separator = new Image
            {
                Source = "Contact_Separator.png",
                VerticalOptions = LayoutOptions.End,
            };
            Grid.SetColumnSpan(separator, 2);
            grid.Children.Add(phraseStackLayout, 0, 0);
            if(audioEnabled)
            {
                SpeakerImage speakerImage = new SpeakerImage("Czesc.mp3");
                grid.Children.Add(speakerImage, 1, 0);
            }
            grid.Children.Add(separator, 0, 2, 0, 1);
            layout.Children.Add(grid);
        }

        public BonusInfoMasterPage()
        {
            Title = "Bonus info";
            layout = new StackLayout
            {
                Margin = new Thickness(10, 5)
            };

            Label topLabel = new Label
            {
                Text = "Usefull Polish phrases",
                TextColor = AgoraColor.DarkBlue,
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
                HorizontalOptions = LayoutOptions.Center
            };
            layout.Children.Add(topLabel);

            TopicLabel topicLabel1 = new TopicLabel("Polite phrases");
            layout.Children.Add(topicLabel1);

            AddPhraseToLayout("Please - ", "Proszę");
            AddPhraseToLayout("Thank you - ", "Dziękuję ");
            AddPhraseToLayout("I’m sorry - ", "Przepraszam");
            AddPhraseToLayout("Good morning - ", "Dzień dobry");
            AddPhraseToLayout("Good evening - ", "Dobry wieczór");
            AddPhraseToLayout("Good night - ", "Dobranoc");
            AddPhraseToLayout("Hi/Bye - ", "Cześć");
            AddPhraseToLayout("See you/Goodbye - ", "Do widzenia");
            AddPhraseToLayout("Yes - ", "Tak ");
            AddPhraseToLayout("No - ", "Nie ");

            TopicLabel topicLabel2 = new TopicLabel("Dining out");
            layout.Children.Add(topicLabel2);

            AddPhraseToLayout("Restaurant - ", "Restauracja");
            AddPhraseToLayout("Dinner - ", "Obiad");
            AddPhraseToLayout("Wine - ", "Wino");
            AddPhraseToLayout("Beer - ", "Piwo");
            AddPhraseToLayout("Vegetarian dish - ", "Danie wegetarianskie");
            AddPhraseToLayout("Can we have the bill please? - ", "Proszę o rachunek");

            TopicLabel topicLabel3 = new TopicLabel("Shopping");
            layout.Children.Add(topicLabel3);

            AddPhraseToLayout("Price  - ", "Cena");
            AddPhraseToLayout("Do you accept credit cards? - ", "Czy mogę zaplacić kartą?");
            AddPhraseToLayout("How much for this? - ", "Ile to kosztuje?");

            TopicLabel topicLabel4 = new TopicLabel("Travelling");
            layout.Children.Add(topicLabel4);

            AddPhraseToLayout("Airport - ", "Lotnisko");
            AddPhraseToLayout("Train - ", "Pociąg");
            AddPhraseToLayout("Train station - ", "Dworzec kolejowy");
            AddPhraseToLayout("Bus station - ", "Dworzec autobusowy");
            AddPhraseToLayout("One ticket to ... - ", "Bilet do …");

            TopicLabel topicLabel5 = new TopicLabel("Directions");
            layout.Children.Add(topicLabel5);

            AddPhraseToLayout("Street  - ", "Ulica");
            AddPhraseToLayout("Square  - ", "Plac");
            AddPhraseToLayout("How can I get to ... - ", "Jak moge dojść do ...");
            AddPhraseToLayout("Right/left  - ", "Prawo/lewo");

            TopicLabel topicLabel6 = new TopicLabel("Nightlife");
            layout.Children.Add(topicLabel6);

            AddPhraseToLayout("Cheers! - ", "Na zdrowie!");
            AddPhraseToLayout("One beer please - ", "Jedno piwo proszę");
            AddPhraseToLayout("Can I smoke here? - ", "Czy można tu palić?");     
            AddPhraseToLayout("Where are the toilets? - ", "Gdzie jest toaleta?");
            AddPhraseToLayout("My name is ... - ", "Mam na imię ...");

            TopicLabel topicLabel7 = new TopicLabel("Intresting facts about Kraków");
            layout.Children.Add(topicLabel7);

            AddPhraseToLayout("- ", "Poland's currency is zloty (PLN). It is divided into one hundred smaller units called grosz.", false);
            AddPhraseToLayout("- ", "According to Polish legend, the city is protected by a mighty dragon.", false);
            AddPhraseToLayout("- ", "Krakow’s historical city centre is a UNESCO world heritage site.", false);
            AddPhraseToLayout(
                "- ", "Big head sculpture It’s behind the Sukiennice at Rynek. " +
                "For locals it’s often meeting point. What exactly this sculpture presents? " +
                "It’s Eros Bendato, head of love god. Why has he band on his eyes? " +
                "Maybe because the love is blind…?", false);
            AddPhraseToLayout(
                "- ", "In Wawel Castle, there is an exhibition of one painting." +
                " It is the original painting by Leonardo da Vinci “Lady with an Ermine.” " +
                "Only five other cities in the world can boast a painting by da Vinci, " +
                "so don’t even think about letting this opportunity pass by.", false);
            AddPhraseToLayout(
                "- ", "Krakow has its analog of the Tower of Pisa—the tower of the Town Hall. " +
                "Although it leaned from its base by only 55 cm, because of the high height of the 70-meter tower, the deviation is well marked.", false);

            Content = new ScrollView { Content = layout };
        }
    }
}
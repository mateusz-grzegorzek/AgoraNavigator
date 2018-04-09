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
        Grid grid;
        int rowNumber = 0;

        public void AddPhraseToGrid(string engText, string polText)
        {
            PhraseStackLayout phraseStackLayout = new PhraseStackLayout(engText, polText);
            SpeakerImage speakerImage = new SpeakerImage("Czesc.mp3");
            Image separator = new Image
            {
                Source = "Contact_Separator.png",
                VerticalOptions = LayoutOptions.End
            };
            Grid.SetColumnSpan(separator, 2);
            grid.Children.Add(phraseStackLayout, 0, rowNumber);
            grid.Children.Add(speakerImage, 1, rowNumber);
            grid.Children.Add(separator, 0, 2, rowNumber, rowNumber+1);
            rowNumber++;
        }

        public BonusInfoMasterPage()
        {
            Title = "Bonus info";

            grid = new Grid
            {
                Margin = new Thickness(10, 5),
                RowSpacing = 5            
            };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Label topLabel = new Label
            {
                Text = "Usefull Polish phrases",
                TextColor = AgoraColor.DarkBlue,
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
                HorizontalOptions = LayoutOptions.Center
            };
            grid.Children.Add(topLabel, 0, rowNumber++);

            TopicLabel topicLabel1 = new TopicLabel("Polite phrases");
            grid.Children.Add(topicLabel1, 0, rowNumber++);

            AddPhraseToGrid("Please - ", "Proszę");
            AddPhraseToGrid("Thank you - ", "Dziękuję ");
            AddPhraseToGrid("I’m sorry - ", "Przepraszam");
            AddPhraseToGrid("Good morning - ", "Dzień dobry");
            AddPhraseToGrid("Good evening - ", "Dobry wieczór");
            AddPhraseToGrid("Good night - ", "Dobranoc");
            AddPhraseToGrid("Hi/Bye - ", "Cześć");
            AddPhraseToGrid("See you/Goodbye - ", "Do widzenia");
            AddPhraseToGrid("Yes - ", "Tak ");
            AddPhraseToGrid("No - ", "Nie ");

            TopicLabel topicLabel2 = new TopicLabel("Dining out");
            grid.Children.Add(topicLabel2, 0, rowNumber++);

            AddPhraseToGrid("Restaurant - ", "Restauracja");
            AddPhraseToGrid("Dinner - ", "Obiad");
            AddPhraseToGrid("Wine - ", "Wino");
            AddPhraseToGrid("Beer - ", "Piwo");
            AddPhraseToGrid("Vegetarian dish - ", "Danie wegetarianskie");
            AddPhraseToGrid("Can we have the bill please? - ", "Proszę o rachunek");

            TopicLabel topicLabel3 = new TopicLabel("Shopping");
            grid.Children.Add(topicLabel3, 0, rowNumber++);

            AddPhraseToGrid("Price  - ", "Cena");
            AddPhraseToGrid("Do you accept credit cards? - ", "Czy mogę zaplacić kartą?");
            AddPhraseToGrid("How much for this? - ", "Ile to kosztuje?");

            TopicLabel topicLabel4 = new TopicLabel("Travelling");
            grid.Children.Add(topicLabel4, 0, rowNumber++);

            AddPhraseToGrid("Airport - ", "Lotnisko");
            AddPhraseToGrid("Train - ", "Pociąg");
            AddPhraseToGrid("Train station - ", "Dworzec kolejowy");
            AddPhraseToGrid("Bus station - ", "Dworzec autobusowy");
            AddPhraseToGrid("One ticket to ... - ", "Bilet do …");

            TopicLabel topicLabel5 = new TopicLabel("Directions");
            grid.Children.Add(topicLabel5, 0, rowNumber++);

            AddPhraseToGrid("Street  - ", "Ulica");
            AddPhraseToGrid("Square  - ", "Plac");
            AddPhraseToGrid("How can I get to ... - ", "Jak moge dojść do ...");
            AddPhraseToGrid("Right/left  - ", "Prawo/lewo");

            TopicLabel topicLabel6 = new TopicLabel("Nightlife");
            grid.Children.Add(topicLabel6, 0, rowNumber++);

            AddPhraseToGrid("Cheers! - ", "Na zdrowie!");
            AddPhraseToGrid("One beer please - ", "Jedno piwo proszę");
            AddPhraseToGrid("Can I smoke here? - ", "Czy można tu palić?");     
            AddPhraseToGrid("Where are the toilets? - ", "Gdzie jest toaleta?");
            AddPhraseToGrid("My name is ... - ", "Mam na imię ...");

            TopicLabel topicLabel7 = new TopicLabel("Intresting facts about Kraków");
            grid.Children.Add(topicLabel7, 0, rowNumber++);

            AddPhraseToGrid("- ", "Poland's currency is zloty (PLN). It is divided into one hundred smaller units called grosz.");
            AddPhraseToGrid("- ", "According to Polish legend, the city is protected by a mighty dragon.");
            AddPhraseToGrid("- ", "Krakow’s historical city centre is a UNESCO world heritage site.");
            AddPhraseToGrid(
                "- ", "Big head sculpture It’s behind the Sukiennice at Rynek. " +
                "For locals it’s often meeting point. What exactly this sculpture presents? " +
                "It’s Eros Bendato, head of love god. Why has he band on his eyes? " +
                "Maybe because the love is blind…?");
            AddPhraseToGrid(
                "- ", "In Wawel Castle, there is an exhibition of one painting." +
                " It is the original painting by Leonardo da Vinci “Lady with an Ermine.” " +
                "Only five other cities in the world can boast a painting by da Vinci, " +
                "so don’t even think about letting this opportunity pass by.");
            AddPhraseToGrid(
                "- ", "Krakow has its analog of the Tower of Pisa—the tower of the Town Hall. " +
                "Although it leaned from its base by only 55 cm, because of the high height of the 70-meter tower, the deviation is well marked.");

            Content = new ScrollView { Content = grid };
        }
    }
}
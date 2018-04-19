using Plugin.SimpleAudioPlayer.Abstractions;
using System;
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

        public void AddPhraseToLayout(string engText, string polText, string audioFileName, bool audioEnabled = true)
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
                SpeakerImage speakerImage = new SpeakerImage(audioFileName);
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

            AddPhraseToLayout("Please - ", "Proszę", "Prosze.mp3");
            AddPhraseToLayout("Thank you - ", "Dziękuję", "Dziekuje.mp3");
            AddPhraseToLayout("I’m sorry - ", "Przepraszam", "Przepraszam.mp3");
            AddPhraseToLayout("Good morning - ", "Dzień dobry", "Dzien_dobry.mp3");
            AddPhraseToLayout("Good evening - ", "Dobry wieczór", "Dobry_wieczor.mp3");
            AddPhraseToLayout("Good night - ", "Dobranoc", "Dobranoc.mp3");
            AddPhraseToLayout("Hi/Bye - ", "Cześć", "Czesc.mp3");
            AddPhraseToLayout("See you/Goodbye - ", "Do widzenia", "Do_widzenia.mp3");
            AddPhraseToLayout("Yes - ", "Tak", "Tak.mp3");
            AddPhraseToLayout("No - ", "Nie", "Nie.mp3");

            TopicLabel topicLabel2 = new TopicLabel("Dining out");
            layout.Children.Add(topicLabel2);

            AddPhraseToLayout("Restaurant - ", "Restauracja", "Restauracja.mp3");
            AddPhraseToLayout("Dinner - ", "Obiad", "Obiad.mp3");
            AddPhraseToLayout("Wine - ", "Wino", "Wino.mp3");
            AddPhraseToLayout("Beer - ", "Piwo", "Piwo.mp3");
            AddPhraseToLayout("Vegetarian dish - ", "Danie wegetarianskie", "Danie_wegetarianskie.mp3");
            AddPhraseToLayout("Can we have the bill please? - ", "Proszę o rachunek", "Prosze_o_rachunek.mp3");

            TopicLabel topicLabel3 = new TopicLabel("Shopping");
            layout.Children.Add(topicLabel3);

            AddPhraseToLayout("Price  - ", "Cena", "Cena.mp3");
            AddPhraseToLayout("Do you accept credit cards? - ", "Czy mogę zaplacić kartą?", "Czy_moge_zaplacic_karta.mp3");
            AddPhraseToLayout("How much for this? - ", "Ile to kosztuje?", "Ile_to_kosztuje.mp3");

            TopicLabel topicLabel4 = new TopicLabel("Travelling");
            layout.Children.Add(topicLabel4);

            AddPhraseToLayout("Airport - ", "Lotnisko", "Lotnisko.mp3");
            AddPhraseToLayout("Train - ", "Pociąg", "Pociąg.mp3");
            AddPhraseToLayout("Train station - ", "Dworzec kolejowy", "Dworzec_kolejowy.mp3");
            AddPhraseToLayout("Cracow Gallery - ", "Galeria krakowska", "Galeria_krakowska.mp3");
            AddPhraseToLayout("Krakus Mound - ", "Kopiec Kraka", "Kopiec_kraka.mp3");
            AddPhraseToLayout("Wisła Stadium - ", "Stadion Wisły", "Stadion_wisly.mp3");
            AddPhraseToLayout("Student town - ", "Miasteczko Studenckie", "Miasteczko_studenckie.mp3");

            TopicLabel topicLabel5 = new TopicLabel("Directions");
            layout.Children.Add(topicLabel5);

            AddPhraseToLayout("Krupnicza Street  - ", "Ulica Krupnicza", "Ulica_krupnicza.mp3");
            AddPhraseToLayout("Wolnica Square  - ", "Plac Wolnica", "Plac_wolnica.mp3");
            AddPhraseToLayout("How can I get to ... - ", "Jak moge dojść do ...", "Jak_moge_dojsc_do.mp3");
            AddPhraseToLayout("Right/left  - ", "Prawo/lewo", "Prawo_lewo.mp3");

            TopicLabel topicLabel6 = new TopicLabel("Nightlife");
            layout.Children.Add(topicLabel6);

            AddPhraseToLayout("Cheers! - ", "Na zdrowie!", "Na_zdrowie.mp3");
            AddPhraseToLayout("One beer please - ", "Jedno piwo proszę", "Jedno_piwo_prosze.mp3");
            AddPhraseToLayout("Half a liter of Soplica, please - ", "Poproszę pół litra Soplicy", "Poprosze_pol_litra_soplicy.mp3");
            AddPhraseToLayout("Can I smoke here? - ", "Czy można tu palić?", "Czy_mozna_tu_palic.mp3");     
            AddPhraseToLayout("Where are the toilets? - ", "Gdzie jest toaleta?", "Gdzie_jest_toaleta.mp3");
            AddPhraseToLayout("My name is ... - ", "Mam na imię ...", "Mam_na_imie.mp3");
            AddPhraseToLayout("Where I can find nice girls? - ", "Gdzie znajdę fajne dziewczyny?", "Gdzie_znajde_fajne_dziewczyny.mp3");
            AddPhraseToLayout("Where I can find handsome guys? - ", "Gdzie znajdę przystojnych facetów?", "Gdzie_znajde_przystojnych_facetow.mp3");

            TopicLabel topicLabel7 = new TopicLabel("Intresting facts about Kraków");
            layout.Children.Add(topicLabel7);

            AddPhraseToLayout("- ", "Poland's currency is zloty (PLN). It is divided into one hundred smaller units called grosz.", "", false);
            AddPhraseToLayout("- ", "According to Polish legend, the city is protected by a mighty dragon.", "", false);
            AddPhraseToLayout("- ", "Krakow’s historical city centre is a UNESCO world heritage site.", "", false);
            AddPhraseToLayout(
                "- ", "Big head sculpture It’s behind the Sukiennice at Rynek. " +
                "For locals it’s often meeting point. What exactly this sculpture presents? " +
                "It’s Eros Bendato, head of love god. Why has he band on his eyes? " +
                "Maybe because the love is blind…?", "", false);
            AddPhraseToLayout(
                "- ", "In National Museum, there is an exhibition of one painting." +
                " It is the original painting by Leonardo da Vinci “Lady with an Ermine.” " +
                "Only five other cities in the world can boast a painting by da Vinci, " +
                "so don’t even think about letting this opportunity pass by.", "", false);
            AddPhraseToLayout(
                "- ", "Krakow has its analog of the Tower of Pisa—the tower of the Town Hall. " +
                "Although it leaned from its base by only 55 cm, because of the high height of the 70-meter tower, the deviation is well marked.", "", false);

            Content = new ScrollView { Content = layout };
        }
    }
}

using System.Collections.Generic;
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
            FontFamily = AgoraFonts.GetPoppinsMedium();
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));
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
        }
    }

    public class BonusInfoMasterPage : ContentPage
    {
        public BonusInfoMasterPage()
        {
            Title = "Bonus info";
            Label topLabel = new Label
            {
                Text = "Usefull Polish phrases",
                TextColor = AgoraColor.DarkBlue,
                FontFamily = AgoraFonts.GetPoppinsBold(),
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
                HorizontalOptions = LayoutOptions.Center
            };

            Image separator = new Image { Source = "Contact_Separator.png", VerticalOptions = LayoutOptions.Start };
            Image separator2 = new Image { Source = "Contact_Separator.png", VerticalOptions = LayoutOptions.Start };

            TopicLabel topicLabel1 = new TopicLabel("Polite phrases");
            List<PhraseStackLayout> phraseStackLayoutTopicList1 = new List<PhraseStackLayout>();
            phraseStackLayoutTopicList1.Add(new PhraseStackLayout("Please - ", "Proszę"));
            phraseStackLayoutTopicList1.Add(new PhraseStackLayout("Thank you - ", "Dziękuję "));
            phraseStackLayoutTopicList1.Add(new PhraseStackLayout("I’m sorry - ", "Przepraszam"));
            phraseStackLayoutTopicList1.Add(new PhraseStackLayout("Good morning - ", "Dzień dobry"));
            phraseStackLayoutTopicList1.Add(new PhraseStackLayout("Good evening - ", "Dobry wieczór"));
            phraseStackLayoutTopicList1.Add(new PhraseStackLayout("Good night - ", "Dobranoc"));
            phraseStackLayoutTopicList1.Add(new PhraseStackLayout("Hi/Bye - ", "Cześć"));
            phraseStackLayoutTopicList1.Add(new PhraseStackLayout("See you/Goodbye - ", "Do widzenia"));
            phraseStackLayoutTopicList1.Add(new PhraseStackLayout("Yes - ", "Tak "));
            phraseStackLayoutTopicList1.Add(new PhraseStackLayout("No - ", "Nie "));

            TopicLabel topicLabel2 = new TopicLabel("Dining out");
            List<PhraseStackLayout> phraseStackLayoutTopicList2 = new List<PhraseStackLayout>();
            phraseStackLayoutTopicList2.Add(new PhraseStackLayout("Restaurant - ", "Restauracja"));
            phraseStackLayoutTopicList2.Add(new PhraseStackLayout("Dinner - ", "Obiad"));
            phraseStackLayoutTopicList2.Add(new PhraseStackLayout("Wine - ", "Wino"));
            phraseStackLayoutTopicList2.Add(new PhraseStackLayout("Beer - ", "Piwo"));
            phraseStackLayoutTopicList2.Add(new PhraseStackLayout("Vegetarian dish - ", "Danie wegetarianskie"));
            phraseStackLayoutTopicList2.Add(new PhraseStackLayout("Can we have the bill please? - ", "Proszę o rachunek"));

            TopicLabel topicLabel3 = new TopicLabel("Shopping");
            List<PhraseStackLayout> phraseStackLayoutTopicList3 = new List<PhraseStackLayout>();
            phraseStackLayoutTopicList3.Add(new PhraseStackLayout("Price  - ", "Cena"));
            phraseStackLayoutTopicList3.Add(new PhraseStackLayout("Do you accept credit cards? - ", "Czy mogę zaplacić kartą?"));
            phraseStackLayoutTopicList3.Add(new PhraseStackLayout("How much for this? - ", "Ile to kosztuje?"));

            TopicLabel topicLabel4 = new TopicLabel("Travelling");
            List<PhraseStackLayout> phraseStackLayoutTopicList4 = new List<PhraseStackLayout>();
            phraseStackLayoutTopicList4.Add(new PhraseStackLayout("Airport - ", "Lotnisko"));
            phraseStackLayoutTopicList4.Add(new PhraseStackLayout("Train - ", "Pociąg"));
            phraseStackLayoutTopicList4.Add(new PhraseStackLayout("Airport  - ", "Lotnisko"));
            phraseStackLayoutTopicList4.Add(new PhraseStackLayout("Train station - ", "Dworzec kolejowy"));
            phraseStackLayoutTopicList4.Add(new PhraseStackLayout("Train station - ", "Dworzec kolejowy"));
            phraseStackLayoutTopicList4.Add(new PhraseStackLayout("Bus station - ", "Dworzec autobusowy"));
            phraseStackLayoutTopicList4.Add(new PhraseStackLayout("One ticket to ... - ", "Bilet do …"));

            TopicLabel topicLabel5 = new TopicLabel("Directions");
            List<PhraseStackLayout> phraseStackLayoutTopicList5 = new List<PhraseStackLayout>();
            phraseStackLayoutTopicList5.Add(new PhraseStackLayout("Street  - ", "Ulica"));
            phraseStackLayoutTopicList5.Add(new PhraseStackLayout("Square  - ", "Plac"));
            phraseStackLayoutTopicList5.Add(new PhraseStackLayout("How can I get to ... - ", "Jak moge dojść do ..."));
            phraseStackLayoutTopicList5.Add(new PhraseStackLayout("Right/left  - ", "Prawo/lewo"));

            TopicLabel topicLabel6 = new TopicLabel("Nightlife");
            List<PhraseStackLayout> phraseStackLayoutTopicList6 = new List<PhraseStackLayout>();
            phraseStackLayoutTopicList6.Add(new PhraseStackLayout("Cheers! - ", "Na zdrowie!"));
            phraseStackLayoutTopicList6.Add(new PhraseStackLayout("One beer please - ", "Jedno piwo proszę"));
            phraseStackLayoutTopicList6.Add(new PhraseStackLayout("Can I smoke here? - ", "Czy można tu palić?"));     
            phraseStackLayoutTopicList6.Add(new PhraseStackLayout("Where are the toilets? - ", "Gdzie jest toaleta?"));
            phraseStackLayoutTopicList6.Add(new PhraseStackLayout("My name is ... - ", "Mam na imię ..."));

            TopicLabel topicLabel7 = new TopicLabel("Intresting facts about Kraków");
            List<PhraseStackLayout> phraseStackLayoutTopicList7 = new List<PhraseStackLayout>();
            phraseStackLayoutTopicList7.Add(new PhraseStackLayout(
                "- ", "Poland's currency is zloty (PLN). It is divided into one hundred smaller units called grosz."));
            phraseStackLayoutTopicList7.Add(new PhraseStackLayout(
                "- ", "According to Polish legend, the city is protected by a mighty dragon."));
            phraseStackLayoutTopicList7.Add(new PhraseStackLayout(
                "- ", "Krakow’s historical city centre is a UNESCO world heritage site."));
            phraseStackLayoutTopicList7.Add(new PhraseStackLayout(
                "- ", "Big head sculpture It’s behind the Sukiennice at Rynek. " +
                "For locals it’s often meeting point. What exactly this sculpture presents? " +
                "It’s Eros Bendato, head of love god. Why has he band on his eyes? " +
                "Maybe because the love is blind…?"));
            phraseStackLayoutTopicList7.Add(new PhraseStackLayout(
                "- ", "In Wawel Castle, there is an exhibition of one painting." +
                " It is the original painting by Leonardo da Vinci “Lady with an Ermine.” " +
                "Only five other cities in the world can boast a painting by da Vinci, " +
                "so don’t even think about letting this opportunity pass by."));
            phraseStackLayoutTopicList7.Add(new PhraseStackLayout(
                "- ", "Krakow has its analog of the Tower of Pisa—the tower of the Town Hall. " +
                "Although it leaned from its base by only 55 cm, because of the high height of the 70-meter tower, the deviation is well marked."));

            StackLayout stackLayout = new StackLayout
            {
                Spacing = 0,
                Margin = new Thickness(5, 0)
            };

            stackLayout.Children.Add(topLabel);

            stackLayout.Children.Add(topicLabel1);
            foreach(PhraseStackLayout phraseStackLayout in phraseStackLayoutTopicList1)
            {
                stackLayout.Children.Add(new Image { Source = "Contact_Separator.png" });
                stackLayout.Children.Add(phraseStackLayout);
            }

            stackLayout.Children.Add(topicLabel2);
            foreach (PhraseStackLayout phraseStackLayout in phraseStackLayoutTopicList2)
            {
                stackLayout.Children.Add(new Image { Source = "Contact_Separator.png" });
                stackLayout.Children.Add(phraseStackLayout);
            }

            stackLayout.Children.Add(topicLabel3);
            foreach (PhraseStackLayout phraseStackLayout in phraseStackLayoutTopicList3)
            {
                stackLayout.Children.Add(new Image { Source = "Contact_Separator.png" });
                stackLayout.Children.Add(phraseStackLayout);
            }

            stackLayout.Children.Add(topicLabel4);
            foreach (PhraseStackLayout phraseStackLayout in phraseStackLayoutTopicList4)
            {
                stackLayout.Children.Add(new Image { Source = "Contact_Separator.png" });
                stackLayout.Children.Add(phraseStackLayout);
            }

            stackLayout.Children.Add(topicLabel5);
            foreach (PhraseStackLayout phraseStackLayout in phraseStackLayoutTopicList5)
            {
                stackLayout.Children.Add(new Image { Source = "Contact_Separator.png" });
                stackLayout.Children.Add(phraseStackLayout);
            }

            stackLayout.Children.Add(topicLabel6);
            foreach (PhraseStackLayout phraseStackLayout in phraseStackLayoutTopicList6)
            {
                stackLayout.Children.Add(new Image { Source = "Contact_Separator.png" });
                stackLayout.Children.Add(phraseStackLayout);
            }

            stackLayout.Children.Add(topicLabel7);
            foreach (PhraseStackLayout phraseStackLayout in phraseStackLayoutTopicList7)
            {
                stackLayout.Children.Add(new Image { Source = "Contact_Separator.png" });
                stackLayout.Children.Add(phraseStackLayout);
            }

            CompressedLayout.SetIsHeadless(stackLayout, true);
            Content = new ScrollView { Content = stackLayout };
        }
    }
}
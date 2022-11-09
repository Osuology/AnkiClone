using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnkiClone
{
    public class Deck
    {
        private List<Card> cards;
        private int cardID = 0;

        public Deck()
        {
            cards = new List<Card>() { new Card() { Text = "Apple", Answers = new List<String>() { "ябпоко" } },
                new Card() { Text = "Coffee", Answers = new List<String>() { "кофе" } }};
            PickRandomCard();
        }

        public List<Card> GetCards()
        {
            return cards;
        }

        public Card GetCard(int index)
        {
            return GetCards()[index];
        }

        public Card CurrentCard()
        {
            return GetCard(cardID);
        }

        public void PickRandomCard(bool replacement = false)
        {
            Card card = CurrentCard();
            GetCards().RemoveAt(cardID);
            Random rand = new Random();
            cardID = rand.Next() % GetCards().Count;
            GetCards().Add(card);
        }

        public void SetCards(List<Card> newCards)
        {
            cards = newCards;
        }
    }

    public class Card
    {
        private int ID;
        public String Text { get; set; }
        public List<String> Answers { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Deck deck;
        public MainWindow()
        {
            deck = new Deck();
            InitializeComponent();
            PickRandomCard();
        }

        public void PickRandomCard()
        {
            deck.PickRandomCard();
            cardLabel.Content = deck.CurrentCard().Text;
        }

        private void submitClicked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Checked.");
            if (deck.CurrentCard().Answers.Contains(cardInput.Text.ToLower()))
            {
                cardInput.Text = "";
                PickRandomCard();
                Console.WriteLine("Success!!");
            }
        }

        private void inputKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                submitClicked(sender, e);
            }
        }

        private void newCardClick(object sender, RoutedEventArgs e)
        {
            DeckWindow deckWindow = new DeckWindow();
            deckWindow.Show();
            deckWindow.InitDeck(ref deck);
        }
    }
}

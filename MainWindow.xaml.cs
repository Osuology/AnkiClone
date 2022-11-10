using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        public List<Card> Cards { get { return GetCards();  } set { SetCards(value); } }
        public int CardID { get { return GetCardID();  } set { SetCardID(value); } }

        private List<Card> cards;
        private int cardID = 0;

        public Deck()
        {
            cards = new List<Card>() { new Card("Apple") { Answers = new List<String>() { "ябпоко" } },
                new Card("Coffee") { Answers = new List<String>() { "кофе" } }};
            PickRandomCard();
        }

        public List<Card> GetCards()
        {
            return cards;
        }

        public int GetCardID()
        {
            return cardID;
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

        public void SetCardID(int id)
        {
            cardID = id;
        }

        public void SetCard(int index, Card card)
        {
            cards[index] = card;
        }

        public void AddCard(Card card)
        {
            var cards = GetCards();
            cards.Add(card);
            SetCards(cards);
        }

        public void SaveDeckTo(String path)
        {
            var options = new JsonSerializerOptions { IncludeFields = true };
            var jsonString = JsonSerializer.Serialize(this, options);
            using (StreamWriter file = new(path))
            {
                file.WriteLine(jsonString);
            }
        }

        public void LoadFrom(String path)
        {
            using (StreamReader file = new(path))
            {
                var jsonString = file.ReadToEnd();
                var deck = JsonSerializer.Deserialize<Deck>(jsonString);
                this.cards = deck.cards;
                this.cardID = deck.cardID;
            }
        }
    }

    public class Card
    {
        private int ID;
        public String Text { get; set; }
        public List<String> Answers { get; set; }

        public Card(string text = "New Card")
        {
            Text = text;
            Answers = new List<string>();
        }
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

        public void Update()
        {
            cardTextBlock.Text = deck.CurrentCard().Text;
        }

        public void PickRandomCard()
        {
            deck.PickRandomCard();
            cardTextBlock.Text = deck.CurrentCard().Text;
        }

        private void submitClicked(object sender, RoutedEventArgs e)
        {
            if (deck.CurrentCard().Answers.Contains(cardInput.Text.ToLower()))
            {
                cardInput.Text = "";
                PickRandomCard();
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
            deckWindow.Owner = this;
        }

        private void saveDeck(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                deck.SaveDeckTo(saveFileDialog.FileName);
        }

        private void loadDeckClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                deck.LoadFrom(openFileDialog.FileName);
            }
        }
    }
}

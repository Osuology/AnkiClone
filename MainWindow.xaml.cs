﻿using Microsoft.Win32;
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

namespace FlashcardMemorizer
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
        public const string RECENTS_PATH = "recents.json";
        
        Deck deck;
        List<string> recentDecks;
        public MainWindow()
        {
            deck = new Deck();
            InitializeComponent();
            LoadRecentDecks();
            PickRandomCard();
        }

        public void LoadRecentDecks()
        {
            recentDecks = new List<string>();
            if (File.Exists(RECENTS_PATH)) {
                using (StreamReader sr = new(RECENTS_PATH))
                {
                    string jsonText = sr.ReadToEnd();
#pragma warning disable CS8601 // Possible null reference assignment.
                    recentDecks = JsonSerializer.Deserialize<List<string>>(jsonText);
#pragma warning restore CS8601 // Possible null reference assignment.
                    if (recentDecks == null)
                        recentDecks = new List<string>();
                    else
                    {
                        // add menuitem's
                        foreach (string path in recentDecks)
                        {
                            var item = new MenuItem();
                            item.Header = path;
                            item.Click += loadRecentDeck;
                            if (recentDeckMenu.Items.Count < 100)
                            {
                                recentDeckMenu.Items.Add(item);
                            }
                        }
                    }
                }
            }
            else
            {
                File.Create(RECENTS_PATH);
            }
        }

        public void UpdateRecentDecks(string path)
        {
            if (!recentDecks.Contains(path))
            {
                recentDecks.Add(path);
                using (StreamWriter sw = new StreamWriter(RECENTS_PATH))
                {
                    string jsonText = JsonSerializer.Serialize(recentDecks);
                    sw.Write(jsonText);
                }
                
                recentDeckMenu.Items.Clear();
                foreach (string deckPath in recentDecks)
                {
                    var item = new MenuItem();
                    item.Header = deckPath;
                    item.Click += loadRecentDeck;
                    recentDeckMenu.Items.Add(item);
                }
            }
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
            { 
                deck.SaveDeckTo(saveFileDialog.FileName);
                UpdateRecentDecks(saveFileDialog.FileName);
            }
        }

        private void loadDeckClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                deck.LoadFrom(openFileDialog.FileName);
                UpdateRecentDecks(openFileDialog.FileName);
            }
        }

        private void loadRecentDeck(object sender, RoutedEventArgs e)
        {
            string path = (string)((MenuItem)sender).Header;
            deck.LoadFrom(path);
        }

        private void openNotes(object sender, RoutedEventArgs e)
        {
            NotesWindow notesWindow = new NotesWindow();
            notesWindow.Show();
            notesWindow.Owner = this;
        }
    }
}

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
using System.Windows.Shapes;

namespace AnkiClone
{
    /// <summary>
    /// Interaction logic for DeckWindow.xaml
    /// </summary>
    public partial class DeckWindow : Window
    {
        Deck deck;
        Card editedCard;

        public DeckWindow()
        {
            InitializeComponent();
        }

        public void InitDeck(ref Deck initDeck)
        {
            deck = initDeck;
            foreach (Card card in deck.GetCards())
            {
                cardList.Items.Add(card.Text);
            }
        }

        private void doneButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cardListSelected(object sender, SelectionChangedEventArgs e)
        {
            editedCard = deck.GetCards().Find(x => x.Text == cardList.SelectedItem.ToString());
            cardTextBox.Text = cardList.SelectedItem.ToString();
        }

        private void cardTextChanged(object sender, TextChangedEventArgs e)
        {
            editedCard.Text = cardTextBox.Text;
            deck.GetCards().Find(x => x == editedCard) = editedCard;
        }
    }
}

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

namespace FlashcardMemorizer
{
    /// <summary>
    /// Interaction logic for DeckWindow.xaml
    /// </summary>
    public partial class DeckWindow : Window
    {
        Deck deck;
        Card editedCard;
        int editedCardIndex;

        int selectedAnswerIndex;

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
            ((MainWindow)this.Owner).Update();
            Close();
        }

        private void cardListSelected(object sender, SelectionChangedEventArgs e)
        {
            editedCard = deck.GetCards().Find(x => x.Text == cardList.SelectedItem.ToString());
            if (editedCard != null)
            {
                editedCardIndex = deck.GetCards().FindIndex(0, x => x == editedCard);
                cardTextBox.TextChanged -= cardTextChanged;
                cardTextBox.Text = editedCard.Text;
                cardTextBox.TextChanged += cardTextChanged;
                answersList.Items.Clear();
                foreach (String answer in editedCard.Answers)
                    answersList.Items.Add(answer);
            }
        }

        // upon this textbox changing, we want:
        //   1. editedCard to be updated
        //   2. the deck to be changed
        //   3. the cardList to be updated
        private void cardTextChanged(object sender, TextChangedEventArgs e)
        {
            editedCard.Text = cardTextBox.Text;
            deck.SetCard(editedCardIndex, editedCard);
            cardList.SelectionChanged -= cardListSelected;
            cardList.Items[editedCardIndex] = editedCard.Text;
            cardList.SelectionChanged += cardListSelected;
        }

        private void addCardClick(object sender, RoutedEventArgs e)
        {
            editedCard = new Card();
            cardTextBox.TextChanged -= cardTextChanged;
            cardTextBox.Text = "New Card";
            cardTextBox.TextChanged += cardTextChanged;
            deck.AddCard(editedCard);
            editedCardIndex = deck.GetCards().Count-1;
            cardList.SelectionChanged -= cardListSelected;
            cardList.Items.Add(editedCard.Text);
            cardList.SelectionChanged += cardListSelected;
        }

        private void addAnswer(object sender, RoutedEventArgs e)
        {
            editedCard.Answers.Add(answerTextBox.Text);
            answersList.Items.Add(answerTextBox.Text);
            answerTextBox.Clear();
        }

        private void answerSelected(object sender, SelectionChangedEventArgs e)
        {
            addAnswerButton.IsEnabled = false;
            selectedAnswerIndex = answersList.SelectedIndex;
            if (selectedAnswerIndex != -1)
            {
                answerTextBox.Text = answersList.SelectedValue.ToString();
                removeAnswerButton.IsEnabled = true;
            }
            else
            {
                removeAnswerButton.IsEnabled = false;
            }
        }

        private void answerAddTextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void answerTextFocused(object sender, RoutedEventArgs e)
        {
            
        }

        private void removeAnswerClick(object sender, RoutedEventArgs e)
        {
            editedCard.Answers.RemoveAt(selectedAnswerIndex);
            deck.SetCard(editedCardIndex, editedCard);

            answersList.Items.RemoveAt(selectedAnswerIndex);
            answersList.UnselectAll();
        }
    }
}

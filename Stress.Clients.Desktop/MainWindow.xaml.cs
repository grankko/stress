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
using Stress.Game;
using Stress.Game.Cards;

namespace Stress.Clients.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Gameplay gameService = new Gameplay();


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gameService.AddPlayer("Anders");
            gameService.AddPlayer("Edith");

            gameService.Draw();

            OnChanged();
        }

        private void OnChanged()
        {
            PlayerOneSlotOne.Content = gameService.PlayerOne.OpenCards[0];
            PlayerOneSlotTwo.Content = gameService.PlayerOne.OpenCards[1];
            PlayerOneSlotThree.Content = gameService.PlayerOne.OpenCards[2];
            PlayerOneSlotFour.Content = gameService.PlayerOne.OpenCards[3];

            LeftPile.Content = gameService.LeftPile.TopCard;
            RightPile.Content = gameService.RightPile.TopCard;

            PlayerTwoSlotOne.Content = gameService.PlayerTwo.OpenCards[0];
            PlayerTwoSlotTwo.Content = gameService.PlayerTwo.OpenCards[1];
            PlayerTwoSlotThree.Content = gameService.PlayerTwo.OpenCards[2];
            PlayerTwoSlotFour.Content = gameService.PlayerTwo.OpenCards[3];

            PlayerOneCardsLeft.Content = $"Player one: {gameService.PlayerOne.Hand.Cards.Count()}";
            PlayerTwoCardsLeft.Content = $"Player two: {gameService.PlayerTwo.Hand.Cards.Count()}";
        }

        private void PlayerTwoSlotOne_Click(object sender, RoutedEventArgs e)
        {
            OpenPileOfCards selectedPile;
            if (PlayerTwoPileLeft.IsChecked.Value)
                selectedPile = gameService.LeftPile;
            else
                selectedPile = gameService.RightPile;

            gameService.PlayCardOnPile(gameService.PlayerTwo, gameService.PlayerTwo.OpenCards[0], selectedPile);

            OnChanged();
        }

        private void PlayerTwoSlotTwo_Click(object sender, RoutedEventArgs e)
        {
            OpenPileOfCards selectedPile;
            if (PlayerTwoPileLeft.IsChecked.Value)
                selectedPile = gameService.LeftPile;
            else
                selectedPile = gameService.RightPile;

            gameService.PlayCardOnPile(gameService.PlayerTwo, gameService.PlayerTwo.OpenCards[1], selectedPile);

            OnChanged();
        }

        private void PlayerTwoSlotThree_Click(object sender, RoutedEventArgs e)
        {
            OpenPileOfCards selectedPile;
            if (PlayerTwoPileLeft.IsChecked.Value)
                selectedPile = gameService.LeftPile;
            else
                selectedPile = gameService.RightPile;

            gameService.PlayCardOnPile(gameService.PlayerTwo, gameService.PlayerTwo.OpenCards[2], selectedPile);

            OnChanged();
        }

        private void PlayerTwoSlotFour_Click(object sender, RoutedEventArgs e)
        {
            OpenPileOfCards selectedPile;
            if (PlayerTwoPileLeft.IsChecked.Value)
                selectedPile = gameService.LeftPile;
            else
                selectedPile = gameService.RightPile;

            gameService.PlayCardOnPile(gameService.PlayerTwo, gameService.PlayerTwo.OpenCards[3], selectedPile);

            OnChanged();
        }

        private void PlayerOneSlotOne_Click(object sender, RoutedEventArgs e)
        {
            OpenPileOfCards selectedPile;
            if (PlayerOnePileLeft.IsChecked.Value)
                selectedPile = gameService.LeftPile;
            else
                selectedPile = gameService.RightPile;

            gameService.PlayCardOnPile(gameService.PlayerOne, gameService.PlayerOne.OpenCards[0], selectedPile);

            OnChanged();
        }

        private void PlayerOneSlotTwo_Click(object sender, RoutedEventArgs e)
        {
            OpenPileOfCards selectedPile;
            if (PlayerOnePileLeft.IsChecked.Value)
                selectedPile = gameService.LeftPile;
            else
                selectedPile = gameService.RightPile;

            gameService.PlayCardOnPile(gameService.PlayerOne, gameService.PlayerOne.OpenCards[1], selectedPile);

            OnChanged();
        }

        private void PlayerOneSlotThree_Click(object sender, RoutedEventArgs e)
        {
            OpenPileOfCards selectedPile;
            if (PlayerOnePileLeft.IsChecked.Value)
                selectedPile = gameService.LeftPile;
            else
                selectedPile = gameService.RightPile;

            gameService.PlayCardOnPile(gameService.PlayerOne, gameService.PlayerOne.OpenCards[2], selectedPile);

            OnChanged();
        }

        private void PlayerOneSlotFour_Click(object sender, RoutedEventArgs e)
        {
            OpenPileOfCards selectedPile;
            if (PlayerOnePileLeft.IsChecked.Value)
                selectedPile = gameService.LeftPile;
            else
                selectedPile = gameService.RightPile;

            gameService.PlayCardOnPile(gameService.PlayerOne, gameService.PlayerOne.OpenCards[3], selectedPile);

            OnChanged();
        }

        private void Draw_Click(object sender, RoutedEventArgs e)
        {
            gameService.Draw();
            OnChanged();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            gameService = new Gameplay();
            gameService.AddPlayer("Anders");
            gameService.AddPlayer("Edith");

            gameService.Draw();

            OnChanged();
        }
    }
}

using Stress.Game.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stress.Game
{
    public class Gameplay
    {
        public DeckOfCards Deck { get; private set; }
        public Player PlayerOne { get; private set; }
        public Player PlayerTwo { get; private set; }

        public OpenPileOfCards LeftPile { get; private set; }
        public OpenPileOfCards RightPile { get; private set; }

        public bool CanStart()
        {
            if (Deck.Cards.Count == 52 && PlayerOne != null && PlayerTwo != null)
                return true;

            return false;
        }

        public Gameplay()
        {
            Initialize();
        }

        /// <summary>
        /// If the players are ready, deal the full deck of cards to the players.
        /// </summary>
        public void Deal()
        {
            if (!CanStart()) throw new InvalidOperationException("Game is not in a ready state");

            while (Deck.Cards.Count > 0)
            {
                var drawnCard = Deck.DrawCard();
                if (drawnCard != null)
                    PlayerOne.PickUpCard(drawnCard);

                drawnCard = Deck.DrawCard();
                if (drawnCard != null)
                    PlayerTwo.PickUpCard(drawnCard);
            }
        }

        /// <summary>
        /// Happens when no player can act and the game is not yet over.
        /// </summary>
        public void Draw()
        {

            // Stale mate, players pick up the respective pile.
            if (PlayerOne.Hand.Cards.Count == 0 && PlayerTwo.Hand.Cards.Count == 0)
            {
                while (LeftPile.Cards.Count > 0)
                    PlayerOne.PickUpCard(LeftPile.DrawCard());
                while (RightPile.Cards.Count > 0)
                    PlayerTwo.PickUpCard(RightPile.DrawCard());
            }

            // If one player has an empty hand, and the other player has more than one card on hand
            // the player without a card will draw one from the other players hand.
            if (PlayerOne.Hand.Cards.Count == 0 && PlayerTwo.Hand.Cards.Count > 1)
                PlayerOne.Hand.Cards.Push(PlayerTwo.Hand.DrawCard());

            else if (PlayerTwo.Hand.Cards.Count == 0 && PlayerOne.Hand.Cards.Count > 1)
                PlayerTwo.Hand.Cards.Push(PlayerOne.Hand.DrawCard());

            LeftPile.PlayCard(PlayerOne.Hand.DrawCard(), true);
            RightPile.PlayCard(PlayerTwo.Hand.DrawCard(), true);
        }

        /// <summary>
        /// Adds a player to the game. If both slots are filled, cards are dealt.
        /// </summary>
        public Player AddPlayer(string nickName)
        {
            if (string.IsNullOrWhiteSpace(nickName))
                throw new ArgumentException($"'{nameof(nickName)}' cannot be null or whitespace", nameof(nickName));

            var newPlayer = new Player(nickName);

            if (PlayerOne == null)
                PlayerOne = newPlayer;
            else if (PlayerTwo == null)
            {
                PlayerTwo = newPlayer;
                Deal();
            }
            else
                throw new InvalidOperationException("The game already has two players");

            return newPlayer;
        }

        public void PlayCardOnPile(Player player, Card card, OpenPileOfCards pile)
        {
            if (pile.CanPlayCard(card))
            {
                pile.PlayCard(player.DrawOpenCard(card));
            }
        }

        private void Initialize()
        {
            Deck = new DeckOfCards();
            Deck.Shuffle();

            LeftPile = new OpenPileOfCards();
            RightPile = new OpenPileOfCards();

            PlayerOne = null;
            PlayerTwo = null;
        }
    }
}

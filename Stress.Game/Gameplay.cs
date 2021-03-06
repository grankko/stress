﻿using Stress.Game.Cards;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Stress.Game.Tests")]
namespace Stress.Game
{
    public class Gameplay : IGameplay
    {
        public StackOfCards Deck { get; private set; }
        public Player PlayerOne { get; private set; }
        public Player PlayerTwo { get; private set; }
        public OpenStackOfCards LeftStack { get; private set; }
        public OpenStackOfCards RightStack { get; private set; }

        public Gameplay()
        {
            Initialize();
        }

        /// <summary>
        /// Restarts the game with the same players
        /// </summary>
        public void RestartGame()
        {
            Deck = StackOfCards.CreateFullDeckOfCards();
            Deck.Shuffle();

            LeftStack = new OpenStackOfCards();
            RightStack = new OpenStackOfCards();

            // todo: ugly
            var playerOneName = PlayerOne.NickName;
            var playerTwoName = PlayerTwo.NickName;

            PlayerOne = null;
            PlayerTwo = null;

            AddPlayer(playerOneName);
            AddPlayer(playerTwoName);
        }

        public Player GetPlayerByNumber(int number)
        {
            if (number == 1)
                return PlayerOne;
            else if (number == 2)
                return PlayerTwo;
            else
                throw new ArgumentException("Invalid player number");
        }

        public bool CanStart()
        {
            if (PlayerOne != null && PlayerTwo != null)
                return true;

            return false;
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
        /// An attempt to jump start the game when no player can act and the game is not yet over.
        /// Plays one card each from their closed stacks on hand to respective open stack.
        /// </summary>
        public void Draw()
        {
            // Stale mate, players pick up the respective stack. Two cards are needed to do a draw.
            if ((PlayerOne.Hand.Cards.Count + PlayerTwo.Hand.Cards.Count) < 2)
            {
                while (LeftStack.Cards.Count > 0)
                    PlayerOne.PickUpCard(LeftStack.DrawCard());
                while (RightStack.Cards.Count > 0)
                    PlayerTwo.PickUpCard(RightStack.DrawCard());
            }

            // If one player has an empty hand, and the other player has more than one card on hand
            // the player without a card will draw one from the other players hand.
            if (PlayerOne.Hand.Cards.Count == 0 && PlayerTwo.Hand.Cards.Count > 1)
                PlayerOne.Hand.Cards.Push(PlayerTwo.Hand.DrawCard());

            else if (PlayerTwo.Hand.Cards.Count == 0 && PlayerOne.Hand.Cards.Count > 1)
                PlayerTwo.Hand.Cards.Push(PlayerOne.Hand.DrawCard());

            LeftStack.AddCard(PlayerOne.Hand.DrawCard(), true);
            RightStack.AddCard(PlayerTwo.Hand.DrawCard(), true);
        }

        /// <summary>
        /// Adds a player to the game. If both slots are filled, cards are dealt.
        /// </summary>
        public void AddPlayer(string nickName)
        {
            if (string.IsNullOrWhiteSpace(nickName))
                throw new ArgumentException($"'{nameof(nickName)}' cannot be null or whitespace", nameof(nickName));

            if (PlayerOne == null)
                PlayerOne = new Player(nickName, true);
            else if (PlayerTwo == null)
            {
                PlayerTwo = new Player(nickName, false);
                Deal();
            }
            else
                throw new InvalidOperationException("The game already has two players");
        }

        public void PlayCardOnStack(Player player, Card card, OpenStackOfCards stack)
        {
            if (stack.CanAddCard(card))
                stack.AddCard(player.PlayOpenCard(card));
        }

        public Player PlayerCallsStressEvent(int playerNumber)
        {
            return PlayerCallsStressEvent(GetPlayerByNumber(playerNumber));
        }

        private Player PlayerCallsStressEvent(Player player)
        {
            Player stressEventLoser;

            if (LeftStack.TopCard.Rank == RightStack.TopCard.Rank)
            {
                // Legit stress event, calling player wins
                if (player.IsPlayerOne)
                    stressEventLoser = PlayerTwo;
                else
                    stressEventLoser = PlayerOne;
            }
            else
            {
                // Not a legit stress event, calling player loses
                if (player.IsPlayerOne)
                    stressEventLoser = PlayerOne;
                else
                    stressEventLoser = PlayerTwo;
            }

            // Loser picks upp both stacks
            while (LeftStack.Cards.Count > 0)
                stressEventLoser.PickUpCard(LeftStack.DrawCard());
            while (RightStack.Cards.Count > 0)
                stressEventLoser.PickUpCard(RightStack.DrawCard());

            Draw();
            return stressEventLoser;
        }

        private void Initialize()
        {
            Deck = StackOfCards.CreateFullDeckOfCards();
            Deck.Shuffle();

            LeftStack = new OpenStackOfCards();
            RightStack = new OpenStackOfCards();

            PlayerOne = null;
            PlayerTwo = null;
        }
    }
}

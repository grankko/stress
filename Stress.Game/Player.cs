using Stress.Game.Cards;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Stress.Game.Tests")]
namespace Stress.Game
{
    public class Player
    {
        public string NickName { get; set; }
        public bool IsPlayerOne { get; private set; }
        public int PlayerNumber { get => IsPlayerOne ? 1 : 2; }

        /// <summary>
        /// The 0 - 4 open cards a player has to act with
        /// </summary>
        public Card[] OpenCards { get; private set; } = new Card[4];

        public int ActiveOpenCards { get => OpenCards.Where(slot => slot != null).Count(); }

        public bool HasWon { get => (ActiveOpenCards + Hand.Cards.Count()) == 0; }

        /// <summary>
        /// All closed cards a player has on hand. If an OpenCard slot is free, it will
        /// be filled from this set of cards.
        /// </summary>
        public StackOfCards Hand { get; private set; }

        public Player(string nickName, bool isPlayerOne)
        {
            if (string.IsNullOrWhiteSpace(nickName))
                throw new ArgumentException($"'{nameof(nickName)}' cannot be null or whitespace", nameof(nickName));

            NickName = nickName;
            IsPlayerOne = isPlayerOne;

            Hand = new StackOfCards();
        }

        /// <summary>
        /// Plays an open card and refills if possible from the closed cards on hand.
        /// </summary>
        /// <param name="card">Card to play</param>
        /// <returns>Played card</returns>
        public Card PlayOpenCard(Card card)
        {
            var index = Array.IndexOf<Card>(OpenCards, card);
            if (index == -1)
                throw new InvalidOperationException($"{NickName} does not have {card}");

            return PlayOpenCard(index);
        }

        private Card PlayOpenCard(int cardIndex)
        {
            if (cardIndex > 3 || cardIndex < 0)
                throw new InvalidOperationException("Player only has card slots 1 - 4.");

            Card playedCard = OpenCards[cardIndex];

            if (Hand.Cards.Count > 0)
                OpenCards[cardIndex] = Hand.DrawCard();
            else
                OpenCards[cardIndex] = null;

            return playedCard;
        }

        /// <summary>
        /// Pick up a card and fill open slot or add to closed cards on hand.
        /// </summary>
        /// <param name="card">Card to pick up.</param>
        public void PickUpCard(Card card)
        {
            if (OpenCards[0] == null)
                OpenCards[0] = card;
            else if (OpenCards[1] == null)
                OpenCards[1] = card;
            else if (OpenCards[2] == null)
                OpenCards[2] = card;
            else if (OpenCards[3] == null)
                OpenCards[3] = card;
            else
                Hand.Cards.Push(card);
        }

    }
}

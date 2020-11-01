using Stress.Game.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stress.Game
{
    public class Player
    {
        public Guid Id { get; private set; }
        public string NickName { get; set; }

        public Card[] OpenCards { get; private set; } = new Card[4];

        public PileOfCards Hand { get; private set; }

        public Player(string nickName)
        {
            Id = Guid.NewGuid();

            if (string.IsNullOrWhiteSpace(nickName))
                throw new ArgumentException($"'{nameof(nickName)}' cannot be null or whitespace", nameof(nickName));

            NickName = nickName;

            Hand = new PileOfCards();
        }

        public Card DrawOpenCard(Card card)
        {
            var index = Array.IndexOf<Card>(OpenCards, card);
            if (index == -1)
                throw new InvalidOperationException($"{NickName} does not have {card}");

            return DrawOpenCard(index);
        }

        private Card DrawOpenCard(int cardIndex)
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

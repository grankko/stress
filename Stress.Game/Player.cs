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

        public Card CardSlot1 { get; set; }
        public Card CardSlot2 { get; set; }
        public Card CardSlot3 { get; set; }
        public Card CardSlot4 { get; set; }

        public PileOfCards Hand { get; private set; }

        public Player(string nickName)
        {
            Id = Guid.NewGuid();

            if (string.IsNullOrWhiteSpace(nickName))
                throw new ArgumentException($"'{nameof(nickName)}' cannot be null or whitespace", nameof(nickName));

            NickName = nickName;

            Hand = new PileOfCards();
        }

        public void PickUpCard(Card card)
        {
            if (CardSlot1 == null)
                CardSlot1 = card;
            else if (CardSlot2 == null)
                CardSlot2 = card;
            else if (CardSlot3 == null)
                CardSlot3 = card;
            else if (CardSlot4 == null)
                CardSlot4 = card;
            else
                Hand.Cards.Push(card);
        }
    }
}

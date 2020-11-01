using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stress.Game.Cards
{
    public class PileOfCards
    {
        public Stack<Card> Cards { get; private set; }

        public PileOfCards()
        {
            Cards = new Stack<Card>();
        }

        public Card DrawCard()
        {
            if (Cards.Count > 0)
                return Cards.Pop();
            else
                return null;
        }

        public void Shuffle()
        {
            var cards = Cards.ToList();
            Random randomizer = new Random();

            Cards.Clear();
            while (cards.Count > 0)
            {
                var randomCard = cards[randomizer.Next(0, cards.Count)];
                Cards.Push(randomCard);
                cards.Remove(randomCard);
            }
        }


    }
}

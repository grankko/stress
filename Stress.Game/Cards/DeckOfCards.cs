using System;
using System.Collections.Generic;
using System.Text;

namespace Stress.Game.Cards
{
    public class DeckOfCards : PileOfCards
    {
        public DeckOfCards() : base()
        {
            PopulateFullDeck();
        }

        private void PopulateFullDeck()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                for (int i = 2; i < 15; i++)
                {
                    Cards.Push(new Card(i, suit));
                }
            }
        }
    }
}

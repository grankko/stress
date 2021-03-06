﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Stress.Game.Tests")]
namespace Stress.Game.Cards
{
    /// <summary>
    /// Represents any set of cards that you can shuffle and draw from.
    /// </summary>
    public class StackOfCards
    {
        public Stack<Card> Cards { get; private set; }

        public StackOfCards()
        {
            Cards = new Stack<Card>();
        }

        internal static StackOfCards CreateFullDeckOfCards()
        {
            var fullDeck = new StackOfCards();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                for (int i = 2; i < 15; i++)
                {
                    fullDeck.Cards.Push(new Card(i, suit));
                }
            }
            return fullDeck;
        }

        internal Card DrawCard()
        {
            if (Cards.Count > 0)
                return Cards.Pop();
            else
                return null;
        }

        internal void Shuffle()
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

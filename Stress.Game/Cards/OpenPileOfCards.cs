using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Stress.Game.Cards
{
    public class OpenPileOfCards : PileOfCards
    {
        public Card TopCard { get { return Cards.Peek(); } }

        public OpenPileOfCards() : base() { }

        /// <summary>
        /// Plays a specific card
        /// </summary>
        /// <param name="card">Card to play</param>
        /// <param name="isDraw">If game is in draw, all cards are playable</param>
        public void PlayCard(Card card, bool isDraw = false)
        {
            if (card is null)
                throw new ArgumentNullException(nameof(card));

            if (!CanPlayCard(card, isDraw))
                throw new InvalidOperationException("Invalid move.");

            Cards.Push(card);
        }

        /// <summary>
        /// Validates if a card is playable on the pile or not
        /// </summary>
        /// <param name="card">Card to play</param>
        /// <param name="isDraw">If game is in draw, all cards are playable</param>
        public bool CanPlayCard(Card card, bool isDraw = false)
        {
            if (card is null)
                throw new ArgumentNullException(nameof(card));

            if (Cards.Count == 0)
                return true;
            else if (isDraw)
                return true;

            // Aces can be played on deuces and kings
            if (card.Rank == 14 && (TopCard.Rank == 2 || TopCard.Rank == 13))
                return true;
            if (TopCard.Rank == 14 && (card.Rank == 2 || card.Rank == 13))
                return true;
            else
            {
                // Cards within 1 rank are playable
                if (Math.Abs(TopCard.Rank - card.Rank) == 1)
                    return true;
                else
                    return false;
            }
        }
    }
}

using System;

namespace Stress.Game.Cards
{
    /// <summary>
    /// Represents a set of cards with the face up in the game of Stress.
    /// The top card is the card that players can act on.
    /// In case of a stress event, the slowest player must pick up these cards
    /// and keep them on hand.
    /// </summary>
    public class OpenStackOfCards : StackOfCards
    {
        public Card TopCard { get { return Cards.Count == 0 ? null : Cards.Peek(); } }

        public OpenStackOfCards() : base() { }

        /// <summary>
        /// Adds a specific card on the stack.
        /// </summary>
        /// <param name="card">Card to add</param>
        /// <param name="isDraw">If game is in draw, all cards are playable</param>
        public void AddCard(Card card, bool isDraw = false)
        {
            if (card is null)
                throw new ArgumentNullException(nameof(card));

            if (!CanAddCard(card, isDraw))
                throw new InvalidOperationException("Invalid move.");

            Cards.Push(card);
        }

        /// <summary>
        /// Validates if a card is playable on the stack or not.
        /// </summary>
        /// <param name="card">Card to play</param>
        /// <param name="isDraw">If game is in draw, all cards are playable</param>
        public bool CanAddCard(Card card, bool isDraw = false)
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

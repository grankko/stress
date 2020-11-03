using System;
using System.Runtime.CompilerServices;

namespace Stress.Game.Cards
{
    public class Card
    {
        public int Rank { get; private set; }
        public Suit Suit { get; private set; }

        public string ShortName
        { 
            get {
                string result = string.Empty;
                switch (Suit)
                {
                    case Suit.Clubs:
                        result = $"C{Rank}";
                        break;
                    case Suit.Diamonds:
                        result = $"D{Rank}";
                        break;
                    case Suit.Hearts:
                        result = $"H{Rank}";
                        break;
                    case Suit.Spades:
                        result = $"S{Rank}";
                        break;
                }
                return result;
            }
        }

        public Card(int rank, Suit suit)
        {
            if (rank > 14 || rank < 2)
                throw new ArgumentException($"{0} is not a valid rank");

            Rank = rank;
            Suit = suit;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Card))
                return false;
            
            Card compareCard = (Card)obj;
            return (compareCard.Suit == this.Suit && compareCard.Rank == Rank);
        }

        public override string ToString()
        {
            string rankNickName = null;
            switch (Rank)
            {
                case 11:
                    rankNickName = "Jack";
                    break;
                case 12:
                    rankNickName = "Queen";
                    break;
                case 13:
                    rankNickName = "King";
                    break;
                case 14:
                    rankNickName = "Ace";
                    break;
                default:
                    rankNickName = Rank.ToString();
                    break;
            }

            return $"{rankNickName} of {Suit}";
        }

        public override int GetHashCode()
        {
            return (Rank, Suit).GetHashCode();
        }
    }


    public enum Suit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }
}

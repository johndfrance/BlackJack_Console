using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console
{
    /// <summary>
    /// Card represents a single playing card and has a Rank and a Suit
    /// 
    /// Card contains an a toString() method 
    /// </summary>

    public class Card
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }
        public Card(Rank rank, Suit suit)
        {
            this.Rank = rank;
            this.Suit = suit;
        }
        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
    }
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades,
    }
    public enum Rank
    {
        Ace = 11,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 10,
        Queen = 10,
        King = 10,
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console
{
    public class Hand : IEnumerable<Card>
    {
        private List<Card> Cards { get; set; }

        public int Count { get; }

        public Hand() { Cards = new List<Card>(); }

        public void addCard(Card card) { Cards.Add(card); }

        public void removeCard(Card card) { Cards.Remove(card); }

        public int countCard() { return Cards.Count(); }
        

        public int HandValue()
        {
            int sum = 0;
            int numOfAces = 0;

            foreach (Card card in Cards)
            {
                sum += (int)card.Rank;
                if (card.Rank == Rank.Ace) { numOfAces++; }
            }
            // Aces are 11 unless that causes the hand to bust, if so they are 1.
            while (numOfAces > 0)
            {
                if (sum > 21)
                {
                    sum -= 10;
                    numOfAces--;
                }
                else { break; }
            }
            return sum;
        }

        public void PrintHand()
        {
            foreach (Card card in Cards) { Console.WriteLine("[" + card.ToString() + "]");}
            Console.Write("");
            Console.WriteLine("Value: "+ HandValue().ToString());
            Console.WriteLine("");
        }

        public IEnumerator<Card> GetEnumerator() { return Cards.GetEnumerator(); }
         IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); } // Explicit non-generic interface implementation

    } 
}

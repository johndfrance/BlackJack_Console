using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console
{
    public class Hand
    {
        private List<Card> Cards { get; set; }

        public Hand()
        {
            Cards = new List<Card>();
        }

        public void addCard(Card card)
        {
            Cards.Add(card);
        }

        public void removeCard(Card card)
        {
            Cards.Remove(card);
        }

        public int HandValue()
        {
            int sum = 0;
            int numOfAces = 0;

            foreach (Card card in Cards)
            {
                sum += (int)card.Rank;
                if (card.Rank == Rank.Ace)
                {
                    numOfAces++;
                }
            }
            // Aces are 11 unless that causes the hand to bust, if so they are 1.
            while (numOfAces > 0)
            {
                if (sum > 21)
                {
                    sum -= 10;
                    numOfAces--;
                }
                else
                {
                    break;
                }
            }
            return sum;
        }

        public void PrintHand()
        {
            foreach (Card card in Cards)
            {
                Console.WriteLine(card.ToString());
            }
            Console.WriteLine("Value: "+ HandValue().ToString());
            Console.WriteLine("");
        }
    } 
}

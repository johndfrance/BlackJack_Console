using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console
{
    public class Round
    {
        private int pot { get; set; }
        private Hand playerHand;
        private Hand dealerHand;
        private Deck deck;

        public Round(Deck deck)
        {
            this.deck = deck;
        }

        public int PlayRound()
        {
            
            deck.Shuffle();
            Console.WriteLine("New Round Starting");
            Console.WriteLine("Place your bet");
            // Get bet from User
            int pot = Int32.Parse(Console.ReadLine());

            playerHand = new Hand();
            playerHand.addCard(deck.DealCard());
            playerHand.addCard(deck.DealCard());
            dealerHand = new Hand();
            dealerHand.addCard(deck.DealCard());
            dealerHand.addCard(deck.DealCard());

            StateofGame();


            while (true) // Player doesn't stand/21/bust
            {
                if (playerHand.HandValue() == 21) // Check for blackjack
                {
                    if (dealerHand.HandValue() == 21) // Check if dealer has blackjack, tie is so.
                    {
                        Console.WriteLine("PUSH");
                        StateofGame();
                        return 0;
                    }
                    else // If dealer doesn't also have 21 player wins
                    {
                        Console.WriteLine("Player wins");
                        StateofGame();
                        return pot;
                    }
                }
                else // Player doesn't have 21
                {
                    StateofGame();

                    // Hit or Stand
                    Console.WriteLine("Hit(h) or Stand(s): ");
                    string hit = Console.ReadLine();

                    if (hit.Equals("h")) // Hit
                    {
                        Card card = deck.DealCard();
                        playerHand.addCard(card);

                        if(playerHand.HandValue() > 21) // Check if player Bust
                        {
                            Console.WriteLine("Player Bust");
                            StateofGame();
                            return -pot;
                        }
                    }
                    else // Stand
                    {
                        break;
                    }
                }
            }
            while(dealerHand.HandValue() <= 17)
            {
                Card card = deck.DealCard();
                dealerHand.addCard(card);

                if (dealerHand.HandValue() > 21)
                {
                    Console.WriteLine("Player Wins, Dealer Bust");
                    StateofGame();
                    return pot;
                }
                else if(dealerHand.HandValue()==21){
                    // We don't need to check if player has 21 bc that would have been already caught.

                    Console.WriteLine("Dealer win");
                    StateofGame(); 
                    return -pot;
                }
            }

            if(playerHand.HandValue() > dealerHand.HandValue())
            {
                Console.WriteLine("Player Wins");
                StateofGame();
                return pot;
            }
            else if(dealerHand.HandValue() > playerHand.HandValue())
            {
                Console.WriteLine("Dealer Wins");
                StateofGame();
                return -pot;
            }
            else
            {
                Console.WriteLine("Push");
                StateofGame();
                return 0;
            }

        }

        public void Bet() { }

        public void Deal()
        {

        }

        public void Play()
        {

        }

        public void Hit() { }
        public void Stand() { }
        public void Insure() { }
        public void Split() { }
        public void Double() { }
        public void StateofGame()
        {
            Console.WriteLine("Player Hand:");
            playerHand.PrintHand();
            Console.WriteLine("Dealer Hand:");
            dealerHand.PrintHand();
        }
    }
}

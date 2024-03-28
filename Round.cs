using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console
{
    public class Round
    {
        /*
         * Rami's TODO List:
         *  [RULES] (in order of priority)
         *   - double down [DONE]
         *      - if hit hasn't been called within the first dealing then double down.
         *      - double the pot but draw one card then end hand for player [needs to be done ONLY in the first hand].
         *          - once [hit] happens, double down is disabled.
         *   - insurance
         *      - if dealer face card is ace, there is a good chance that the next card is worth 10.
         *      - offer player insurance [side bet], therefore being breakeven
         *          - if insurance is clicked && blackjack clause is true then make pot 0.
         *   - split.
         *      - when you get a pair of cards [example: two threes]
         *          - split the pair
         *          - place as side bet
         *          - get dealt one card per pair therefore making two hands.
         *          - once both are established, player is forced to play both hands.
         *   
         *   [game start]
         *   - only see one of the dealers card, no more, no less [INSURANCE RELATED].
         *   - 
         */

        private int pot { get; set; }
        private Hand playerHand;
        private Hand dealerHand;
        private Deck deck;

        public Round(Deck deck) {this.deck = deck;}

        public int PlayRound()
        {
            /*
             * Shuffle the deck then proceed to make player place a bet. 
             * Once a valid bet is placed then you proceed with giving 
             * both the dealer and player a set of cards [2 cards to start]
             * and ask them to hit, stand, or double down. 
             * 
             * NOTE: double down is only available when you make your first decision in the gameplay loop
             */
            deck.Shuffle();
            Console.WriteLine("New Round Starting");
            Console.WriteLine("Place your bet");

            // This will store the bet in 'pot' only if the user input is a valid integer.
            int parsedBet;
            if (!int.TryParse(Console.ReadLine(), out parsedBet))
            {
                Console.WriteLine("Invalid bet amount. Please enter a valid number.");
                return 0; 
            }
            pot = parsedBet; 

            playerHand = new Hand();
            playerHand.addCard(deck.DealCard());
            playerHand.addCard(deck.DealCard());

            dealerHand = new Hand();
            dealerHand.addCard(deck.DealCard());
            dealerHand.addCard(deck.DealCard());

            GameState();

            bool playerTurn = true;


            /*
             * Once the inital moves are done and the player nor dealer
             * has won already, it will start the secondary game loop
             * checking an input for whether the player has hit, stood, 
             * or doubled down. Using a switch statement we are able to
             * figure out the players action out of those three options.
             * 
             * HIT: 
             * As the player choses to hit, a new card gets dealt. 
             * If the player now has over 21, they lose.
             * 
             * STAND: As the player choses to stand, they skip thier turn
             * 
             * DOUBLE DOWN: 
             * When a player does this move at the start of the round, 
             * thier bets double, they must add a card to thier roster,
             * and must stand as a result having chance dictate if they
             * lose or not (over 21).
             */
            while (playerTurn)
            {
                // Checking for blackjack at the start of the player's turn.
                if (playerHand.HandValue() == 21) { Console.WriteLine("Blackjack! Player wins."); return pot; }

                Console.WriteLine("Hit [h], Stand [s]" + (playerHand.countCard() == 2 ? ", Double down [d]" : "") + ": ");
                string playerChoice = Console.ReadLine().ToLower();

                switch (playerChoice)
                {
                    // Hit
                    case "h":
                        Card card = deck.DealCard();
                        playerHand.addCard(card);
                        GameState();

                        if (playerHand.HandValue() > 21) { Console.WriteLine("Bust! Player loses."); return -pot; } break;

                    // Double down
                    case "d": 
                        if (playerHand.countCard() == 2)
                        {
                            pot *= 2;
                            playerHand.addCard(deck.DealCard());
                            GameState();

                            if (playerHand.HandValue() > 21) { Console.WriteLine("Bust! Player loses."); return -pot; } playerTurn = false; 
                        }
                        else {Console.WriteLine("Double down can only be done on the first hand.");} break;

                    //stand
                    case "s": playerTurn = false; break;

                    default: Console.WriteLine("Invalid option, please try again."); break;
                }
            }

            while (dealerHand.HandValue() <= 17) {dealerHand.addCard(deck.DealCard());}
            GameState(); // Show final hands.

            // Compare hand values and decide the winner.
            int playerHandValue = playerHand.HandValue();
            int dealerHandValue = dealerHand.HandValue();

            if (dealerHandValue > 21 || playerHandValue > dealerHandValue) { Console.WriteLine("Player Wins!"); return pot;}
            else if (playerHandValue < dealerHandValue) {Console.WriteLine("Dealer Wins."); return -pot;}
            else { Console.WriteLine("Push. It's a tie."); return 0;}
        }

        // TODO:
        public void Insure() { }
        public void Split() { }


        public void GameState()
        {
            Console.WriteLine("Player Hand:");
            playerHand.PrintHand();
            Console.WriteLine("Dealer Hand:");
            dealerHand.PrintHand();
        }
    }
}

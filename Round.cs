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
        private int pot { get; set; }

        private Hand playerHand;
        private Hand dealerHand;
        private Deck deck;
        private Card card;

        public Round(Deck deck) { this.deck = deck; }

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

            Console.WriteLine("");
            Console.WriteLine("New Round Starting");
            Console.Write("");
            Console.Write("Place your bet: ");

            // This will store the bet in 'pot' only if the user input is a valid integer.
            int parsedBet;
            if (!int.TryParse(Console.ReadLine(), out parsedBet))
            {
                Console.WriteLine("Invalid bet amount. Please enter a valid number.");
                Console.Write("");
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
                if (playerHand.HandValue() == 21)
                {
                    Console.WriteLine("Blackjack! Player wins.");
                    return pot;
                }

                Console.Write("Hit [h], Stand [s]" + (playerHand.countCard() == 2 ? ", Double down [d]" : "") + ": ");
                string playerChoice = Console.ReadLine().ToLower();

                switch (playerChoice)
                {
                    // Hit
                    case "h":
                        Card card = deck.DealCard();
                        playerHand.addCard(card);
                        GameState();

                        if (playerHand.HandValue() > 21)
                        {
                            Console.WriteLine("Bust! Player loses.");
                            Console.Write("");
                            return -pot;
                        }
                        break;

                    // Double down
                    case "d":
                        if (playerHand.countCard() == 2)
                        {
                            pot *= 2;
                            playerHand.addCard(deck.DealCard());
                            GameState();

                            if (playerHand.HandValue() > 21)
                            {
                                Console.WriteLine("Bust! Player loses.");
                                Console.Write("");
                                return -pot;
                            }
                            playerTurn = false;
                        }
                        else
                        {
                            Console.WriteLine("Double down can only be done on the first hand.");
                        }
                        break;

                    //stand
                    case "s":
                        playerTurn = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }

            while (dealerHand.HandValue() <= 17) { dealerHand.addCard(deck.DealCard()); }
            GameState(); // Show final hands.

            // Compare hand values and decide the winner.
            int playerHandValue = playerHand.HandValue();
            int dealerHandValue = dealerHand.HandValue();

            if (dealerHandValue > 21 || playerHandValue > dealerHandValue)
            {
                Console.WriteLine("Player Wins!");
                Console.Write("");
                return pot;
            }
            else if (playerHandValue < dealerHandValue)
            {
                Console.WriteLine("Dealer Wins.");
                Console.Write("");
                return -pot;
            }
            else
            {
                Console.WriteLine("Push. It's a tie.");
                Console.Write("");
                return 0;
            }
        }

        public void Insure()
        {
            bool IsInsured = false;

            foreach (var card in dealerHand)
            {
                if (card.Rank == Rank.Ace)
                {
                    IsInsured = true;
                    break;
                }
            }

            if (IsInsured)
            {
                Console.WriteLine("Dealer has an Ace. Insurance is possible.");

                Console.WriteLine("Would you like insurance? [Y/N]: ");
                string playerChoice = Console.ReadLine().ToLower();

                if (playerChoice == "y")
                {
                    Console.WriteLine("Enter your side bet: ");
                    if (!int.TryParse(Console.ReadLine(), out int sideBet))
                    {
                        Console.WriteLine("Invalid bet amount. Please enter a valid number.");
                        Console.Write("");
                    }
                    // Implementation of handling the side bet goes here
                    pot = 0;
                }
                else if (playerChoice != "n")
                {
                    Console.WriteLine("Invalid option, please try again.");
                    Console.Write("");
                }
            }
            else
            {
                return; // Exit the method if there is no Ace, as insurance is not relevant
            }
        }

        public void Split() 
        {
            // insert this function where the dealing of cards takes place

            /*
             * In order to split, two cards must be dealt that have the same value.
             * 
             * NOTE: Splitting is optional where if you do split, you turn a single hand 
             * into two hands where you place an additional bet equal to the original bet.
             * 
             * Once you split the cards, the dealer will give you an additional card per hand.
             * For example if you have two sevens, it will now be one seven on each hand plus a 
             * drawn card for each hand like [seven of diamond, ace of hearts] and 
             * [seven of diamond, nine of spades].
             * 
             * you are to play out each hand one at a time. You are allowed to hit, stand, double down,
             * and split again (only splitting when you recieve another pair). As you are playing one hand
             * at a time, the wins and losses will be encapsulated within those hands individually.
             * 
             * SPECIAL RULES:
             * 1) Splitting aces allow you do draw one card to each ace but you are prohibited from 
             * hitting.
             * 2) if you get a 10 card after splitting aces, it is counted as 21 points rather than 'blackjack'
             * 3) you are allowed to resplit if you get a pair right after the first split causing four hands rather than two.
             * 4) you are allowed to double down on both hands one by one after the split.
             */

            //TODO:

            // possible idea: create two instances of a hand with thier individual draws and switch between them as you play?

            bool canSplit;

            //check if two cards are a pair
            foreach(Card card in Hand)
            {
                if (Cards.Count == 2 && Cards[0].Rank == Cards[1].Rank)
                {
                    canSplit = true;
                    break;
                }
                else
                {
                    canSplit = false;
                    break;
                }
            }

            if (canSplit == true)
            {
                //draw a card from the deck for each hand

                // validate if a hand has an ace in it

                // validate if a card has a value of 10

                // figure out how to play each hand individually??? [probably something to do with objects of hands]

                // hit for each hand

                // stand for each hand

                // double down for each hand

                // split if another pair occurs [have a split validation logic embedded in the main loop??]
            }
        }


        public void GameState()
        {
            Console.WriteLine("");
            Console.WriteLine("Player Hand:");
            playerHand.PrintHand();
            Console.Write("");

            Console.Write("");
            Console.WriteLine("Dealer Hand:");
            dealerHand.PrintHand();
            Console.Write("");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console
{
    public class Game
    {
        private Deck GameDeck;
        private Player player;

        public Game()
        {
            Deck deck = new Deck();
            deck.Shuffle();
            this.player = new Player();
            GameDeck = deck;
        }

        public void PlayGame()
        {
            Game game = new Game();

            while (true)
            {
                Round round = new Round(GameDeck);
                player.wallet += round.PlayRound();
                Console.WriteLine("\nPlayer now has: $" + player.wallet.ToString());
                if(GameDeck.deck.Count()< 20)
                {
                    Console.WriteLine("\nSHUFFLING\n");
                    GameDeck = new Deck();
                    GameDeck.Shuffle();
                }
                if(player.wallet <= 0)
                {
                    Console.WriteLine("You don't have anymore money");
                    break;
                }
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Console
{
    public class Player
    {
        private int id { get; set; }
        private string name { get; set; }
        public int wallet { get; set; }


        public Player()
        {
            id = 0;
            name = "JEFF";
            wallet = 20000;
        }


    }
}

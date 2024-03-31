using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Misc
{
    public class Player
    {
        public ulong id;
        public string link;

        public Player()
        {

        }

        public Player(ulong id, string link)
        {
            this.id = id;
            this.link = link;
        }
    }
}

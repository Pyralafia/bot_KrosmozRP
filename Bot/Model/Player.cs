using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bot.Model
{
    public class Player
    {
        [XmlElement(ElementName = "Id")]
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

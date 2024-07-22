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

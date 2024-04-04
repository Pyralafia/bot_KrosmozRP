using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bot.Misc
{
    internal class CharacterSheet : Player
    {
        [XmlElement(ElementName = "")]
        public Classes classe;
        [XmlElement(ElementName = "")]
        public int initativ;
        [XmlElement(ElementName = "")]
        public int wisdom;
        [XmlElement(ElementName = "")]
        public int agility;
        [XmlElement(ElementName = "")]
        public int luck;
        [XmlElement(ElementName = "")]
        public int strength;
        [XmlElement(ElementName = "")]
        public int intelligence;
    }
}

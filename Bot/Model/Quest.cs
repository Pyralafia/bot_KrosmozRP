using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bot.Model
{
    [XmlType("_")]
    public class Quest
    {
        [XmlElement(ElementName = "Id")]
        public string id;
        [XmlElement(ElementName = "Name")]
        public string name;
        [XmlElement(ElementName = "Level")]
        public string lvl;
        [XmlElement(ElementName = "Type")]
        public string type;
        [XmlElement(ElementName = "Rarity")]
        public string rarity;
        [XmlElement(ElementName = "Repetability")]
        public string repeated;
        [XmlElement(ElementName = "NbMax")]
        public string playerMax;
        [XmlElement(ElementName = "Dedicated")]
        public string playerName;
        [XmlElement(ElementName = "Description")]
        public string description;
        [XmlElement(ElementName = "Kamas")]
        public string kamas;
        [XmlElement(ElementName = "Reward")]
        public string reward;
        [XmlElement(ElementName = "Status")]
        public string status;

    }
}

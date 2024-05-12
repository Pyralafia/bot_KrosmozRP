using Bot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bot.Model
{
    [XmlType("_")]
    public class CharacterSheet
    {
        [XmlElement(ElementName = "Id")]
        public string id;
        [XmlElement(ElementName = "Classe")]
        public string classeXmlString;
        public Classes classe;
        [XmlElement(ElementName = "Sagesse")]
        public int wisdom;
        [XmlElement(ElementName = "Agilite")]
        public int agility;
        [XmlElement(ElementName = "Chance")]
        public int luck;
        [XmlElement(ElementName = "Force")]
        public int strength;
        [XmlElement(ElementName = "Intelligence")]
        public int intelligence;

        public CharacterSheet() { }

        public void ConvertXmlStringToClass()
        {
            switch (classeXmlString.Substring(0, 2))
            {
                case "01":
                    classe = Classes.Ecaflip;
                    break;

                case "02":
                    classe = Classes.Eniripsa;
                    break;

                case "03":
                    classe = Classes.Iop;
                    break;

                case "04":
                    classe = Classes.Cra;
                    break;

                case "05":
                    classe = Classes.Feca;
                    break;

                case "06":
                    classe = Classes.Sacrieur;
                    break;

                case "07":
                    classe = Classes.Sadida;
                    break;

                case "08":
                    classe = Classes.Osamodas;
                    break;

                case "09":
                    classe = Classes.Enutrof;
                    break;

                case "10":
                    classe = Classes.Sram;
                    break;

                case "11":
                    classe = Classes.Xelor;
                    break;

                case "12":
                    classe = Classes.Pandawa;
                    break;

                case "13":
                    classe = Classes.Roublard;
                    break;

                case "14":
                    classe = Classes.Zobal;
                    break;

                case "15":
                    classe = Classes.Steamer;
                    break;

                case "16":
                    classe = Classes.Eliotrop;
                    break;

                case "17":
                    classe = Classes.Huppermage;
                    break;

                case "18":
                    classe = Classes.Forgelance;
                    break;

                default:
                    break;
            }
        }
    }
}

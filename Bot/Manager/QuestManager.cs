using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bot.Manager
{
    [XmlType("_")]
    public class Quest
    {
        [XmlElement(ElementName = "Id")]
        public int id;
        [XmlElement(ElementName = "Name")]
        public string name;
        [XmlElement(ElementName = "Level")]
        public int lvl;
        [XmlElement(ElementName = "Type")]
        public string type;
        [XmlElement(ElementName = "Rarity")]
        public string rarity;
        [XmlElement(ElementName = "Repetability")]
        public string repeated;
        [XmlElement(ElementName = "NbMax")]
        public int playerMax;
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

    internal static class QuestManager
    {
        public static List<Quest> GetQuestBoard()
       {
            List<Quest> allQuests = FileManager.LoadQuests();
            List<Quest> activQuest = new List<Quest>();
            List<Quest> board = new List<Quest>();
            int nbQuest = 6;

            foreach (Quest q in allQuests)
            {
                if (q.status == "Active")
                {
                    activQuest.Add(q);
                }
            }

            if (activQuest.Count <= nbQuest)
            {
                return activQuest;
            }
            else
            {
                int[] indexTaken = {-1, -1, -1, -1, -1, -1};
                Random random = new Random();

                for (int i = 0; i < 6; i++)
                {
                    int index;
                    bool isTaken = true;
                    do
                    {
                        index = random.Next(activQuest.Count);
                        if (!indexTaken.Contains(index))
                        {
                            indexTaken[i] = index;
                            isTaken = false;
                        }

                    } while (isTaken);

                    board.Add(activQuest[index]);
                }

                return board;
            }

        }

        public static Quest GetQuestById(int id)
        {
            Quest res = null;
            List<Quest> allQuests = FileManager.LoadQuests();

            foreach (Quest q in allQuests)
            {
                if (q.id == id)
                {
                    res = q;
                    break;
                }
            }

            return res;
        }
    }
}

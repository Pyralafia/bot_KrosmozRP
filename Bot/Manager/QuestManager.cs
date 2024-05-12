using Bot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bot.Manager
{
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

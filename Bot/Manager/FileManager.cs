using Bot.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Bot.Manager
{
    internal static class FileManager
    {
        public static string GetToken()
        {
            return File.ReadAllText("../../Files/token.txt");
        }

        public static string GetApiKey()
        {
            return File.ReadAllText("../../Files/google.txt");
        }

        public static void SavePlayers(List<Player> playerList)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Player>));

            using (StreamWriter stream = new StreamWriter("../../Files/Players.xml"))
            {
                serializer.Serialize(stream, playerList);
            }
        }

        public static List<Player> LoadPlayers()
        {
            List<Player> res = new List<Player>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Player>));

            try
            {
                using (StreamReader stream = new StreamReader("../../Files/Players.xml"))
                {
                    res = (List<Player>)serializer.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to retrieve player : {e}");
            }

            return res;
        }

        public static ulong[] GetServerIDs()
        {
            ulong[] res = new ulong[2];

            using (FileStream stream = new FileStream("../../Files/DiscordServer.txt", FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        res[i] = ulong.Parse(reader.ReadLine());
                    }
                }
            }

            return res;
        }

        public static List<Quest> LoadQuests()
        {
            List<Quest> res = new List<Quest>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Quest>), new XmlRootAttribute("Quests"));

            try
            {
                using (StreamReader stream = new StreamReader("../../Files/Quetes.xml"))
                {
                    res = (List<Quest>)serializer.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to retrieve quest : {e}");
            }

            return res;
        }

        public static string[] LoadPassifEca()
        {
            string[] res = new string[6];
            using (FileStream stream = new FileStream("../../Files/passif_eca.txt", FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    for (int i = 0; i < 6; i++)
                    {
                        res[i] = reader.ReadLine();
                    }
                }
            }

            return res;
        }

        public static List<CharacterSheet> LoadCharacterSheet()
        {
            List<CharacterSheet> res = new List<CharacterSheet>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<CharacterSheet>), new XmlRootAttribute("Fiches"));

            try
            {
                using (StreamReader stream = new StreamReader("../../Files/CharacterSheet.xml"))
                {
                    res = (List<CharacterSheet>)serializer.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable ton retrieve charactersheet : {e}");
            }

            return res;
        }
    }
}

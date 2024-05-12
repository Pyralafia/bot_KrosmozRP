using Bot.Model;
using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Manager
{
    internal class PlayerManager
    {
        Dictionary<ulong, Player> playerDictio;
        Dictionary<ulong, CharacterSheet> sheetDictio;

        public PlayerManager()
        {
            playerDictio = new Dictionary<ulong, Player>();
            sheetDictio = new Dictionary<ulong, CharacterSheet>();
        }

        #region // Player admin
        public List<Player> GetPlayerList()
        {
            return playerDictio.Values.ToList();
        }

        public void LoadPlayerList(List<Player> playerList)
        {
            foreach (Player p in playerList)
            {
                if (!playerDictio.ContainsKey(p.id))
                {
                    playerDictio.Add(p.id, p);
                }
            }
        }

        public string  RegisterPlayer(ulong id, string link)
        {
            if (!playerDictio.ContainsKey(id))
            {
                Player p = new Player(id, link) ;
                playerDictio.Add(p.id, p);
                FileManager.SavePlayers(playerDictio.Values.ToList());

                return "Player Registered";
            }
            else
            {
                
                return "Player already registered";
            }
        }

        public string GetPlayerSheet(ulong id)
        {
            if (playerDictio.ContainsKey(id))
            {
                return $"<{playerDictio[id].link}>";
            }
            else
            {
                return "Vous n'êtes pas encore enregistrer comme player, ou je bug. Dans les deux cas, contacter la MJ";
            }
        }
        #endregion

        #region // Character Sheet admin
        public void LoadCharacterSheet(List<CharacterSheet> sheetList)
        {
            foreach (CharacterSheet sheet in sheetList)
            {
                if (!sheetDictio.ContainsKey((ushort)sheet.id))
                {
                    sheet.ConvertXmlStringToClass();
                    sheetDictio.Add((ushort)sheet.id, sheet);
                }
            }
        }

        public CharacterSheet GetCharacterSheet(ulong id)
        {
            if (sheetDictio.ContainsKey(id))
            {
                return sheetDictio[id];
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}

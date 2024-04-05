using Bot.Manager;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Command
{
    internal static class Admin
    {
        public static async void RegisterPlayer(SocketSlashCommand command)
        {
            ulong idPlayer = ((IUser)command.Data.Options.First().Value).Id;
            string link = command.Data.Options.Last().Value.ToString();

            string status = BotKrosmozRP.botKrosmoz.PlayerManager.RegisterPlayer(idPlayer, link);

            await MessageManager.SendEphemeral(command, status);
        }


        public static async void SendCharacterSheet(SocketSlashCommand command)
        {
            ulong id = command.User.Id;
            string res = BotKrosmozRP.botKrosmoz.PlayerManager.GetPlayerSheet(id);
            await MessageManager.SendEphemeral(command, res);
        }
    }
}

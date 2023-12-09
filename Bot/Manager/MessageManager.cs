using Bot.Misc;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Manager
{
    internal class MessageManager
    {
        public async Task SendRollAnswer(SocketSlashCommand command, string rollString, bool isGmRoll = false)
        {
            string nbSuccess = rollString.Substring(0, rollString.IndexOf("."));
            string criticalSuccess = rollString.Substring(rollString.IndexOf(".") + 1, rollString.IndexOf(",") - rollString.IndexOf(".") - 1);
            string criticalFailure = rollString.Substring(rollString.IndexOf(",") + 1, rollString.IndexOf("]") - rollString.IndexOf(",") - 1);

            string res = $"{command.User.GlobalName} : \n";

            if (nbSuccess != "0")
            {
                res += $"> **Nb succès : {nbSuccess}**\n";
            }

            res += $">>> Nb succès critique : {criticalSuccess}\n" +
                $"Nb echec critique : {criticalFailure}\n" +
                $"```{rollString.Substring(rollString.IndexOf("]") + 1)}```";

            await command.RespondAsync(res, ephemeral:isGmRoll);
        }

        public async Task SendEcaSlashAnswer(SocketSlashCommand command, string res, int ecaDice)
        {

        }

        public async Task ErrorMessage(SocketSlashCommand command)
        {

        }
    }
}

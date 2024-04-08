using Bot.Misc;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static Bot.Command.Roll;

namespace Bot.Manager
{
    internal static class MessageManager
    {
        public static async Task SendRollAnswer(SocketSlashCommand command, string[] rollString, bool isGmRoll = false)
        {
            string res = $"**{command.User.GlobalName}** → {command.Data.Options.First().Value} \n"+
                $"### {rollString[0]}\n"+
                $"```{rollString[1]}```";

            await command.RespondAsync(res, ephemeral: isGmRoll);
        }

        public static async Task SendRollStatAnswer(SocketSlashCommand command, RollTenRes roll, bool isGmRoll = false)
        {
            string res = $"{command.User.GlobalName} : \n";

            if (roll.nbSuccess != 0)
            {
                res += $"> **Nb succès : {roll.nbSuccess}**\n";
            }

            res += $">>> Nb succès critique : {roll.nbCriticalSuccess}\n" +
                $"Nb echec critique : {roll.nbFailure}\n" +
                $"```{roll.series}```";

            await command.RespondAsync(res, ephemeral:isGmRoll);
        }

        public static async Task SendRollEca(SocketSlashCommand command, string passif, RollTenRes roll, bool isGmRoll = false)
        {
            string res = $"**{command.User.GlobalName}** : \n\n";

            res += $"Passif écaflip : {passif} \n\n";

            if (roll.nbSuccess != 0)
            {
                res += $"> **Nb succès : {roll.nbSuccess}**\n";
            }

            res += $">>> Nb succès critique : {roll.nbCriticalSuccess}\n" +
                $"Nb echec critique : {roll.nbFailure}\n" +
                $"```{roll.series}```";

            await command.RespondAsync(res, ephemeral: isGmRoll);
        }

        public static async Task SendQuestBoard(SocketSlashCommand command, List<Quest> questList)
        {
            ITextChannel channel = (ITextChannel)await BotKrosmozRP.botKrosmoz.Client.GetChannelAsync(BotKrosmozRP.botKrosmoz.QuestChannelId);

            var messages = await channel.GetMessagesAsync(250).FlattenAsync();
            await channel.DeleteMessagesAsync(messages);

            for (int i = 0; i < questList.Count; i++)
            {
                EmbedBuilder embed = new EmbedBuilder()
                    .WithTitle($"[{questList[i].id}] {questList[i].name}")
                    .WithDescription(questList[i].description + "\n ---------------------------------")
                    .WithColor(questList[i].rarity == "Classique" ? Color.LightGrey : Color.Orange);
                embed.AddField("Specifications : ",
                    $"Type : {questList[i].type}\n" +
                    $"Niveau : {questList[i].lvl}\n" +
                    $"Nb recommandé : {questList[i].playerMax}\n" +
                    $"Récompenses monétaire : {questList[i].kamas}\n" +
                    $"Récompenses spécifiques : {questList[i].reward}\n" +
                    "--------------------------------- \n" +
                    "Check si intéressé(e)   \u2705 ");

                if (questList[i].playerName != "")
                {
                    embed.WithFooter($"Quête d'histoire pour {questList[i].playerName}");
                }

                IMessage message = await channel.SendMessageAsync("", embed: embed.Build());
                message.AddReactionAsync(new Emoji("\u2705"));
            }

            await command.RespondAsync("Quests updated with success", ephemeral: true);
        }

        public static async Task SendCreateSession(SocketSlashCommand command, Quest quest)
        {
            ITextChannel channel = (ITextChannel)await BotKrosmozRP.botKrosmoz.Client.GetChannelAsync(BotKrosmozRP.botKrosmoz.OrganisationChannelId);
            var boardMessages = await channel.GetMessagesAsync().FlattenAsync();
            string registeredPlayer = string.Empty;
            IMessage questBoardMessage;
            IMessage sendedMessage;
            IThreadChannel thread;
            List<IUser> users = new List<IUser>();

            foreach (IMessage m in boardMessages)
            {
                string title = m.Embeds.First().Title;
                string extracedId = title.Substring(1, title.IndexOf(']') - 1);

                if (int.Parse(extracedId) == quest.id)
                {
                    questBoardMessage = m;
                    users = (List<IUser>)await questBoardMessage.GetReactionUsersAsync(new Emoji("\u2705"), 1000).FlattenAsync();
                    break;
                }
            }

            string messageContent = "**Ouverture session** \n" +
                $"Quête : __{quest.name}__\n";

            if (command.Data.Options.Count != 1)
            {
                var options = command.Data.Options.ToArray();
                messageContent += "Dédiée : ";

                for (int i = 1; i < command.Data.Options.Count; i++)
                {
                    messageContent += ((IUser)options[i]).Mention + " ";
                }

                messageContent += "\n";
            }

            messageContent += "Inscrits : ";

            foreach (IUser u in users)
            {
                messageContent += $"{u.Mention} ";
                registeredPlayer += $"{u.Mention} ";
            }

            sendedMessage = await channel.SendMessageAsync(messageContent);

            thread = await sendedMessage.Thread.CreateThreadAsync(quest.name, ThreadType.PrivateThread);
            await thread.SendMessageAsync($"Organisation de la séance avec {registeredPlayer} comme inscrit actuellement");

            await command.RespondAsync("Mesage envoyé", ephemeral: true);

        }

        public static async Task SendEphemeral(SocketSlashCommand command, string message)
        {
            await command.RespondAsync(message, ephemeral: true);
        }
    }
}

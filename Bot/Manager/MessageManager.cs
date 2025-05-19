using Bot.Model;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static Bot.Command.Roll;

namespace Bot.Manager
{
    internal static class MessageManager
    {
        //command utilisée pour els roll sans param
        public static async Task SendRollAnswer(SocketSlashCommand command, string rollString, bool isGmRoll = false)
        {
            string res = $"**{command.User.GlobalName}** → \n" +
                $"``` {rollString}```";
            await command.RespondAsync(res, ephemeral: isGmRoll);
        }

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
                $"Nb echec critique : {roll.nbCriticalFailure}\n" +
                $"```{roll.series}```";

            await command.RespondAsync(res, ephemeral:isGmRoll);
        }

        public static async Task SendRollStatEcaAnswer(SocketSlashCommand command, string passif, RollTenRes roll, bool isGmRoll = false)
        {
            string res = $"**{command.User.GlobalName}** : \n\n";
            string splitPassif = passif.Replace("◘", "\n");

            res += $"__Passif écaflip :__ \n{splitPassif} \n\n";

            if (roll.nbSuccess != 0)
            {
                res += $"> **Nb succès : {roll.nbSuccess}**\n";
            }

            res += $">>> Nb succès critique : {roll.nbCriticalSuccess}\n" +
                $"Nb echec critique : {roll.nbCriticalFailure}\n" +
                $"```{roll.series}```";

            await command.RespondAsync(res, ephemeral: isGmRoll);
        }

        public static async Task SendQuestBoard(SocketSlashCommand command, List<Quest> questList)
        {
            ITextChannel channel = (ITextChannel)await BotKrosmozRP.botKrosmoz.Client.GetChannelAsync(BotKrosmozRP.botKrosmoz.QuestChannelId);
            var messages = await channel.GetMessagesAsync(250).FlattenAsync();
            try
            {
                await channel.DeleteMessagesAsync(messages); 
                await channel.SendMessageAsync($"Cher(e)s {MentionUtils.MentionRole(1239254629280256030)}, le tableau des quêtes a été mis à jour.");
                
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
            catch (Exception ex)
            {
                await command.RespondAsync($"{ex.Message}");
            }
        }

        public static async Task SendUniqueQuest(SocketSlashCommand command, Quest quest)
        {

            ITextChannel channel = (ITextChannel)await BotKrosmozRP.botKrosmoz.Client.GetChannelAsync(BotKrosmozRP.botKrosmoz.QuestChannelId);

            EmbedBuilder embed = new EmbedBuilder()
                    .WithTitle($"[{quest.id}] {quest.name}")
                    .WithDescription(quest.description + "\n ---------------------------------")
                    .WithColor(quest.rarity == "Classique" ? Color.LightGrey : Color.Orange);
            embed.AddField("Specifications : ",
                $"Type : {quest.type}\n" +
                $"Niveau : {quest.lvl}\n" +
                $"Nb recommandé : {quest.playerMax}\n" +
                $"Récompenses monétaire : {quest.kamas}\n" +
                $"Récompenses spécifiques : {quest.reward}\n" +
                "--------------------------------- \n" +
                "Check si intéressé(e)   \u2705 ");

            IMessage message = await channel.SendMessageAsync("", embed: embed.Build());
            message.AddReactionAsync(new Emoji("\u2705"));
            await command.RespondAsync("Quest created", ephemeral: true);
        }

        public static async Task SendCreateSession(SocketSlashCommand command, Quest quest)
        {
            ITextChannel orgChannel = (ITextChannel)await BotKrosmozRP.botKrosmoz.Client.GetChannelAsync(BotKrosmozRP.botKrosmoz.OrganisationChannelId);
            ITextChannel questChannel = (ITextChannel)await BotKrosmozRP.botKrosmoz.Client.GetChannelAsync(BotKrosmozRP.botKrosmoz.QuestChannelId);
            var boardMessages = await questChannel.GetMessagesAsync().FlattenAsync();
            string registeredPlayer = string.Empty;
            IMessage questBoardMessage;
            IMessage sendedMessage;
            IThreadChannel thread;
            IEnumerable<IUser> userEnumarable;

            string messageContent = "**Ouverture session** \n" +
                $"Quête : __{quest.name}__\n";

            foreach (IMessage m in boardMessages)
            {
                Embed embed = (Embed)m.Embeds.First();
                string title = m.Embeds.First().Title;
                string extractedId = title.Substring(1, title.IndexOf(']') - 1);

                if (extractedId == quest.id.ToString())
                {
                    questBoardMessage = m;
                    userEnumarable = await questBoardMessage.GetReactionUsersAsync(new Emoji("\u2705"), 1000).FlattenAsync(); 

                    if (command.Data.Options.Count != 1)
                    {
                        var options = command.Data.Options.ToArray();
                        messageContent += "Dédiée : ";

                        for (int i = 1; i < command.Data.Options.Count; i++)
                        {
                            messageContent += ((IUser)options[i].Value).Mention + " ";
                        }

                        messageContent += "\n";
                    }

                    foreach (IUser u in userEnumarable)
                    {
                        if (!u.IsBot)
                        {
                            registeredPlayer += $"{u.Mention} ";
                        }
                    }

                    sendedMessage = await orgChannel.SendMessageAsync(messageContent);
                    thread = await orgChannel.CreateThreadAsync(quest.name, message:sendedMessage);
                    await thread.SendMessageAsync($"Organisation de la séance avec {registeredPlayer} comme inscrit actuellement.\n\n" +
                        $"**Résumé** : *{quest.description}* \n\n" +
                        $"Maitre(sse) de jeu prévue : {BotKrosmozRP.botKrosmoz.Pyra.Mention}");
                    
                    await command.RespondAsync("Mesage envoyé", ephemeral: true);
                    break;
                }
            }


            

        }

        public static async Task SendEphemeral(SocketSlashCommand command, string message)
        {
            await command.RespondAsync(message, ephemeral: true);
        }
    }
}

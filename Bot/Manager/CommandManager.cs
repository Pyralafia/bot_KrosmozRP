using Bot.Misc;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Manager
{
    internal class CommandManager
    {
        private SocketGuild _guild;

        public void SetGuild(SocketGuild guild) { _guild = guild; }

        public void SetupCommand()
        {
            CommandOption scoreSuccess = new CommandOption("success", ApplicationCommandOptionType.Integer, "[Optional] Le score de succès", false);
            CommandOption gmScoreSuccess = new CommandOption("success", ApplicationCommandOptionType.Integer, "Le score de succès");
            CommandOption nbDice = new CommandOption("nb_dés", ApplicationCommandOptionType.Integer, "Le nombre de d10 à lancer");
            CommandOption nbDiceBonus = new CommandOption("dés_modif", ApplicationCommandOptionType.Integer, "Le nombre de dé bonus ou malus (-X si malus)", false);
            CommandOption idMob = new CommandOption("id_mob", ApplicationCommandOptionType.Integer, "ID du mob à ajouter");
            CommandOption nbMob = new CommandOption("nb_mob", ApplicationCommandOptionType.Integer, "Nombre de mob à ajouter");
            CommandOption idQuest = new CommandOption("id_quest", ApplicationCommandOptionType.Integer, "ID de la quete");

            SlashCommandBuild("roll", "Lance X dés à 10 faces", options: new CommandOption[] { nbDice, scoreSuccess });
            SlashCommandBuild("gmroll", "Roll as ephemeral", true, new CommandOption[] { nbDice, gmScoreSuccess });
            SlashCommandBuild("rollsag", "Faire un test de sagesse", options: new CommandOption[] { nbDiceBonus });
            SlashCommandBuild("rollagi", "Faire un test d'agilité", options: new CommandOption[] { nbDiceBonus });
            SlashCommandBuild("rollcha", "Faire un test de chance", options: new CommandOption[] { nbDiceBonus });
            SlashCommandBuild("rollfor", "Faire un test de force", options: new CommandOption[] { nbDiceBonus });
            SlashCommandBuild("rollint", "Faire un test d'intelligence", options: new CommandOption[] { nbDiceBonus });
            SlashCommandBuild("createfight", "Create a fight", true);
            SlashCommandBuild("addmob", "Add a mob to the current fight", true, options: new CommandOption[] { idMob, nbMob });
            SlashCommandBuild("closefight", "Close the actual fight", true);
            SlashCommandBuild("closequest", "Close a quest", true, options: new CommandOption[] { idQuest });
        }

        private async void SlashCommandBuild(string name, string description, bool isForGM = false, params CommandOption[] options)
        {
            SlashCommandBuilder command = new SlashCommandBuilder();

            command.WithName(name);
            command.WithDescription(description);

            if (isForGM)
            {
                command.WithDefaultMemberPermissions(GuildPermission.PrioritySpeaker);
            }

            for (int i = 0; i < options.Length; i++)
            {
                command.AddOption(options[i].Name, options[i].Type, options[i].Description, options[i].IsRequired);
            }

            try
            {
                await _guild.CreateApplicationCommandAsync(command.Build());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "roll":
                    RollDice(command);
                    break;
                case "gmroll":
                    GmRoll(command);
                    break;
                case "rollsag":
                case "rollagi":
                case "rollcha":
                case "rollfor":
                case "rollint":
                    await command.RespondAsync($"Commande non disponible pour le moment, un peu de patience !", ephemeral: true);
                    break;
                default:
                    break;
            }
        }

        private int[] ProcessRollCommand(SocketSlashCommand command)
        {
            int[] res = new int[2];
            res[0] = int.Parse(command.Data.Options.First().Value.ToString());
            res[1] = -1;

            if (command.Data.Options.Count == 2)
            {
                res[1] = int.Parse(command.Data.Options.Last().Value.ToString());
            }

            return res;
        }

        private async void RollDice(SocketSlashCommand command)
        {
            int[] commandOptions = ProcessRollCommand(command);
            await BotKrosmozRP.botKrosmoz.MessageManager.SendRollAnswer(command, Roll.RollDices(commandOptions[0], commandOptions[1]));
        }

        private async void GmRoll(SocketSlashCommand command)
        {
            int[] commandOptions = ProcessRollCommand(command);
            await BotKrosmozRP.botKrosmoz.MessageManager.SendRollAnswer(command, Roll.RollDices(commandOptions[0], commandOptions[1]), true);
        }
    }
}

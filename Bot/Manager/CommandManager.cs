using Bot.Command;
using Bot.Model;
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
            CommandOption diceString = new CommandOption("roll", ApplicationCommandOptionType.String, "Le roll à effectué au format XdY+Z (où +Z est optionnel)", false);
            CommandOption nbDice = new CommandOption("nb_dés", ApplicationCommandOptionType.Integer, "Le nombre de d10 à lancer");
            CommandOption nbDiceBonus = new CommandOption("dés_modif", ApplicationCommandOptionType.Integer, "Le nombre de dé bonus ou malus (-X si malus)", false);
            CommandOption successValue = new CommandOption("success_value", ApplicationCommandOptionType.Integer, "La valeur de succès annoncée par la MJ", false);
            CommandOption player = new CommandOption("player", ApplicationCommandOptionType.User, "Le joueur à enregistrer");
            CommandOption linkString = new CommandOption("link_charactersheet", ApplicationCommandOptionType.String, "Le lien complet vers la fiche personnage");

            //roll command
            SlashCommandBuild("roll", "Lancer de dé au format classique XdY+Z", options: new CommandOption[] { diceString });
            SlashCommandBuild("rollstat", "Faire un test de Xd10", options: new CommandOption[] { nbDice });
            
            SlashCommandBuild("rollsag", "Faire un test de sagesse", options: new CommandOption[] { nbDiceBonus });
            SlashCommandBuild("rollagi", "Faire un test d'agilité", options: new CommandOption[] { nbDiceBonus });
            SlashCommandBuild("rollcha", "Faire un test de chance", options: new CommandOption[] { nbDiceBonus });
            SlashCommandBuild("rollfor", "Faire un test de force", options: new CommandOption[] { nbDiceBonus });
            SlashCommandBuild("rollint", "Faire un test d'intelligence", options: new CommandOption[] { nbDiceBonus });

            //Sheet command
            SlashCommandBuild("sheet", "Vous renvois le lien vers votre fiche de personnage");

            //gm command
            SlashCommandBuild("gmroll", "Lancer de dé au format classique XdY", true, options: new CommandOption[] { diceString });
            SlashCommandBuild("gmrollstat", "Faire un test de Xd10", true, options: new CommandOption[] { nbDice });
            SlashCommandBuild("register", "enregistrer un joueur", true, options: new CommandOption[] { player, linkString });
            SlashCommandBuild("updatequest", "Met à jour le tableau des quêtes", true);
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
            string stats = "";
            switch (command.Data.Name)
            {
                case "sheet":
                    Admin.SendCharacterSheet(command);
                    break;

                case "roll":
                    if (AllDiceFormatChecker(command))
                    {
                        await MessageManager.SendRollAnswer(command, Roll.RollAllDices(command));
                    }
                    break;

                case "gmroll":
                    if (AllDiceFormatChecker(command))
                    {
                        await MessageManager.SendRollAnswer(command, Roll.RollAllDices(command), true);
                    }
                    break;

                case "rollsag":
                case "rollagi":
                case "rollcha":
                case "rollfor":
                case "rollint":
                    if (BotKrosmozRP.botKrosmoz.PlayerManager.GetCharacterSheet(command.User.Id).classe == Classes.Ecaflip)
                    {
                        (string passif, Roll.RollTenRes res) = Roll.RollEcaStat(command);
                        await MessageManager.SendRollStatEcaAnswer(command, passif, res);
                    }
                    else
                    {
                        await MessageManager.SendRollStatAnswer(command, Roll.RollStats(command));
                    }
                    break;

                case "rollstat":
                    await MessageManager.SendRollStatAnswer(command, Roll.RollTenDice(command));
                    break;

                case "gmrollstat":
                    await MessageManager.SendRollStatAnswer(command, Roll.RollTenDice(command), true);
                    break;

                case "gmrolleca":
                    if (AllDiceFormatChecker(command))
                    {
                        Roll.RollTenEca(command);
                    }
                    break;

                case "register":
                    Admin.RegisterPlayer(command);
                    break;

                case "updatequest":
                    await MessageManager.SendQuestBoard(command, QuestManager.GetQuestBoard());
                    break;

                default:
                    break;
            }
        }

        private bool AllDiceFormatChecker(SocketSlashCommand command)
        {
            bool res = false;

            try
            {
                string commandOption = command.Data.Options.First().Value.ToString().ToLower();
                int indexOfD = commandOption.IndexOf('d');
                int indexOfPlus = commandOption.IndexOf("+");
                string rightSection = indexOfPlus != -1 ? commandOption.Substring(indexOfD + 1, indexOfPlus - indexOfD - 1) : commandOption.Substring(indexOfD + 1);

                if (indexOfD > 0)
                {
                    int.Parse(commandOption.Substring(0, indexOfD ));
                    int.Parse(rightSection);

                    res = true;
                }
            }
            catch (Exception)
            {
                MessageManager.SendEphemeral(command, "Merci d'utiliser le format XdY où X est le nombre de dés à lancés et Y le nombre de face de ces dés.");
            }

            return res;
        }
    }
}

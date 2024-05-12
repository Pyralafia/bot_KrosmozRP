using Bot.Command;
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
            CommandOption diceString = new CommandOption("roll", ApplicationCommandOptionType.String, "Le roll à effectué au format XdY+Z (où +Z est optionnel)", false);
            CommandOption nbDice = new CommandOption("nb_dés", ApplicationCommandOptionType.Integer, "Le nombre de d10 à lancer");
            CommandOption nbDiceBonus = new CommandOption("dés_modif", ApplicationCommandOptionType.Integer, "Le nombre de dé bonus ou malus (-X si malus)", false);
            CommandOption successValue = new CommandOption("success_value", ApplicationCommandOptionType.Integer, "La valeur de succès annoncée par la MJ", false);
            CommandOption player = new CommandOption("player", ApplicationCommandOptionType.User, "Le tag du joueur");
            CommandOption playerDedicated = new CommandOption("player", ApplicationCommandOptionType.User, "Le tag du joueur", false);
            CommandOption playerDedicatedTwo = new CommandOption("player2", ApplicationCommandOptionType.User, "Le tag du joueur", false);
            CommandOption playerDedicatedThree = new CommandOption("player3", ApplicationCommandOptionType.User, "Le tag du joueur", false);
            CommandOption linkString = new CommandOption("link_charactersheet", ApplicationCommandOptionType.String, "Le lien complet vers la fiche personnage");
            CommandOption questId = new CommandOption("quest_id", ApplicationCommandOptionType.Integer, "L'ID de la quete concernée");
                 
            //roll command
            SlashCommandBuild("roll", "Lancer de dé au format classique XdY+Z", options: new CommandOption[] { diceString });
            SlashCommandBuild("rollstat", "Faire un test de Xd10", options: new CommandOption[] { nbDice });
            /*
            SlashCommandBuild("rollsag", "Faire un test de sagesse", options: new CommandOption[] { nbDiceBonus });
            SlashCommandBuild("rollagi", "Faire un test d'agilité", options: new CommandOption[] { nbDiceBonus });
            SlashCommandBuild("rollcha", "Faire un test de chance", options: new CommandOption[] { nbDiceBonus });
            SlashCommandBuild("rollfor", "Faire un test de force", options: new CommandOption[] { nbDiceBonus });
            SlashCommandBuild("rollint", "Faire un test d'intelligence", options: new CommandOption[] { nbDiceBonus });
            SlashCommandBuild("rolleca", "Faire un test de Xd10 en étant un(e) Ecaflip", options: new CommandOption[] { nbDice });
            */

            //misc command
            SlashCommandBuild("sheet", "Vous renvois le lien vers votre fiche de personnage");

            //gm command
            SlashCommandBuild("gmroll", "Lancer de dé au format classique XdY", true, options: new CommandOption[] { diceString });
            SlashCommandBuild("gmrollstat", "Faire un test de Xd10", true, options: new CommandOption[] { nbDice });
            SlashCommandBuild("register", "enregistrer un joueur", true, options: new CommandOption[] { player, linkString });
            SlashCommandBuild("updatrequest", "Met à jour le tableau des quêtes", true);
            SlashCommandBuild("createquest", "Créé une quete à partir de son idée et l'ajoute au tableau de quête", true, options: new CommandOption[] { questId });
            SlashCommandBuild("createsession", "Créé un le message et le thread pour l'organisation d'une session", 
                                true, new CommandOption[] { questId, playerDedicated, playerDedicatedTwo, playerDedicatedThree });

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
                Console.WriteLine($"Error creating '{name}' : {e.Message}");
            }
        }

        public async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "sheet":
                    ProcessSheet(command);
                    break;

                case "roll":
                    if (AllDiceFormatChecker(command))
                    {
                        Roll.RollAllDices(command);
                    }
                    break;

                case "gmroll":
                    if (AllDiceFormatChecker(command))
                    {
                        Roll.RollAllDices(command, true);
                    }
                    break;

                case "rollsag":
                case "rollagi":
                case "rollcha":
                case "rollfor":
                case "rollint":
                    await command.RespondAsync($"Commande non disponible pour le moment, un peu de patience !", ephemeral: true);
                    break;

                case "rollstat":
                    await MessageManager.SendRollStatAnswer(command, Roll.RollTenDice(int.Parse(command.Data.Options.First().Value.ToString())));
                    break;

                case "gmrollstat":
                    await MessageManager.SendRollStatAnswer(command, Roll.RollTenDice(int.Parse(command.Data.Options.First().Value.ToString())), true);
                    break;

                case "rolleca":
                    ProcessEcaRoll(command);
                    break;

                case "gmrolleca":
                    ProcessEcaRoll(command, true);
                    break;

                case "register":
                    ProcessRegisterPlayer(command);
                    break;

                case "updatequest":
                    await MessageManager.SendQuestBoard(command, QuestManager.GetQuestBoard());
                    break;

                case "createsession":
                    await MessageManager.SendCreateSession(command, QuestManager.GetQuestById(int.Parse(command.Data.Options.First().Value.ToString())));
                    break;

                case "createquest":
                    await MessageManager.SendUniqueQuest(command, QuestManager.GetQuestById(int.Parse(command.Data.Options.First().Value.ToString())));
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

        private async void ProcessEcaRoll(SocketSlashCommand command, bool isGm = false)
        {
            (string passif, Roll.RollTenRes roll) = Roll.RollEca(int.Parse(command.Data.Options.First().Value.ToString()));
            await MessageManager.SendRollEca(command, passif, roll, isGm);
        }

        private async void ProcessRegisterPlayer(SocketSlashCommand command)
        {
            ulong idPlayer = ((IUser)command.Data.Options.First().Value).Id;
            string link = command.Data.Options.Last().Value.ToString();

            string status = BotKrosmozRP.botKrosmoz.PlayerManager.RegisterPlayer(idPlayer, link);

            await MessageManager.SendEphemeral(command, status);
        }

        private async void ProcessSheet(SocketSlashCommand command)
        {
            ulong id = command.User.Id;
            string res = BotKrosmozRP.botKrosmoz.PlayerManager.GetPlayerSheet(id);
            await MessageManager.SendEphemeral(command, res);
        }
    }
}

using Bot.Manager;
using Bot.Model;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Bot.Command
{
    internal static class Roll
    {
        public struct RollTenRes
        {
            public string series;
            public int nbSuccess;
            public int nbCriticalSuccess;
            public int nbCriticalFailure;
        }

        public static string[] RollAllDices(SocketSlashCommand command)
        {
            Random random = new Random();
            string options = command.Data.Options.First().Value.ToString().ToLower();
            string[] resString = { "", "" };
            int indexOfd = options.IndexOf("d");
            int indexOfPlus = options.IndexOf("+");
            int nbDice = int.Parse(options.Substring(0, indexOfd));
            int maxDice = indexOfPlus != -1 ? int.Parse(options.Substring(indexOfd + 1, indexOfPlus - indexOfd - 1)) : int.Parse(options.Substring(indexOfd +1));
            int additionnal = indexOfPlus == -1 ? 0 : int.Parse(options.Substring(indexOfPlus + 1));
            int total = 0;

            for (int i = 0; i < nbDice; i++)
            {
                int dice = random.Next(1, maxDice+1);
                total += dice;
                resString[1] += $"{dice} ";
            }

            total += additionnal;
            resString[0] = total.ToString();

            return resString;
        }

        private static RollTenRes RollTen(int nbDice, int scoreSuccess = -1)
        {
            Random rand = new Random();
            RollTenRes res = new RollTenRes();

            for (int i = 0; i < nbDice; i++)
            {
                int roll = rand.Next(1, 11);

                if (roll <= scoreSuccess)
                {
                    res.nbSuccess++;
                }

                if (roll == 1)
                {
                    res.nbCriticalSuccess++;
                }
                else if (roll == 10)
                {
                    res.nbCriticalFailure++;
                }

                res.series += $" {roll} ";
            }
            return res;
        }

        public static RollTenRes RollTenDice(SocketSlashCommand command)
        {
            int nbDice = int.Parse(command.Data.Options.First().Value.ToString());
            int scoreSuccess = command.Data.Options.Count() > 1 ? int.Parse(command.Data.Options.Last().Value.ToString()) : -1;

            return RollTen(nbDice, scoreSuccess);
        }

        public static RollTenRes RollStats(SocketSlashCommand command)
        {
            int nbDice = -1;
            int scoreSuccess = command.Data.Options.Count() > 1 ? int.Parse(command.Data.Options.Last().Value.ToString()) : -1;
            CharacterSheet sheet = BotKrosmozRP.botKrosmoz.PlayerManager.GetCharacterSheet(command.User.Id);

            switch (command.Data.Name)
            {
                case "rollsag":
                    nbDice = sheet.wisdom;
                    break;
                case "rollagi":
                    nbDice = sheet.agility;
                    break;
                case "rollcha":
                    nbDice = sheet.luck;
                    break;
                case "rollfor":
                    nbDice = sheet.strength;
                    break;
                case "rollint":
                    nbDice = sheet.intelligence;
                    break;
            }

            return RollTen(nbDice, scoreSuccess);
        }

        public static (string passif, RollTenRes resRoll) RollEcaStat(SocketSlashCommand command)
        {
            Random rand = new Random();
            RollTenRes resRoll = RollStats(command);
            string resPassif = BotKrosmozRP.botKrosmoz.PassifEca[rand.Next(BotKrosmozRP.botKrosmoz.PassifEca.Length)];

            return (resPassif, resRoll);
        }

        public static (string eca, RollTenRes roll) RollTenEca(SocketSlashCommand command)
        {
            Random random = new Random();
            RollTenRes resRoll = RollTenDice(command);

            string resPassif = BotKrosmozRP.botKrosmoz.PassifEca[random.Next(BotKrosmozRP.botKrosmoz.PassifEca.Length)];

            return (resPassif, resRoll);
        }

        // 34% to gain between 10 and 100, return 0 if the sram don't get anything
        private static int RollSram()
        {
            Random rand = new Random();
            int res = 0;

            if (rand.NextDouble() > 0.66)
            {
                res = new Random().Next(10, 100);
            }

            return res;
        }

        // 25% to not lose the invisibility
        private static bool RollSournoiserie()
        {
            Random rand = new Random();
            return rand.NextDouble() > 0.75;
        }
    }
}

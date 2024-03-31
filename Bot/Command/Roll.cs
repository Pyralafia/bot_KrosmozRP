using Bot.Manager;
using Bot.Misc;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Command
{
    internal static class Roll
    {
        public struct RollTenRes
        {
            public string series;
            public string ecaRoll;
            public int nbSuccess;
            public int nbCriticalSuccess;
            public int nbFailure;
        }

        public static async void RollAllDices(SocketSlashCommand command, bool asGm = false)
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

            await MessageManager.SendRollAnswer(command, resString, asGm);
        }

        public static RollTenRes RollTenDice(int nbDice, int scoreSuccess = -1)
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
                    res.nbFailure++;
                }

                res.series += $" {roll} ";
            }

            return res;
        }

        public static (string eca, RollTenRes roll) RollEca(int nbDice, int scoreSuccess = -1)
        {
            Random random = new Random();
            RollTenRes resRoll = RollTenDice(nbDice, scoreSuccess);

            string resString = BotKrosmozRP.botKrosmoz.PassifEca[random.Next(6)];

            return (resString, resRoll);
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

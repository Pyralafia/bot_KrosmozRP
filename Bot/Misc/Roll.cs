using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Misc
{
    static class Roll
    {
        public static string RollDices(int nbDice, int scoreSuccess = -1)
        {
            Random rand = new Random();
            string res = "";
            int[] rollResult = { 0, 0, 0};

            for (int i = 0; i < nbDice; i++)
            {
                int roll = rand.Next(1, 11);

                if (roll <= scoreSuccess)
                {
                    rollResult[0]++;
                }

                if (roll == 1)
                {
                    rollResult[1] ++;
                }
                else if(roll == 10)
                {
                    rollResult[2]++;
                }

                res += $" {roll} ";
            }

            res = $"{rollResult[0]}.{rollResult[1]},{rollResult[2]}]" + res;

            return res;
        }

        // 34% to gain between 10 and 100, return 0 if the sram don't get anything
        public static int RollSram()
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
        public static bool RollSournoiserie()
        {
            Random rand = new Random();
            return rand.NextDouble() > 0.75;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Misc
{
    internal class PassivEffect
    {
        public string inCbt;
        public string outCbt;
    }

    internal class PassifEca
    {
        public List<PassivEffect> effectList = new List<PassivEffect>();

        public string GetRandomEffect()
        {
            Random random = new Random();
            PassivEffect randomEffect = effectList[random.Next(effectList.Count)];

            return BotKrosmozRP.botKrosmoz.FightManager.isFightActiv ? randomEffect.inCbt : randomEffect.outCbt;
        }
    }
}

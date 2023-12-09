using Bot.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Manager
{
    internal class FightManager
    {
        public bool isFightActiv = false;
        public List<MobFight> mobList = new List<MobFight>();


        public void CreateFight()
        {
            isFightActiv = true;
        }

        public void Drop()
        {

        }

        public void CloseFight()
        {

        }
    }
}

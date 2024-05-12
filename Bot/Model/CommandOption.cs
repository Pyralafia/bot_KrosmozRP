using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Model
{
    internal class CommandOption
    {
        string name;
        ApplicationCommandOptionType type;
        string description;
        bool isRequired;

        public string Name { get { return name; } }
        public ApplicationCommandOptionType Type { get { return type; } }
        public string Description { get { return description; } }
        public bool IsRequired { get { return isRequired; } }

        public CommandOption(string name, ApplicationCommandOptionType type, string description, bool isRequired = true)
        {
            this.name = name;
            this.type = type;
            this.description = description;
            this.isRequired = isRequired;
        }
    }
}

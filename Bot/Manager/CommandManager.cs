using Discord;
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

        }

        public async Task SlashCommandHandler(SocketSlashCommand command)
        {

        }

        private async void SlashCommandBuild(string name, string description)
        {
            SlashCommandBuilder command = new SlashCommandBuilder();
            command.WithName(name);
            command.WithDescription(description);

            try
            {
                await _guild.CreateApplicationCommandAsync(command.Build());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

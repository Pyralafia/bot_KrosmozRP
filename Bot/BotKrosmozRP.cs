using Bot.Manager;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot
{
    internal class BotKrosmozRP
    {
        public static BotKrosmozRP botKrosmoz;

        private DiscordSocketClient _client;
        private MessageManager _messageManager;
        private CommandManager _commandManager;
        private ButtonManager _buttonManager;


        public DiscordSocketClient Client { get { return _client; } }


        public static Task Main(string[] args) => new BotKrosmozRP().MainAsync();

        public async Task MainAsync()
        {
            string token = FileManager.GetToken();

            botKrosmoz = this;

            _messageManager = new MessageManager();
            _commandManager = new CommandManager();
            _buttonManager = new ButtonManager();
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.Ready += ClientReady;
            _client.SlashCommandExecuted += _commandManager.SlashCommandHandler;
            _client.ButtonExecuted += _buttonManager.ButtonHandler;

            await _client.LoginAsync(TokenType.Bot, token);
            await Task.Delay(-1);
        }


        public Task ClientReady()
        {
            _commandManager.SetGuild(_client.GetGuild(1120772394777325680));
            _commandManager.SetupCommand();

            return Task.CompletedTask;
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}

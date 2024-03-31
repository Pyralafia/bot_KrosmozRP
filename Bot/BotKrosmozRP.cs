using Bot.Manager;
using Bot.Misc;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Bot
{
    internal class BotKrosmozRP
    {
        public static BotKrosmozRP botKrosmoz;

        private DiscordSocketClient _client;
        private CommandManager _commandManager;
        private ButtonManager _buttonManager;
        private PlayerManager _playerManager;

        private ulong guildId;
        private ulong questChannelId;
        private string[] passifEca;

        public DiscordSocketClient Client { get { return _client; } }
        public PlayerManager PlayerManager { get { return _playerManager; } }

        public ulong GuildId { get { return guildId; } }
        public ulong QuestChannelId { get { return questChannelId; } }

        public string[] PassifEca { get { return passifEca; } }

        public static Task Main(string[] args) => new BotKrosmozRP().MainAsync();

        public async Task MainAsync()
        {
            string token = FileManager.GetToken();


            botKrosmoz = this;

            _commandManager = new CommandManager();
            _buttonManager = new ButtonManager();
            _playerManager = new PlayerManager();
            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.Ready += ClientReady;
            _client.SlashCommandExecuted += _commandManager.SlashCommandHandler;
            _client.ButtonExecuted += _buttonManager.ButtonHandler;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        public Task ClientReady()
        {
            ulong[] serverInfo = FileManager.GetServerIDs();

            guildId = serverInfo[0];
            questChannelId = serverInfo[1];

            passifEca = FileManager.LoadPassifEca();
            _playerManager.LoadPlayerList(FileManager.LoadPlayers());

            _commandManager.SetGuild(_client.GetGuild(guildId));
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

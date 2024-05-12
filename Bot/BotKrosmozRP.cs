using Bot.Manager;
using Bot.Model;
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
        private IUser _pyralafia;

        private ulong _guildId;
        private ulong _questChannelId;
        private ulong _organisationChannelId;
        private string[] _passifEca;

        public DiscordSocketClient Client { get { return _client; } }
        public PlayerManager PlayerManager { get { return _playerManager; } }

        public ulong GuildId { get { return _guildId; } }
        public ulong QuestChannelId { get { return _questChannelId; } }
        public ulong OrganisationChannelId { get { return _organisationChannelId; } }

        public IUser Pyra { get { return _pyralafia; } }

        public string[] PassifEca { get { return _passifEca; } }

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

        public async Task<Task> ClientReady()
        {
            ulong[] serverInfo = FileManager.GetServerIDs();

            _guildId = serverInfo[0];
            _questChannelId = serverInfo[1];
            _organisationChannelId = serverInfo[2];

            _passifEca = FileManager.LoadPassifEca();
            _playerManager.LoadPlayerList(FileManager.LoadPlayers());
            _playerManager.LoadCharacterSheet(FileManager.LoadCharacterSheet());

            _commandManager.SetGuild(_client.GetGuild(guildId));
            _commandManager.SetupCommand();
            
			_pyralafia = await Client.GetUserAsync(727970687095930991);
			
            Console.WriteLine("Loading done.");
            return Task.CompletedTask;
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}

using Discord;
using Discord.WebSocket;
using DiscordNerfWatcher.Application.Events;
using DiscordNerfWatcher.Bot.Modules;
using MediatR;

namespace DiscordNerfWatcher.Bot
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly DiscordSocketClient _client;
        private readonly IModuleHandler _moduleHandler;
        private readonly IPublisher _publisher;
        private readonly IConfiguration _configuration;


        public Worker(ILogger<Worker> logger, IDiscordClient discordSocketClient, IModuleHandler moduleHandler, IPublisher publisher, IConfiguration configuration)
        {
            _logger = logger;
            _client = (DiscordSocketClient)discordSocketClient;
            _moduleHandler = moduleHandler;
            _publisher = publisher;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var task = MainAsync(stoppingToken);
            await task;
        }
        public async Task MainAsync(CancellationToken stoppingToken)
        {
            _client.Ready += async () =>
            {
                await Task.Run(() => _logger.LogInformation($"Bot is connected and ready!"));
                await _moduleHandler.InitializeAsync();

            };
            _client.MessageReceived += async (message) =>
            {
                var MessageReceiveEvent = new OnMessageReceiveEvent
                {
                    ChannelId = message.Channel.Id,
                    MessageAuthorId = message.Author.Id,
                    MessageContent = message.Content,
                    MessageId = message.Id
                };
                await _publisher.Publish(MessageReceiveEvent);
            };
            _client.Log += Log;


            var token = _configuration.GetValue<string>("Discord:Token");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.

            await Task.Delay(Timeout.Infinite).WaitAsync(stoppingToken);
        }
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

    }
}
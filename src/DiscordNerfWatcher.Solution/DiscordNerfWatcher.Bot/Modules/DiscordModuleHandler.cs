using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;

namespace DiscordNerfWatcher.Bot.Modules
{
    public class DiscordModuleHandler : IModuleHandler
    {
        private readonly InteractionService _interactionService;
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DiscordModuleHandler> _logger;
        public DiscordModuleHandler(InteractionService interactionService, IDiscordClient DiscordSocketClient, IServiceProvider serviceProvider, ILogger<DiscordModuleHandler> logger)
        {
            _interactionService = interactionService;
            _discordSocketClient = (DiscordSocketClient)DiscordSocketClient;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        public async Task InitializeAsync()
        {
            await _interactionService.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);

            await _interactionService.RegisterCommandsGloballyAsync(true);

            // process the InteractionCreated payloads to execute Interactions commands
            _discordSocketClient.InteractionCreated += HandleInteraction;

            // process the command execution results 
            _interactionService.SlashCommandExecuted += SlashCommandExecuted;
            _interactionService.ContextCommandExecuted += ContextCommandExecuted;
            _interactionService.ComponentCommandExecuted += ComponentCommandExecuted;
        }
        private Task ComponentCommandExecuted(ComponentCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                    case InteractionCommandError.UnknownCommand:
                    case InteractionCommandError.BadArgs:
                    case InteractionCommandError.Exception:
                    case InteractionCommandError.Unsuccessful:
                    default:
                        _logger.LogError($"{arg3.Error} {arg3.ErrorReason}");
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private Task ContextCommandExecuted(ContextCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                    case InteractionCommandError.UnknownCommand:
                    case InteractionCommandError.BadArgs:
                    case InteractionCommandError.Exception:
                    case InteractionCommandError.Unsuccessful:
                    default:
                        _logger.LogError($"{arg3.Error} {arg3.ErrorReason}");
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private Task SlashCommandExecuted(SlashCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                    case InteractionCommandError.UnknownCommand:
                    case InteractionCommandError.BadArgs:
                    case InteractionCommandError.Exception:
                    case InteractionCommandError.Unsuccessful:
                    default:
                        _logger.LogError($"{arg3.Error} {arg3.ErrorReason}");
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private async Task HandleInteraction(SocketInteraction arg)
        {
            try
            {


                // We don't want the bot to respond to itself or other bots.
                if (arg.User.Id == _discordSocketClient.CurrentUser.Id || arg.User.IsBot)
                    return;
                // create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules
                var ctx = new SocketInteractionContext(_discordSocketClient, arg);

                await _interactionService.ExecuteCommandAsync(ctx, _serviceProvider);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // if a Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
                // response, or at least let the user know that something went wrong during the command execution.
                if (arg.Type == InteractionType.ApplicationCommand)
                {
                    await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
                }
            }
        }
    }
}

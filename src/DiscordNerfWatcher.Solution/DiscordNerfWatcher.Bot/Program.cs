using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordNerfWatcher.Application.Behaviors;
using DiscordNerfWatcher.Application.Requests.Commands;
using DiscordNerfWatcher.Application.Settings;
using DiscordNerfWatcher.Bot;
using DiscordNerfWatcher.Bot.Modules;
using DiscordNerfWatcher.Bot.Services.Stores;
using DiscordNerfWatcher.Database;
using DiscordNerfWatcher.EntityFramework.Services;
using MediatR;
using System.Reflection;


var logLevelMapper = new Dictionary<LogLevel, LogSeverity>()
{
    { LogLevel.Information,LogSeverity.Info },
    { LogLevel.Error,LogSeverity.Error },
    { LogLevel.Warning,LogSeverity.Warning },
    { LogLevel.Trace,LogSeverity.Verbose },
    { LogLevel.Debug,LogSeverity.Debug }
};

IHost host = Host.CreateDefaultBuilder(args)
     .ConfigureAppConfiguration(config =>
     {
         config.AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();
     }).ConfigureLogging((context, logging) =>
     {
         var env = context.HostingEnvironment;
         var config = context.Configuration.GetSection("Logging");
         // ...
         logging.AddConfiguration(config);
         logging.AddConsole();
         // ...
         logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Error);
     })
    .ConfigureServices((hostContext, services) =>
    {
        var configLogger = hostContext.Configuration.GetSection("Logging");
        var logLevel = Enum.Parse<LogLevel>(configLogger.GetValue<string>("LogLevel:Default"));

        var discordLogSeverity = logLevelMapper[logLevel];

        services.Configure<CacheSettings>(hostContext.Configuration.GetSection("CacheSettings"));
        services.AddMemoryCache().ConfigureDNWDatabase(hostContext.Configuration);
        services.AddHostedService<Worker>();
        var config = new DiscordSocketConfig()
        {
            AlwaysDownloadUsers = true,
            GatewayIntents =
            GatewayIntents.All,
            LogLevel = discordLogSeverity,
            UseInteractionSnowflakeDate = false,


        };
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(BlockUserFromChannelCommand).Assembly, Assembly.GetExecutingAssembly()))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheInvalidatorBehavior<,>))
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>))
        .AddTransient<IChannelUserBlockRepository, EFChannelUserBlockRepository>();
        var client = new DiscordSocketClient(config);
        var interactionService = new InteractionService(client, new InteractionServiceConfig()
        {
            // Again, log level:
            LogLevel = discordLogSeverity

        });
        services.AddSingleton<IDiscordClient>(client)
        .AddSingleton(interactionService)
        .AddSingleton<IModuleHandler, DiscordModuleHandler>()
        .AddSingleton<IRateLimitInfoStore, MemoryRateLimitInfoStore>();


    })
    .Build();

host.Run();

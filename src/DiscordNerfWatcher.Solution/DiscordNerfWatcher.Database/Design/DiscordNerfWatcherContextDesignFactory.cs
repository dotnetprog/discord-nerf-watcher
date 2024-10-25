using DiscordNerfWatcher.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DiscordNerfWatcher.Database.Design
{
    internal class DiscordNerfWatcherContextDesignFactory : IDesignTimeDbContextFactory<DiscordNerfWatcherContext>
    {
        private readonly IConfiguration _configuration;
        private const string ASPCORE_ENV_NAME = "ASPNETCORE_ENVIRONMENT";
        public DiscordNerfWatcherContextDesignFactory(IConfiguration configuration)
        { this._configuration = configuration; }
        public DiscordNerfWatcherContextDesignFactory()
        {
            // Get environment
            // var environment = Environment.GetEnvironmentVariable(ASPCORE_ENV_NAME);

            //this._configuration = new ConfigurationBuilder()
            //    .AddJsonFile($"appsettings.json")
            //    .AddJsonFile($"appsettings.{environment}.json")
            //    .Build();
        }
        public DiscordNerfWatcherContext CreateDbContext(string[] args)
        {
            Console.WriteLine("Inputs:");
            Console.WriteLine(string.Join("|", args));
            var environment = Environment.GetEnvironmentVariable(ASPCORE_ENV_NAME);
            var indexEnv = Array.IndexOf(args, "--environment");
            if (indexEnv > -1)
            {
                environment = args[indexEnv + 1];
            }


            return Create(
                Directory.GetCurrentDirectory(),
                environment,
                "DefaultConnection");

        }
        private DiscordNerfWatcherContext Create(string basePath, string environmentName, string connectionStringName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connstr = config.GetConnectionString(connectionStringName);
            Console.WriteLine($"Environment: {environmentName}");

            if (string.IsNullOrWhiteSpace(connstr))
            {
                throw new InvalidOperationException(
                    $"Could not find a connection string named '{connectionStringName}'.");
            }
            else
            {
                var optionsBuilder = new DbContextOptionsBuilder<DiscordNerfWatcherContext>();
                optionsBuilder.UseSqlServer(connstr);
                return new DiscordNerfWatcherContext(optionsBuilder.Options);
            }
        }
    }
}

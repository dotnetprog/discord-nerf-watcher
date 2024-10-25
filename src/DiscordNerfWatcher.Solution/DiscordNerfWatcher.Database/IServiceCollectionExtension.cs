using DiscordNerfWatcher.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordNerfWatcher.Database
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection ConfigureDNWDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextFactory<DiscordNerfWatcherContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                (so) => so.EnableRetryOnFailure(3, TimeSpan.FromSeconds(2), null)));

            return services;
        }
    }
}

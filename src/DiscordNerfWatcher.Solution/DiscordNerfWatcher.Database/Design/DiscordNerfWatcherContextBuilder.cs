using DiscordNerfWatcher.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DiscordNerfWatcher.Database.Design
{
    public class DiscordNerfWatcherContextBuilder : DbContextBuilder<DiscordNerfWatcherContext>
    {
        public DiscordNerfWatcherContextBuilder(string? connectionString) : base(connectionString)
        {
        }

        public override DiscordNerfWatcherContext BuildContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DiscordNerfWatcherContext>();
            optionsBuilder.UseSqlServer(this.ConnectionString);

            return new DiscordNerfWatcherContext(optionsBuilder.Options);
        }
    }
}

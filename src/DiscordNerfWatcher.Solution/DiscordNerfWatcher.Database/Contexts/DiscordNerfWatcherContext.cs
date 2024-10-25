using Microsoft.EntityFrameworkCore;

namespace DiscordNerfWatcher.Database.Contexts
{
    public class DiscordNerfWatcherContext : DbContext
    {
        public DiscordNerfWatcherContext(DbContextOptions options) : base(options) { }

        public DbSet<ChannelUserBlock> ChannelUserBlocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChannelUserBlock>()
                .HasKey(c => new { c.ChannelId, c.UserId });
        }



    }
}

using Microsoft.EntityFrameworkCore;

namespace DiscordNerfWatcher.Database.Design
{
    public abstract class DbContextBuilder<T> where T : DbContext
    {

        protected readonly string? ConnectionString;
        public DbContextBuilder(string? connectionString)
        {
            ConnectionString = connectionString;
        }

        public abstract T BuildContext();



    }
}

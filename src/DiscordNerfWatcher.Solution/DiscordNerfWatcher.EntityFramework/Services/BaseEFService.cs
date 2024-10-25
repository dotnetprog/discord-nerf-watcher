using Microsoft.EntityFrameworkCore;

namespace DiscordNerfWatcher.EntityFramework.Services
{

    public abstract class BaseEFService<TDbContext> where TDbContext : DbContext
    {
        private readonly IDbContextFactory<TDbContext> _factory;
        protected BaseEFService(IDbContextFactory<TDbContext> factory)
        {
            _factory = factory;

        }



        protected TDbContext CreateDbContext() => _factory.CreateDbContext();



    }
}

using DiscordNerfWatcher.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DiscordNerfWatcher.EntityFramework.Services
{
    public class EFChannelUserBlockRepository : BaseEFService<DiscordNerfWatcherContext>, IChannelUserBlockRepository
    {
        public EFChannelUserBlockRepository(IDbContextFactory<DiscordNerfWatcherContext> factory) : base(factory)
        {
        }

        public async Task BlockUserFromChannel(ChannelUserBlock channelUserBlock)
        {
            using (var dbContext = this.CreateDbContext())
            {
                dbContext.ChannelUserBlocks.Add(channelUserBlock);

                await dbContext.SaveChangesAsync();

            }



        }

        public async Task<IReadOnlyCollection<ChannelUserBlock>> GetAllBlockedUserFromChannel(ulong channelId)
        {
            using (var dbContext = this.CreateDbContext())
            {
                return await dbContext.ChannelUserBlocks.Where(cub => cub.ChannelId == channelId).ToListAsync();
            }
        }

        public async Task UnblockUserFromChannel(ulong channelId, ulong userId)
        {
            using (var dbContext = this.CreateDbContext())
            {
                var userBlocked = await dbContext.ChannelUserBlocks.FirstOrDefaultAsync(d => d.UserId == userId && d.ChannelId == channelId);

                if (userBlocked != null)
                {
                    dbContext.ChannelUserBlocks.Remove(userBlocked);
                    await dbContext.SaveChangesAsync();
                }

            }
        }

        
    }
}

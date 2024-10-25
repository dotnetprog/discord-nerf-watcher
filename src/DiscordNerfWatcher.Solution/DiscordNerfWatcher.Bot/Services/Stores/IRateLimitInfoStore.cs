using Discord;

namespace DiscordNerfWatcher.Bot.Services.Stores
{
    public interface IRateLimitInfoStore
    {

        Task<IRateLimitInfo?> GetRateLimitInfoAsync(ulong channelid);

        Task AddRateLimitInfoAsync(ulong channelid, IRateLimitInfo ratelimit);



    }
}

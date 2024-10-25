using Discord;
using Microsoft.Extensions.Caching.Memory;

namespace DiscordNerfWatcher.Bot.Services.Stores
{
    public class MemoryRateLimitInfoStore : IRateLimitInfoStore
    {

        private readonly IMemoryCache _memoryCache;
        const string ByChannelKeyPrefix = "ChannelRate";
        //private static readonly object _lock = new object();
        public MemoryRateLimitInfoStore(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }



        public Task AddRateLimitInfoAsync(ulong channelid, IRateLimitInfo ratelimit)
        {

            var key = $"{ByChannelKeyPrefix}.{channelid}";
            var Cachedratelimit = _memoryCache.Get<IRateLimitInfo>(key);
            if (Cachedratelimit == null || ratelimit.Reset > Cachedratelimit.Reset)
                _memoryCache.Set(key, ratelimit);
            return Task.CompletedTask;
        }

        public Task<IRateLimitInfo?> GetRateLimitInfoAsync(ulong channelid)
        {

            var key = $"{ByChannelKeyPrefix}.{channelid}";
            var ratelimit = _memoryCache.Get<IRateLimitInfo>(key);
            return Task.FromResult(ratelimit);


        }
    }
}

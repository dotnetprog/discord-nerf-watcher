namespace DiscordNerfWatcher.Application.Requests.Queries
{
    public interface ICacheableQuery
    {
        public string CacheKey { get; }
        public bool BypassCache { get; }
    }
}

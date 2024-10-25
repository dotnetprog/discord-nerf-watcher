using MediatR;

namespace DiscordNerfWatcher.Application.Requests.Queries
{
    public class GetBlockedUsersFromChannelQuery : IRequest<IReadOnlyCollection<ChannelUserBlock>>, ICacheableQuery
    {
        public ulong ChannelId { get; set; }

        public string CacheKey => $"{nameof(GetBlockedUsersFromChannelQuery)}.{ChannelId}";

        public bool BypassCache { get; set; }
    }
}

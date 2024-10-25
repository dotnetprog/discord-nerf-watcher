

using DiscordNerfWatcher.Application.Requests.Queries;
using MediatR;

namespace DiscordNerfWatcher.Application.Requests.Commands
{
    public class UnblockUserFromChannelCommand : IRequest<Unit>, ICacheInvalidatorCommand
    {
        public ulong ChannelId { get; set; }
        public ulong UserId { get; set; }

        public string[] CacheKeys => new string[] {
            $"{nameof(GetBlockedUsersFromChannelQuery)}.{ChannelId}"
        };
    }
}

using DiscordNerfWatcher.Application.Requests.Queries;
using MediatR;


namespace DiscordNerfWatcher.Application.Requests.Commands
{
    public class BlockUserFromChannelCommand : IRequest<Unit>, ICacheInvalidatorCommand
    {
        public ChannelUserBlock? ChannelUserBlock { get; set; }

        public string[] CacheKeys => new string[] {
            $"{nameof(GetBlockedUsersFromChannelQuery)}.{ChannelUserBlock.ChannelId}"
        };
    }
}

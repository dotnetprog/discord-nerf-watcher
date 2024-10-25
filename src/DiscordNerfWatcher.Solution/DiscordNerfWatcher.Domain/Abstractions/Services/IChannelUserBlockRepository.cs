using System;



public interface IChannelUserBlockRepository
{
    Task BlockUserFromChannel(ChannelUserBlock channelUserBlock);
    Task UnblockUserFromChannel(ulong channelId, ulong userId);
    Task<IReadOnlyCollection<ChannelUserBlock>>GetAllBlockedUserFromChannel(ulong channelId);
}

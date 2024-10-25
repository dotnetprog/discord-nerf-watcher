using MediatR;

namespace DiscordNerfWatcher.Application.Requests.Queries;

public class GetBlockedUsersFromChannelQueryHandler : IRequestHandler<GetBlockedUsersFromChannelQuery, IReadOnlyCollection<ChannelUserBlock>>
{
    private readonly IChannelUserBlockRepository _channelUserBlockRepository;
    public GetBlockedUsersFromChannelQueryHandler(IChannelUserBlockRepository channelUserBlockRepository)
    {
        _channelUserBlockRepository = channelUserBlockRepository;
    }
    public async Task<IReadOnlyCollection<ChannelUserBlock>> Handle(GetBlockedUsersFromChannelQuery request, CancellationToken cancellationToken)
        => await _channelUserBlockRepository.GetAllBlockedUserFromChannel(request.ChannelId);

}

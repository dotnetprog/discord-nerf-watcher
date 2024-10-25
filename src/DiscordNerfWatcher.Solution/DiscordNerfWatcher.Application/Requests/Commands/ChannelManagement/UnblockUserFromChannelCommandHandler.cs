using MediatR;


namespace DiscordNerfWatcher.Application.Requests.Commands
{
    public class UnblockUserFromChannelCommandHandler : IRequestHandler<UnblockUserFromChannelCommand, Unit>
    {
        private readonly IChannelUserBlockRepository _channelUserBlockRepository;

        public UnblockUserFromChannelCommandHandler(IChannelUserBlockRepository channelUserRepository)
        {
            this._channelUserBlockRepository = channelUserRepository;
        }
        public async Task<Unit> Handle(UnblockUserFromChannelCommand request, CancellationToken cancellationToken)
        {
            var elements = await _channelUserBlockRepository.GetAllBlockedUserFromChannel(request.ChannelId);

            if (!elements.Any(e => e.UserId == request.UserId))
            {
                return Unit.Value;
            }
            await _channelUserBlockRepository.UnblockUserFromChannel(request.ChannelId, request.UserId);

            return Unit.Value;
        }

    }
}

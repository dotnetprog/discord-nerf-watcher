using MediatR;

namespace DiscordNerfWatcher.Application.Requests.Commands
{
    public class BlockUserFromChannelCommandHandler : IRequestHandler<BlockUserFromChannelCommand, Unit>
    {

        private readonly IChannelUserBlockRepository _channelUserBlockRepository;

        public BlockUserFromChannelCommandHandler(IChannelUserBlockRepository channelUserRepository)
        {
            this._channelUserBlockRepository = channelUserRepository;
        }

        public async Task<Unit> Handle(BlockUserFromChannelCommand request, CancellationToken cancellationToken)
        {
            var elements = await _channelUserBlockRepository.GetAllBlockedUserFromChannel(request.ChannelUserBlock.ChannelId);
            if (elements.Any(e => e.UserId == request.ChannelUserBlock.UserId))
                return Unit.Value;
            await this._channelUserBlockRepository.BlockUserFromChannel(request.ChannelUserBlock);
            return Unit.Value;
        }




    }
}

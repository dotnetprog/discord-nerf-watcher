using DiscordNerfWatcher.Application.Events;
using DiscordNerfWatcher.Application.Requests.Queries;
using DiscordNerfWatcher.Bot.Requests.Commands;
using MediatR;

namespace DiscordNerfWatcher.Bot.Subscribers
{
    public class DeleteMessageSubscriber : INotificationHandler<OnMessageReceiveEvent>
    {
        private readonly ISender _sender;

        public DeleteMessageSubscriber(ISender sender)
        {
            _sender = sender;
        }
        public async Task Handle(OnMessageReceiveEvent notification, CancellationToken cancellationToken)
        {
            var query = new GetBlockedUsersFromChannelQuery { ChannelId = notification.ChannelId };
            var users = await _sender.Send(query, cancellationToken);

            if (!users.Any(u => u.UserId == notification.MessageAuthorId))
            {
                return;
            }


            var deleteCommand = new DeleteMessageCommand()
            {
                channelid = notification.ChannelId,
                messageid = notification.MessageId
            };
            await _sender.Send(deleteCommand, cancellationToken);

        }
    }
}

using MediatR;

namespace DiscordNerfWatcher.Application.Events
{
    public class OnMessageReceiveEvent : INotification
    {

        public ulong MessageId { get; set; }
        public ulong ChannelId { get; set; }
        public string MessageContent { get; set; }
        public ulong MessageAuthorId { get; set; }


    }
}

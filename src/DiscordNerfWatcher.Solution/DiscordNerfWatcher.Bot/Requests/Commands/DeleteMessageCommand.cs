using MediatR;

namespace DiscordNerfWatcher.Bot.Requests.Commands
{
    public class DeleteMessageCommand : IRequest<Unit>
    {

        public ulong channelid { get; set; }
        public ulong messageid { get; set; }

    }
}

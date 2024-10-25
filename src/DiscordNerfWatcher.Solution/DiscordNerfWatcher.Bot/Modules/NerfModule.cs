using Discord;
using Discord.Interactions;
using DiscordNerfWatcher.Application.Requests.Commands;
using DiscordNerfWatcher.Application.Requests.Queries;
using MediatR;

namespace DiscordNerfWatcher.Bot.Modules
{
    public class NerfModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly ISender _sender;
        public NerfModule(ISender sender)
        {
            this._sender = sender;
        }
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        [SlashCommand("nerf", "Nerf user from channel")]
        public async Task NerfCommand(IGuildUser user, ITextChannel channel)
        {
            await RespondAsync($"User :{user.GlobalName} is being nerfed from this channel:{channel.Name}", ephemeral: true);
            var command = new BlockUserFromChannelCommand()
            {
                ChannelUserBlock = new ChannelUserBlock
                {
                    UserId = user.Id,
                    ChannelId = channel.Id,
                    GuildId = channel.GuildId
                }
            };


            await this._sender.Send(command, CancellationToken.None);



            await FollowupAsync($"User :{user.GlobalName} has been nerfed from this channel:{channel.Name}", ephemeral: true);
        }
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        [SlashCommand("nerf-list", "Get all user nerfed from a channel")]
        public async Task GetAllNerfUserFromChannel(ITextChannel channel)
        {

            var query = new GetBlockedUsersFromChannelQuery
            {
                ChannelId = channel.Id
            };

            var users = await this._sender.Send(query, CancellationToken.None);

            var embed = new EmbedBuilder();

            embed.WithAuthor(Context.Client.CurrentUser)
                    .WithCurrentTimestamp();

            if (users.Count > 0)
            {
                embed.WithTitle($"Users Nerfed from {channel.Name}").
                     WithDescription(string.Join("\n", users.Select(u => $"<@{u.UserId}>")));
            }
            else
                embed.WithTitle($"No user is nerfed from this channel {channel.Name}");

            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        [SlashCommand("removenerf", "Remove a nerf user from channel")]
        public async Task RemoveNerfCommand(IGuildUser user, ITextChannel channel)
        {
            await RespondAsync($"User :{user.GlobalName} is being unnerfed from this channel:{channel.Name}", ephemeral: true);
            var command = new UnblockUserFromChannelCommand()
            {
                UserId = user.Id,
                ChannelId = channel.Id

            };
            await this._sender.Send(command, CancellationToken.None);

            await FollowupAsync($"User :{user.GlobalName} has been unnerfed from this channel:{channel.Name}", ephemeral: true);

        }
    }

}

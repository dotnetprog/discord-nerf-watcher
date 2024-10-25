using DiscordNerfWatcher.Domain.Entities;

public class ChannelUserBlock : Entity
{
    public ulong GuildId
    { get; set; }

    public ulong ChannelId
    { get; set; }

    public ulong UserId
    { get; set; }


}

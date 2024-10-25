namespace DiscordNerfWatcher.Application.Requests.Commands
{
    public interface ICacheInvalidatorCommand
    {

        public string[] CacheKeys { get; }

    }
}

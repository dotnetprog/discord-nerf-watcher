using Discord;
using DiscordNerfWatcher.Bot.Services.Stores;
using MediatR;

namespace DiscordNerfWatcher.Bot.Requests.Commands
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, Unit>
    {
        private readonly IDiscordClient _discordClient;
        private readonly IRateLimitInfoStore _ratelimitstore;
        private readonly ILogger<DeleteMessageCommandHandler> _logger;
        private ulong channelid { get; set; }
        public DeleteMessageCommandHandler(IDiscordClient discordClient, IRateLimitInfoStore ratelimitstore, ILogger<DeleteMessageCommandHandler> logger)
        {
            _discordClient = discordClient;
            _ratelimitstore = ratelimitstore;
            _logger = logger;
        }
        public async Task<Unit> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {

            var channel = await _discordClient.GetChannelAsync(request.channelid, CacheMode.AllowDownload);

            if (channel is not ITextChannel)
            {
                return Unit.Value;
            }
            channelid = channel.Id;
            var textChannel = channel as ITextChannel;




            var currentRateLimit = await _ratelimitstore.GetRateLimitInfoAsync(channelid);
            if (currentRateLimit is not null &&
                currentRateLimit.Remaining.HasValue &&
                currentRateLimit.Remaining < 5 &&
                currentRateLimit.RetryAfter.HasValue)
            {
                _logger.LogInformation($"Need to wait {currentRateLimit.RetryAfter.Value} seconds, only {currentRateLimit.Remaining ?? 0} remaining");
                await Task.Delay(currentRateLimit.RetryAfter.Value * 1300);
            }
            var options = new RequestOptions();
            options.RatelimitCallback = RateLimitCallBack;
            await textChannel.DeleteMessageAsync(request.messageid, options);



            return Unit.Value;
        }
        private async Task RateLimitCallBack(IRateLimitInfo ratelimitinfo)
        {
            _logger.LogDebug(@$"Adding ratelimit in store: Endpoint: {ratelimitinfo.Endpoint},
                                Limit: {ratelimitinfo.Limit},
                                Bucket: {ratelimitinfo.Bucket},
                                RetryAfter: {ratelimitinfo.RetryAfter},
                                IsGlobal: {ratelimitinfo.IsGlobal},
                                Lag: {ratelimitinfo.Lag},
                                Remaining: {ratelimitinfo.Remaining},
                                Reset: {ratelimitinfo.Reset},
                                ResetAfter: {ratelimitinfo.ResetAfter}".Replace(Environment.NewLine, string.Empty));
            await _ratelimitstore.AddRateLimitInfoAsync(channelid, ratelimitinfo);
        }
    }
}

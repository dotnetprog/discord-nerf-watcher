using DiscordNerfWatcher.Application.Requests.Commands;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DiscordNerfWatcher.Application.Behaviors
{
    public class CacheInvalidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheInvalidatorCommand
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger _logger;

        public CacheInvalidatorBehavior(IMemoryCache cache, ILogger<TResponse> logger)
        {
            _cache = cache;
            _logger = logger;

        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {

            foreach (var cacheKeys in request.CacheKeys)
            {
                _logger.LogInformation("Removing cache: " + cacheKeys);
                _cache.Remove(cacheKeys);
            }

            return await next();
        }

    }
}

using DiscordNerfWatcher.Application.Requests.Queries;
using DiscordNerfWatcher.Application.Settings;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace DiscordNerfWatcher.Application.Behaviors
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheableQuery
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger _logger;
        private readonly CacheSettings _settings;
        public CachingBehavior(IMemoryCache cache, ILogger<TResponse> logger, IOptions<CacheSettings> settings)
        {
            _cache = cache;
            _logger = logger;
            _settings = settings.Value;
        }



        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {

            TResponse response;
            if (request.BypassCache) return await next();
            async Task<TResponse> GetResponseAndAddToCache()
            {
                response = await next();
                var slidingExpiration = TimeSpan.FromHours(_settings.SlidingExpiration);
                var options = new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration };
                var serializedData = JsonSerializer.Serialize(response);
                _cache.Set(request.CacheKey, serializedData, options);
                return response;
            };
            var cachedResponse = _cache.Get<string>(request.CacheKey);
            if (cachedResponse != null)
            {
                response = JsonSerializer.Deserialize<TResponse>(cachedResponse);
                _logger.LogInformation($"Fetched from Cache -> '{request.CacheKey}'.");
            }
            else
            {
                response = await GetResponseAndAddToCache();
                _logger.LogInformation($"Added to Cache -> '{request.CacheKey}'.");
            }
            return response;
        }
    }
}

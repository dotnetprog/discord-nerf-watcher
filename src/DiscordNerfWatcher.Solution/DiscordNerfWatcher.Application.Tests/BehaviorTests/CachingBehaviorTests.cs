using DiscordNerfWatcher.Application.Behaviors;
using DiscordNerfWatcher.Application.Settings;
using DiscordNerfWatcher.Application.Tests.Fake.CQRS;
using DiscordNerfWatcher.Application.Tests.Fake.Data;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Text.Json;

namespace DiscordNerfWatcher.Application.Tests.BehaviorTests
{

    public class CachingBehaviorTests
    {
        private readonly IServiceProvider _serviceProvider;
        private IMemoryCache MemoryCache => _serviceProvider.GetService<IMemoryCache>();
        private readonly ILogger<FakeClassData[]> FakeLogger;
        private readonly IOptions<CacheSettings> _cacheSettingsOptions;
        private readonly IPipelineBehavior<FakeCacheableQuery, FakeClassData[]> pipelineBehavior;
        public CachingBehaviorTests()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            _serviceProvider = services.BuildServiceProvider();
            FakeLogger = Substitute.For<ILogger<FakeClassData[]>>();
            _cacheSettingsOptions = Options.Create<CacheSettings>(new CacheSettings() { SlidingExpiration = 2 });
            this.pipelineBehavior = new CachingBehavior<FakeCacheableQuery, FakeClassData[]>(this.MemoryCache, this.FakeLogger, _cacheSettingsOptions);
        }




        [Fact]
        public async Task Given_a_CacheableQuery_behavior_should_cache_results()
        {
            //Arrange
            var queryResults = FakeDataGenerator.FakeClassData().Generate(5);

            var query = new FakeCacheableQuery
            {
                BypassCache = false
            };
            //Act

            var results = await pipelineBehavior.Handle(query, async () =>
            {

                return queryResults.ToArray();

            }, CancellationToken.None);
            //Assert

            var cachedResults = JsonSerializer.Deserialize<FakeClassData[]>(this.MemoryCache.Get<string>(query.CacheKey));

            cachedResults.Should().HaveCount(results.Length);
            cachedResults.All(cr => results.Any(qr => qr.FakeId == cr.FakeId)).Should().BeTrue();




        }
        [Fact]
        public async Task Given_a_CacheableQuery_when_it_is_call_many_time_behavior_should_usecache_results()
        {
            //Arrange
            var queryResults = FakeDataGenerator.FakeClassData().Generate(5);
            var queryResults2 = FakeDataGenerator.FakeClassData().Generate(5);
            var query = new FakeCacheableQuery
            {
                BypassCache = false
            };
            //Act

            var results = await pipelineBehavior.Handle(query, async () =>
            {

                return queryResults.ToArray();

            }, CancellationToken.None);
            results = await pipelineBehavior.Handle(query, async () =>
            {

                return queryResults2.ToArray();

            }, CancellationToken.None);
            //Assert

            var cachedResults = JsonSerializer.Deserialize<FakeClassData[]>(this.MemoryCache.Get<string>(query.CacheKey));

            cachedResults.Should().HaveCount(results.Length);
            cachedResults.All(cr => results.Any(qr => qr.FakeId == cr.FakeId)).Should().BeTrue();




        }
        [Fact]
        public async Task Given_a_CacheableQuery_when_it_cache_is_bypassed_behavior_should_notcache_results()
        {
            //Arrange
            var queryResults = FakeDataGenerator.FakeClassData().Generate(5);

            var query = new FakeCacheableQuery
            {
                BypassCache = true
            };
            //Act

            var results = await pipelineBehavior.Handle(query, async () =>
            {

                return queryResults.ToArray();

            }, CancellationToken.None);

            //Assert

            var cachedResultsRaw = this.MemoryCache.Get<string>(query.CacheKey);
            cachedResultsRaw.Should().BeNull();






        }

    }
}

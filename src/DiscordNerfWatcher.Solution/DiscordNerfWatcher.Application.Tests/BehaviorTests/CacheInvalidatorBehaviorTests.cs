using DiscordNerfWatcher.Application.Behaviors;
using DiscordNerfWatcher.Application.Tests.Fake.CQRS;
using DiscordNerfWatcher.Application.Tests.Fake.Data;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace DiscordNerfWatcher.Application.Tests.BehaviorTests
{
    public class CacheInvalidatorBehaviorTests
    {
        private readonly IServiceProvider _serviceProvider;
        private IMemoryCache MemoryCache => _serviceProvider.GetService<IMemoryCache>();
        private readonly ILogger<Unit> FakeLogger;
        private readonly IPipelineBehavior<FakeRequestCommand, Unit> _pipelineBehavior;
        private const string CacheKey = "FakeCacheKey";
        public CacheInvalidatorBehaviorTests()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            _serviceProvider = services.BuildServiceProvider();
            FakeLogger = Substitute.For<ILogger<Unit>>();

            this._pipelineBehavior = new CacheInvalidatorBehavior<FakeRequestCommand, Unit>(this.MemoryCache, this.FakeLogger);
        }


        [Fact]
        public async Task Given_a_CacheInvalidatorCommand_when_it_is_executed_should_invalidate_the_current_cache()
        {
            //Arrange
            var fakeData = FakeDataGenerator.FakeClassData().Generate(5);
            this.MemoryCache.Set(CacheKey, fakeData);
            var command = new FakeRequestCommand
            {
                FakeStringProperty = "abdc"
            };

            //Act

            await _pipelineBehavior.Handle(command, async () =>
            {
                return Unit.Value;
            }, CancellationToken.None);

            //Assert

            this.MemoryCache.Get(CacheKey).Should().BeNull();

        }


    }
}

using DiscordNerfWatcher.Application.Requests.Commands;
using MediatR;

namespace DiscordNerfWatcher.Application.Tests.Fake.CQRS
{
    public class FakeRequestCommand : IRequest<Unit>, ICacheInvalidatorCommand
    {
        public string FakeStringProperty { get; set; }

        public string[] CacheKeys => new string[]
        {
            "FakeCacheKey"
        };
    }
}

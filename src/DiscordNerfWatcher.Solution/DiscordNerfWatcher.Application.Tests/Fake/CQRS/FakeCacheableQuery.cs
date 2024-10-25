using DiscordNerfWatcher.Application.Requests.Queries;
using DiscordNerfWatcher.Application.Tests.Fake.Data;
using MediatR;

namespace DiscordNerfWatcher.Application.Tests.Fake.CQRS
{
    public class FakeCacheableQuery : IRequest<FakeClassData[]>, ICacheableQuery
    {
        public string CacheKey => "FakeCacheKey";

        public bool BypassCache { get; set; }
    }
}

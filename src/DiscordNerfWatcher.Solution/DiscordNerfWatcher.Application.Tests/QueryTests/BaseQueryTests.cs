using DiscordNerfWatcher.Application.Tests.Fake.Service;
using MediatR;

namespace DiscordNerfWatcher.Application.Tests.QueryTests
{
    public abstract class BaseQueryTests<TQueryHandler, TQuery, TResult>
        where TQueryHandler : IRequestHandler<TQuery, TResult>
        where TQuery : IRequest<TResult>
    {
        protected readonly FakeChannelUserBlockRepository _channelUserBlockRepository;
        protected BaseQueryTests()
        {
            _channelUserBlockRepository = new FakeChannelUserBlockRepository();
        }

        public void Dispose()
        {
            _channelUserBlockRepository.Data.Clear();

        }

        public abstract TQueryHandler BuildQueryHandler();

        protected async Task<TResult> RunAsync(TQuery Query)
        {
            var queryHandler = BuildQueryHandler();


            //Act
            return await queryHandler.Handle(Query, CancellationToken.None);
        }

    }
}

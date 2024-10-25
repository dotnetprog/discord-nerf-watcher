using DiscordNerfWatcher.Application.Tests.Fake.Service;
using MediatR;

namespace DiscordNerfWatcher.Application.Tests.CommandTests
{
    public abstract class BaseCommandTest<TCommandHandler, TCommand>
        where TCommandHandler : IRequestHandler<TCommand, Unit>
        where TCommand : IRequest<Unit>
    {
        protected readonly FakeChannelUserBlockRepository _repository;
        protected BaseCommandTest()
        {
            this._repository = new FakeChannelUserBlockRepository();
        }
        public void Dispose()
        {
            _repository.Data.Clear();
        }
        public abstract TCommandHandler BuildCommandHandler();

        protected async Task RunAsync(TCommand command)
        {
            var cmd = BuildCommandHandler();


            //Act
            await cmd.Handle(command, CancellationToken.None);
        }


    }
}

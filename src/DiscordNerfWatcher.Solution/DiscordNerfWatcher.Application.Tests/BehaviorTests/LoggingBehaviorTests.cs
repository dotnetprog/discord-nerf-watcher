using DiscordNerfWatcher.Application.Behaviors;
using DiscordNerfWatcher.Application.Tests.Fake.CQRS;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace DiscordNerfWatcher.Application.Tests.BehaviorTests
{
    public class LoggingBehaviorTests
    {

        [Fact]
        public async Task behavior_should_log_request_execution()
        {
            //Arrange
            var logMock = Substitute.For<ILogger<LoggingBehavior<FakeRequestCommand, Unit>>>();
            var request = new FakeRequestCommand()
            {
                FakeStringProperty = "test data"
            };
            var fakeDelegate = new RequestHandlerDelegate<Unit>(async () => await Task.FromResult(Unit.Value));
            var behavior = new LoggingBehavior<FakeRequestCommand, Unit>(logMock);

            //Act
            var response = await behavior.Handle(request, fakeDelegate, CancellationToken.None);

            //Assert

            logMock.Received().Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("[START]") && o.ToString().Contains(nameof(FakeRequestCommand))),
                null,
                Arg.Any<Func<object, Exception, string>>());
            logMock.Received().Log(
               LogLevel.Information,
               Arg.Any<EventId>(),
               Arg.Is<object>(o => o.ToString().Contains("[END]")
                                && o.ToString().Contains(nameof(FakeRequestCommand))
                                && o.ToString().Contains("Execution time")),
               null,
               Arg.Any<Func<object, Exception, string>>());

        }
        [Fact]
        public async Task behavior_should_log_request_payload()
        {
            //Arrange
            var logMock = Substitute.For<ILogger<LoggingBehavior<FakeRequestCommand, Unit>>>();
            var request = new FakeRequestCommand()
            {
                FakeStringProperty = "test data"
            };
            var fakeDelegate = new RequestHandlerDelegate<Unit>(async () => await Task.FromResult(Unit.Value));
            var behavior = new LoggingBehavior<FakeRequestCommand, Unit>(logMock);

            //Act
            var response = await behavior.Handle(request, fakeDelegate, CancellationToken.None);

            //Assert

            logMock.Received().Log(
                LogLevel.Debug,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("[PROPS]") && o.ToString().Contains("test data")),
                null,
                Arg.Any<Func<object, Exception, string>>());


        }
        [Fact]
        public async Task behavior_should_log_payloadSerialization_error()
        {
            //Arrange
            var logMock = Substitute.For<ILogger<LoggingBehavior<FakeUnSerializableRequestCommand, Unit>>>();
            var request = new FakeUnSerializableRequestCommand()
            {
                UnSerializableProperty = IntPtr.Zero
            };
            var fakeDelegate = new RequestHandlerDelegate<Unit>(async () => await Task.FromResult(Unit.Value));
            var behavior = new LoggingBehavior<FakeUnSerializableRequestCommand, Unit>(logMock);

            //Act
            var response = await behavior.Handle(request, fakeDelegate, CancellationToken.None);

            //Assert

            logMock.Received().Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("[Serialization ERROR]")),
                null,
                Arg.Any<Func<object, Exception, string>>());


        }

    }
}

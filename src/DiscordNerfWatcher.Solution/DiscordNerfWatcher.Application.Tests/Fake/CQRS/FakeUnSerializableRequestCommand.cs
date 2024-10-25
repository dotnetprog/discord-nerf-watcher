using MediatR;

namespace DiscordNerfWatcher.Application.Tests.Fake.CQRS
{
    public class FakeUnSerializableRequestCommand : IRequest<Unit>
    {
        public IntPtr UnSerializableProperty { get; set; }

    }
}

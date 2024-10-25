using DiscordNerfWatcher.Application.Requests.Commands;
using FluentAssertions;

namespace DiscordNerfWatcher.Application.Tests.CommandTests
{
    public class UnblockUserFromChannelCommandTests : BaseCommandTest<UnblockUserFromChannelCommandHandler, UnblockUserFromChannelCommand>
    {
        public override UnblockUserFromChannelCommandHandler BuildCommandHandler() => new UnblockUserFromChannelCommandHandler(this._repository);

        [Fact]
        public async Task WhenCommandIsExecutedRepositoryShouldUnblockUserTest()
        {
            //Arrange
            var cub = new ChannelUserBlock()
            {
                ChannelId = 1,
                GuildId = 1,
                UserId = 2
            };
            this._repository.Data.Add((ChannelUserBlock)cub.Clone());

            var request = new UnblockUserFromChannelCommand()
            {
                ChannelId = cub.ChannelId,
                UserId = cub.UserId
            };

            //Act
            await RunAsync(request);

            //assert
            this._repository.Data.Should().NotContain(d => d.ChannelId == cub.ChannelId
                                                    && d.UserId == cub.UserId);
        }
        [Fact]
        public async Task WhenCommandIsExecutedRepositoryHaveMoreThanOneUserBlocked()
        {

            //Arrange
            var cub1 = new ChannelUserBlock()
            {
                ChannelId = 1,
                GuildId = 1,
                UserId = 2
            };

            var cub2 = new ChannelUserBlock()
            {
                ChannelId = 1,
                GuildId = 1,
                UserId = 3
            };
            this._repository.Data.Add((ChannelUserBlock)cub1.Clone());
            this._repository.Data.Add((ChannelUserBlock)cub2.Clone());

            var request = new UnblockUserFromChannelCommand()
            {
                ChannelId = cub1.ChannelId,
                UserId = cub1.UserId
            };
            //Act
            await RunAsync(request);

            //Assert
            this._repository.Data.Should().ContainSingle();

        }

        [Fact]
        public async Task WhenCommandIsExecutedRepositoryShouldNotRemoveUserNotBlocked()
        {

            //Arrange
            var cub = new ChannelUserBlock()
            {
                ChannelId = 1,
                GuildId = 1,
                UserId = 2
            };

            this._repository.Data.Add(cub);

            var request = new UnblockUserFromChannelCommand()
            {
                ChannelId = 2,
                UserId = 3
            };
            await RunAsync(request);

            this._repository.Data.Should().Contain(d => d.ChannelId == cub.ChannelId
                                                    && d.UserId == cub.UserId
                                                    && d.GuildId == cub.GuildId);

        }

    }
}

using DiscordNerfWatcher.Application.Requests.Commands;
using FluentAssertions;

namespace DiscordNerfWatcher.Application.Tests.CommandTests
{
    public class BlockUserFromChannelCommandTests : BaseCommandTest<BlockUserFromChannelCommandHandler, BlockUserFromChannelCommand>
    {

        public override BlockUserFromChannelCommandHandler BuildCommandHandler() => new BlockUserFromChannelCommandHandler(this._repository);

        [Fact]
        public async Task WhenCommandIsExecutedRepositoryShouldBlockUserTest()
        {

            //Arrange

            var cub = new ChannelUserBlock()
            {
                ChannelId = 1,
                GuildId = 1,
                UserId = 2
            };


            var request = new BlockUserFromChannelCommand()
            {
                ChannelUserBlock = cub
            };

            //Act
            await this.RunAsync(request);



            //Assert
            this._repository.Data.FirstOrDefault(d => d.ChannelId == cub.ChannelId
                                                    && d.UserId == cub.UserId
                                                    && d.GuildId == cub.GuildId).Should().NotBeNull();

        }
        [Fact]
        public async Task WhenCommandIsExecutedRepositoryShouldNotDuplicateUser()
        {

            //Arrange

            var cub = new ChannelUserBlock()
            {
                ChannelId = 1,
                GuildId = 1,
                UserId = 2
            };
            this._repository.Data.Add((ChannelUserBlock)cub.Clone());

            var request = new BlockUserFromChannelCommand()
            {
                ChannelUserBlock = cub
            };

            //Act
            await this.RunAsync(request);


            //Assert
            this._repository.Data.Where(d => d.ChannelId == cub.ChannelId
                                                    && d.UserId == cub.UserId
                                                    && d.GuildId == cub.GuildId).Should().HaveCount(1);

        }

    }
}

using DiscordNerfWatcher.Application.Requests.Queries;
using FluentAssertions;

namespace DiscordNerfWatcher.Application.Tests.QueryTests
{
    public class GetBlockedUsersFromChannelQueryTests
        : BaseQueryTests<GetBlockedUsersFromChannelQueryHandler, GetBlockedUsersFromChannelQuery, IReadOnlyCollection<ChannelUserBlock>>
    {
        public override GetBlockedUsersFromChannelQueryHandler BuildQueryHandler() => new GetBlockedUsersFromChannelQueryHandler(this._channelUserBlockRepository);

        [Theory]
        [InlineData(1, 10)]
        [InlineData(2, 1)]
        public async void query_should_return_allblockedusers_from_channelid(ulong channelid, int countexpected)
        {
            //Arrange
            var cubs = new List<ChannelUserBlock>()
            {
                new ChannelUserBlock
                {
                    ChannelId = 2,
                    UserId = 13,
                    GuildId = 1
                }
            };

            int limit = 10;
            cubs.AddRange(GetData(1, 1, limit));

            this._channelUserBlockRepository.Data.AddRange(cubs);
            var request = new GetBlockedUsersFromChannelQuery()
            {
                ChannelId = channelid
            };

            //Act
            var results = await RunAsync(request);

            //Assert
            results.Select(r => r.ChannelId).Should().AllBeEquivalentTo(channelid).And.HaveCount(countexpected);

        }

        [Fact]
        public async void query_should_return_None()
        {
            //Arrange
            var cubs = new List<ChannelUserBlock>()
            {
                new ChannelUserBlock
                {
                    ChannelId = 2,
                    UserId = 13,
                    GuildId = 1
                }
            };

            int limit = 10;

            cubs.AddRange(GetData(1, 1, limit));
            this._channelUserBlockRepository.Data.AddRange(cubs);
            var request = new GetBlockedUsersFromChannelQuery()
            {
                ChannelId = 10
            };

            //Act
            var results = await RunAsync(request);

            //Assert
            results.Should().BeEmpty();

        }

        private IEnumerable<ChannelUserBlock> GetData(ulong channelid, ulong guildid, int limit)
        {
            for (var i = 0; i < limit; i++)
            {
                yield return new ChannelUserBlock()
                {
                    ChannelId = channelid,
                    GuildId = guildid,
                    UserId = (ulong)i,
                };

            };
        }

    }


}


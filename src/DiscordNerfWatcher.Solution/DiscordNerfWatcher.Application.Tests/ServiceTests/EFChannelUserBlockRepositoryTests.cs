using DiscordNerfWatcher.Database.Contexts;
using DiscordNerfWatcher.EntityFramework.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NSubstitute;

namespace DiscordNerfWatcher.Application.Tests.InMemoryTest
{
    public class EFChannelUserBlockRepositoryTests
    {
        private readonly DbContextOptions<DiscordNerfWatcherContext> _contextOptions;




        private DbContextOptions<DiscordNerfWatcherContext> GetDbContextOptions() => new DbContextOptionsBuilder<DiscordNerfWatcherContext>()
                .UseInMemoryDatabase("EFChannelUserBlockRepositoryTest")
                .ConfigureWarnings(e => e.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;


        private DiscordNerfWatcherContext BuildDbContext(DbContextOptions<DiscordNerfWatcherContext> options)
            => new DiscordNerfWatcherContext(options);


        private readonly IChannelUserBlockRepository _repository;

        private readonly DiscordNerfWatcherContext _dbcontext;
        private readonly IDbContextFactory<DiscordNerfWatcherContext> _dbFactory;
        public EFChannelUserBlockRepositoryTests()
        {

            _contextOptions = GetDbContextOptions();
            _dbcontext = BuildDbContext(_contextOptions);

            _dbcontext.Database.EnsureDeleted();
            _dbcontext.Database.EnsureCreated();




            var MockedFactory = Substitute
                .For<IDbContextFactory<DiscordNerfWatcherContext>>();
            MockedFactory.CreateDbContext().Returns(_dbcontext);

            _dbFactory = MockedFactory;



            _repository = new EFChannelUserBlockRepository(_dbFactory);
        }

        [Fact]
        public async Task Given_AChannelUserBlock_WhenItIsAdded_ShouldBeInDatabase()
        {
            //Arrange

            var cub = new ChannelUserBlock()
            {
                ChannelId = 1,
                GuildId = 1,
                UserId = 2
            };

            //Act
            await _repository.BlockUserFromChannel(cub);


            //Assert
            using (var context = new DiscordNerfWatcherContext(_contextOptions))
            {
                var user = await context.ChannelUserBlocks.FirstOrDefaultAsync(d => d.ChannelId == cub.ChannelId
                                                    && d.UserId == cub.UserId
                                                    && d.GuildId == cub.GuildId);
                user.Should().NotBeNull();


            }



        }
        [Fact]
        public async Task Given_AChannelUserBlock_WhenItIsRemoved_ShouldNotBeInDatabase()
        {

            //Arrange

            var cub = new ChannelUserBlock()
            {
                ChannelId = 1,
                GuildId = 1,
                UserId = 2
            };
            using (var context = new DiscordNerfWatcherContext(_contextOptions))
            {
                context.ChannelUserBlocks.Add(cub);
                context.SaveChanges();
            }

            //Act

            await _repository.UnblockUserFromChannel(cub.ChannelId, cub.UserId);


            //Assert
            using (var context = new DiscordNerfWatcherContext(_contextOptions))
            {
                var user = await context.ChannelUserBlocks.FirstOrDefaultAsync(d => d.ChannelId == cub.ChannelId
                                                    && d.UserId == cub.UserId);

                user.Should().BeNull();

            }
        }
        [Fact]
        public async Task Given_AChannelUserBlock_Should_Return_Allblockedusers_From_Channelid()
        {

            var cub1 = new ChannelUserBlock()
            {
                ChannelId = 1,
                GuildId = 1,
                UserId = 1
            };
            var cub2 = new ChannelUserBlock()
            {
                ChannelId = 1,
                GuildId = 1,
                UserId = 2
            };

            using (var context = new DiscordNerfWatcherContext(_contextOptions))
            {
                context.ChannelUserBlocks.AddRange(cub1, cub2);
                context.SaveChanges();

            }
            var users = await _repository.GetAllBlockedUserFromChannel(cub1.ChannelId);

            users.Should().NotBeEmpty();
            users.Should().HaveCount(2);



        }
    }
}

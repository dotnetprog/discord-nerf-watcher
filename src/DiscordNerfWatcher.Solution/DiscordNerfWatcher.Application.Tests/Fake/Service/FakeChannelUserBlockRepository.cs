namespace DiscordNerfWatcher.Application.Tests.Fake.Service
{
    public class FakeChannelUserBlockRepository : IChannelUserBlockRepository
    {
        public List<ChannelUserBlock> Data { get; set; }

        public FakeChannelUserBlockRepository(ChannelUserBlock[] channelUserBlock) : this()
        {
            Data.AddRange(channelUserBlock);

        }
        public FakeChannelUserBlockRepository()
        {
            this.Data = new List<ChannelUserBlock>();


        }
        public async Task BlockUserFromChannel(ChannelUserBlock channelUserBlock)
        {




            await Task.Run(() => Data.Add(channelUserBlock));

        }

        public async Task<IReadOnlyCollection<ChannelUserBlock>> GetAllBlockedUserFromChannel(ulong channelId)
        {
            return await Task.FromResult(this.Data.Where(cub => cub.ChannelId == channelId).ToArray());
            //Retourner la liste
        }

        public async Task UnblockUserFromChannel(ulong channelId, ulong userId)
        {

            //Trouver le channeluserblock correspondant au channelId et UserId
            // SI pas trouver, ne rien faire
            // SI trouver, on retire lelemen trouver de la listeé

            var element = this.Data
                             .Where(cub => cub.ChannelId == channelId && cub.UserId == userId).FirstOrDefault();
            if (element == null) { return; }

            await Task.Run(() => Data.Remove(element));




        }


    }
}

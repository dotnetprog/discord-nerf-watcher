namespace DiscordNerfWatcher.Domain.Entities
{
    public abstract class Entity : ICloneable
    {
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }



    }
}

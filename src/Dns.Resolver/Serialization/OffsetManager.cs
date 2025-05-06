namespace Dns.Resolver.Serialization
{
    internal class OffsetManager
    {
        public OffsetManager(int startingOFfset = 0)
        {
            Offset = startingOFfset;
        }

        public void Advance(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "count must be greater than or equal to zero.");

            Offset += count;
        }

        public int Offset { get; private set; }

        public void SeekAsolute(int offset)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), "offset must be greater than or equal to zero.");

            Offset = offset;
        }
    }
}

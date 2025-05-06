namespace Dns.Resolver.Serialization;

internal static class DoubleByteHelper
{
    public static ushort GetBit(bool value, int position)
    {
        if (position > 15)
            throw new ArgumentOutOfRangeException(nameof(position), "position must be between 0 and 15.");

        if (value)
            return (ushort)(0b0001 << (15 - position));

        return 0;
    }

    /// <summary>
    /// Take a 4 bit number and shift it into a 16 bit bitpacked number.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static ushort GetQuadBits(byte value, int position)
    { 
        if (value > 0b1111)
            throw new ArgumentOutOfRangeException(nameof(value), "value must be between 0 and 0b1111.");

        if (position > 12)
              throw new ArgumentOutOfRangeException(nameof(position), "position must be between 0 and 3.");

        return (ushort)(value << (15 -  position));
    }
}
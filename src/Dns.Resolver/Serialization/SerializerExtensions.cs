namespace Dns.Resolver.Serialization;

internal static class SerializerExtensions
{
    public static ushort ClearStringPointerMask(this ushort value)
    {
        return (ushort)(value & ~DnsBitReaderWriterBase.StringPointerMask);
    }

    public static bool IsStringPointerMask(this byte value)
    {
        return (value & DnsBitReaderWriterBase.StringPointerByteMask) == DnsBitReaderWriterBase.StringPointerByteMask;
    }

    public static bool GetBit(this ushort value, int bitIndex)
    {
        if (bitIndex < 0 || bitIndex > 15)
            throw new ArgumentOutOfRangeException(nameof(bitIndex), "bitIndex must be between 0 and 15 inclusive.");

        var bitMask = 1 << (15 - bitIndex);

        return (value & bitMask) == bitMask;
    }

    public static byte GetQuadBits(this ushort value, int bitIndex)
    {
        var bitMask = 0b0000 << (15 - bitIndex);

        return (byte)((value & bitMask) >> (15 - bitIndex));
    }
}

using BitConverter;

namespace Dns.Resolver.Serialization;

internal abstract class DnsBitReaderWriterBase
{
    protected static EndianBitConverter bitConverter = EndianBitConverter.BigEndian;

    /// <summary>
    /// If a string starts with this two bits, the value is stored as an offset to a previous position in the message.
    /// </summary>
    internal const uint StringPointerMask = 0b1100000000000000;

    internal const byte StringPointerByteMask = 0b11000000;
}

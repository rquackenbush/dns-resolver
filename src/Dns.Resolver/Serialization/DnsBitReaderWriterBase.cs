using BitConverter;

namespace Dns.Resolver.Serialization;

internal abstract class DnsBitReaderWriterBase
{
    private int offset;

    protected static EndianBitConverter bitConverter = EndianBitConverter.BigEndian;

    /// <summary>
    /// If a string starts with this two bits, the value is stored as an offset to a previous position in the message.
    /// </summary>
    internal const uint StringPointerMask = 0b1100000000000000;

    internal const byte StringPointerByteMask = 0b11000000;

    /// <summary>
    /// The current offset of the reader / writer.
    /// </summary>
    protected int Offset => offset;

    /// <summary>
    /// Advance <see cref="Offset"/> by amount.
    /// </summary>
    /// <param name="amount"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected void AdvanceOffset(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "amount must be greater than zero.");

        offset += amount;
    }
}

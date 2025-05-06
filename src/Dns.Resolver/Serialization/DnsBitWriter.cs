using System.Text;

namespace Dns.Resolver.Serialization;

internal class DnsBitWriter : DnsBitReaderWriterBase
{
    private readonly Stream stream;
    private readonly Dictionary<string, int> stringOffsets = new Dictionary<string, int>();

    internal DnsBitWriter(Stream stream)
    {
        this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
    }

    public void Write(string value, bool useCompression)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        if (value.Length == 0)
            throw new ArgumentException("Please specify a non zero number of strings.");

        if (useCompression && stringOffsets.TryGetValue(value, out var previousOffset))
        {
            if ((previousOffset & 0b1100000000000000) != 0)
                throw new InvalidOperationException($"The offset of string '{value}' is too large.");

            ushort pointer = (ushort)(previousOffset | 0b1100000000000000);

            //This is a pointer
            Write(pointer);
        }
        else
        {
            //Store the offset before we move it
            stringOffsets.Add(value, Offset);

            var parts = value.Split('.');

            foreach (var part in parts)
            {
                if (part.Length > byte.MaxValue)
                    throw new ArgumentException($"Strings must be 256 characters or less. The specified string '{value}' was {value.Length} characters long.", nameof(value));

                Write((byte)part.Length);

                var buffer = Encoding.ASCII.GetBytes(part);

                Write(buffer);
            }

            //Null terminate the string.
            Write((byte)0x00);
        }
    }

    public void Write(byte value)
    {
        Write([value]);
    }

    public void Write(ushort value)
    {
        var buffer = bitConverter.GetBytes(value) ?? [0, 0];

        Write(buffer);
    }

    public void Write(uint value)
    {
        var buffer = bitConverter?.GetBytes(value) ?? [0, 0, 0, 0];

        Write(buffer);
    }

    public void Write(byte[] value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        stream.Write(value);

        AdvanceOffset(value.Length);
    }
}

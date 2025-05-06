using BitConverter;
using System.Text;

namespace Dns.Resolver.Serialization;

internal class DnsBitWriter
{
    private static EndianBitConverter bitConverter = EndianBitConverter.BigEndian;
    private readonly Stream stream;
    private uint offset;
    private readonly Dictionary<string, uint> stringOffsets = new Dictionary<string, uint>();

    internal DnsBitWriter(Stream stream)
    {
        this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
    }

    public void Write(string value, bool useCompression = true)
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
            stringOffsets.Add(value, offset);

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
        stream.Write([value]);

        offset++;
    }

    public void Write(ushort value)
    {
        var buffer = bitConverter.GetBytes(value) ?? [0, 0];

        stream.Write(buffer);

        offset += (uint)buffer.Length;
    }

    public void Write(uint value)
    {
        var buffer = bitConverter?.GetBytes(value) ?? [0, 0, 0, 0];

        stream.Write(buffer);

        offset += (uint)buffer.Length;
    }

    public void Write(byte[] value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        stream.Write(value);

        offset += (uint)value.Length;
    }
}

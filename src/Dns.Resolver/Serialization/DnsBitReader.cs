using System.Text;

namespace Dns.Resolver.Serialization
{
    internal class DnsBitReader : DnsBitReaderWriterBase
    {
        private readonly byte[] buffer;

        private readonly Dictionary<int, string> previouslyReadStrings = new Dictionary<int, string>();

        public DnsBitReader(byte[] buffer)
        {
            this.buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        }

        public string ReadString()
        {
            var peekByte = PeekByte();

            //Peek at the first byte to see if this is an offset or a length
            if (peekByte.IsStringPointerMask())
            {
                //This is an offset
                var rawPointerValue = ReadUInt16();

                //Strip off the pointer mask leading bits
                var pointerValue = rawPointerValue.ClearStringPointerMask();

                if (!previouslyReadStrings.TryGetValue(pointerValue, out var previouslyReadString))
                    throw new InvalidOperationException($"No string was previously read at offset {pointerValue}.");

                return previouslyReadString;
            }
            else
            {
                var parts = new List<string>();

                //Save the offset for later
                var stringOffset = Offset;

                var part = ReadStringPart();

                while (part != null)
                {
                    parts.Add(part);

                    part = ReadStringPart();
                }

                var result = parts.Count > 0 ? string.Join('.', parts) : string.Empty;

                previouslyReadStrings.Add(stringOffset, result);

                return result;
            }
        }

        private string? ReadStringPart()
        {
            var partLength = ReadByte();

            if (partLength == 0)
                return null;

            var value = Encoding.ASCII.GetString(buffer, Offset, partLength);

            AdvanceOffset(partLength);

            return value;
        }

        private byte PeekByte()
        {
            return buffer[Offset];
        }

        public byte ReadByte()
        {
            var value = buffer[Offset];

            AdvanceOffset(1);

            return value;
        }

        public ushort ReadUInt16() 
        {
            var value = bitConverter.ToUInt16(buffer, Offset);

            AdvanceOffset(2);

            return value;
        }

        public byte[] ReadBytes(int count)
        {
            if (count == 0)
                return Array.Empty<byte>();

            var value = new byte[count];

            Array.Copy(buffer, Offset, value, 0, count);

            AdvanceOffset(count);

            return value;
        }

        public UInt32 ReadUInt32()
        {
            var value = bitConverter.ToUInt32(buffer, Offset);

            AdvanceOffset(4);

            return value;
        }
    }
}

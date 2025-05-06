using System.Text;

namespace Dns.Resolver.Serialization
{
    internal class DnsBitReader : DnsBitReaderWriterBase
    {
        private readonly byte[] buffer;

        private const int MaxStringPartCount = 100;

        public DnsBitReader(byte[] buffer)
        {
            this.buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        }

        public string ReadString(OffsetManager offsetManager)
        {
            var parts = new List<string>();

            ReadStringParts(offsetManager, parts);

            return parts.Count > 0 ? string.Join('.', parts) : string.Empty;            
        }

        private void ReadStringParts(OffsetManager offsetManager, List<string> parts)
        {
            var currentOffsetManager = offsetManager;

            var count = 0;

            while (true)
            {
                count++;

                if (count > MaxStringPartCount)
                    throw new InvalidOperationException($"MaxStringPartCount {MaxStringPartCount} exceeded while parsing string.");

                var peekByte = PeekByte(currentOffsetManager);

                //Peek at the first byte to see if this is an offset or a length
                if (peekByte.IsStringPointerMask())
                {
                    //This is an offset
                    var rawPointerValue = ReadUInt16(currentOffsetManager);

                    //Strip off the pointer mask leading bits
                    var pointerValue = rawPointerValue.ClearStringPointerMask();

                    currentOffsetManager = new OffsetManager(pointerValue);
                }
                else
                {
                    //Read the size of the string part.
                    var partLength = ReadByte(currentOffsetManager);

                    //This is the end of the string parts.
                    if (partLength == 0)
                        return;

                    parts.Add(Encoding.ASCII.GetString(buffer, currentOffsetManager.Offset, partLength));

                    currentOffsetManager.Advance(partLength);
                }
            };
        }

        private byte PeekByte(OffsetManager offsetManager)
        {
            return buffer[offsetManager.Offset];
        }

        public byte ReadByte(OffsetManager offsetManager)
        {
            var value = buffer[offsetManager.Offset];

            offsetManager.Advance(1);

            return value;
        }

        public ushort ReadUInt16(OffsetManager offsetManager) 
        {
            var value = bitConverter.ToUInt16(buffer, offsetManager.Offset);

            offsetManager.Advance(2);

            return value;
        }

        public byte[] ReadBytes(OffsetManager offsetManager, int count)
        {
            if (count == 0)
                return Array.Empty<byte>();

            var value = new byte[count];

            Array.Copy(buffer, offsetManager.Offset, value, 0, count);

            offsetManager.Advance(count);

            return value;
        }

        public UInt32 ReadUInt32(OffsetManager offsetManager)
        {
            var value = bitConverter.ToUInt32(buffer, offsetManager.Offset);

            offsetManager.Advance(4);

            return value;
        }
    }
}

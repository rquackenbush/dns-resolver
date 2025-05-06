using Dns.Resolver.Messages;

namespace Dns.Resolver.Serialization;

// https://mislove.org/teaching/cs4700/spring11/handouts/project1-primer.pdf

public class MessageSerializer
{
    public static byte[] Serialize(Message message)
    {
        if (message.Header == null)
            throw new ArgumentException("message must have a Header.");

        using var stream = new MemoryStream();

        var writer = new DnsBitWriter(stream);

        ushort questionCount = (ushort)(message.Questions?.Length ?? 0);
        ushort answerCount = (ushort)(message.Answers?.Length ?? 0);
        ushort nameServerRRCount = (ushort)(message.NameServerResourceRecords?.Length ?? 0);
        ushort additionalRRCount = (ushort)(message.AdditionalResourceRecords?.Length ?? 0);

        WriteHeader(message.Header, questionCount, answerCount, nameServerRRCount, additionalRRCount, writer);
        WriteQuestions(message.Questions, writer);
        WriteResourceRecords(message.Answers, writer);
        WriteResourceRecords(message.NameServerResourceRecords, writer);
        WriteResourceRecords(message.AdditionalResourceRecords, writer);

        return stream.ToArray();
    }

    private static void WriteHeader(Header header, ushort questionCount, ushort answerCount, ushort nameServerRRCount, ushort additionalRRCount, DnsBitWriter writer)
    {
        writer.Write(header.TransactionId);

        WriteHeaderFlags(header.Flags, writer);

        writer.Write(questionCount);
        writer.Write(answerCount);
        writer.Write(nameServerRRCount);
        writer.Write(additionalRRCount);

    }

    private static void WriteHeaderFlags(HeaderFlags flags, DnsBitWriter writer)
    {
        ushort value = (ushort)(
           DoubleByteHelper.GetBit(flags.QueryReply == QueryReply.Reply, 0) |
           DoubleByteHelper.GetQuadBits((byte)flags.OpCode, 1) |
           DoubleByteHelper.GetBit(flags.AuthoritativeAnswer, 5) |
           DoubleByteHelper.GetBit(flags.TrunCation, 6) |
           DoubleByteHelper.GetBit(flags.RecursionDesired, 7) |
           DoubleByteHelper.GetBit(flags.RecursionAvailable, 8) |
           DoubleByteHelper.GetQuadBits(flags.ResponseCode, 12));

        writer.Write(value);
    }

    private static void WriteQuestions(Question[]? questions, DnsBitWriter writer)
    {
        if (questions == null)
            return;

        foreach(var question in questions)
        {
            WriteQuestion(question, writer);
        }
    }

    private static void WriteQuestion(Question question, DnsBitWriter writer)
    {
        writer.Write(question.Name);

        writer.Write(question.Type);
        writer.Write(question.ClassCode);
    }

    private static void WriteResourceRecords(ResourceRecord[]? resourceRecords, DnsBitWriter writer)
    {
        if (resourceRecords == null) 
            return;

        foreach(var resourceRecord in resourceRecords)
        {
            WriteResourceRecord(resourceRecord, writer);
        }
    }

    private static void WriteResourceRecord(ResourceRecord resourceRecord, DnsBitWriter writer)
    {
        writer.Write(resourceRecord.Name);
        writer.Write(resourceRecord.Type);
        writer.Write(resourceRecord.Class);
        writer.Write(resourceRecord.TimeToLive);

        if (resourceRecord.ResourceRecordData == null)
        {
            writer.Write((ushort)0);
        }
        else
        {
            writer.Write((ushort)resourceRecord.ResourceRecordData.Length);
            writer.Write(resourceRecord.ResourceRecordData);
        }
    }

    public static Message Deserialize(byte[] buffer)
    {
        throw new NotImplementedException();
    }
}

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

    public static ushort GetQuadBits(byte value, int position)
    { 
        if (value > 0b1111)
            throw new ArgumentOutOfRangeException(nameof(value), "value must be between 0 and 0b1111.");

        if (position > 12)
              throw new ArgumentOutOfRangeException(nameof(position), "position must be between 0 and 3.");

        return (ushort)(value << (15 -  position));
    }
}

internal static class ByteHelper
{
    public static byte GetBit(bool value, int position)
    { 
        if (position > 7)
            throw new ArgumentOutOfRangeException(nameof(position), "position must be between 0 and 7.");

        if (value)
            return (byte)(0b01 << (7 - position));

        return 0;
    }

    public static byte GetQuadBits(byte value, int position)
    {
        if (value > 0b1111)
            throw new ArgumentOutOfRangeException(nameof(value), "value must be between 0 and 0b1111.");

        if (position > 3)
            throw new ArgumentOutOfRangeException(nameof(position), "position must be between 0 and 3.");

        return (byte)(value << (7 - position));
    }
}

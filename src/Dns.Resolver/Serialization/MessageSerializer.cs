using Dns.Resolver.Messages;

namespace Dns.Resolver.Serialization;

// https://mislove.org/teaching/cs4700/spring11/handouts/project1-primer.pdf

public class MessageSerializer
{
    /// <summary>
    /// Trivial class to group commonly needed objects for implementation.
    /// </summary>
    /// <param name="Writer"></param>
    /// <param name="Options"></param>
    private record class WriterContext(DnsBitWriter Writer, MessageSerializerOptions Options)
    {
    }

    public static byte[] Serialize(Message message, MessageSerializerOptions? options = null)
    {
        if (message.Header == null)
            throw new ArgumentException("message must have a Header.");

        using var stream = new MemoryStream();

        var writer = new DnsBitWriter(stream);

        var context = new WriterContext(new DnsBitWriter(stream), options ?? MessageSerializerOptions.Default);

        ushort questionCount = (ushort)(message.Questions?.Length ?? 0);
        ushort answerCount = (ushort)(message.Answers?.Length ?? 0);
        ushort nameServerRRCount = (ushort)(message.NameServerResourceRecords?.Length ?? 0);
        ushort additionalRRCount = (ushort)(message.AdditionalResourceRecords?.Length ?? 0);

        WriteHeader(context, message.Header, questionCount, answerCount, nameServerRRCount, additionalRRCount);
        WriteQuestions(context, message.Questions);
        WriteResourceRecords(context, message.Answers);
        WriteResourceRecords(context, message.NameServerResourceRecords);
        WriteResourceRecords(context, message.AdditionalResourceRecords);

        return stream.ToArray();
    }

    private static void WriteHeader(WriterContext context, Header header, ushort questionCount, ushort answerCount, ushort nameServerRRCount, ushort additionalRRCount)
    {
        context.Writer.Write(header.TransactionId);

        WriteHeaderFlags(context, header.Flags);

        context.Writer.Write(questionCount);
        context.Writer.Write(answerCount);
        context.Writer.Write(nameServerRRCount);
        context.Writer.Write(additionalRRCount);
    }

    private static void WriteHeaderFlags(WriterContext context, HeaderFlags flags)
    {
        ushort value = (ushort)(
           DoubleByteHelper.GetBit(flags.QueryReply == QueryReply.Reply, 0) |
           DoubleByteHelper.GetQuadBits((byte)flags.OpCode, 1) |
           DoubleByteHelper.GetBit(flags.AuthoritativeAnswer, 5) |
           DoubleByteHelper.GetBit(flags.Truncated, 6) |
           DoubleByteHelper.GetBit(flags.RecursionDesired, 7) |
           DoubleByteHelper.GetBit(flags.RecursionAvailable, 8) |
           DoubleByteHelper.GetQuadBits(flags.ResponseCode, 12));

        context.Writer.Write(value);
    }

    private static void WriteQuestions(WriterContext context, Question[]? questions)
    {
        if (questions == null)
            return;

        foreach(var question in questions)
        {
            context.Writer.Write(question.Name, context.Options.CompressStrings);

            context.Writer.Write(question.Type);
            context.Writer.Write(question.ClassCode);
        }
    }

    private static void WriteResourceRecords(WriterContext context, ResourceRecord[]? resourceRecords)
    {
        if (resourceRecords == null) 
            return;

        foreach(var resourceRecord in resourceRecords)
        {
            context.Writer.Write(resourceRecord.Name, context.Options.CompressStrings);
            context.Writer.Write(resourceRecord.Type);
            context.Writer.Write(resourceRecord.Class);
            context.Writer.Write(resourceRecord.TimeToLive);

            if (resourceRecord.ResourceRecordData == null)
            {
                context.Writer.Write((ushort)0);
            }
            else
            {
                context.Writer.Write((ushort)resourceRecord.ResourceRecordData.Length);
                context.Writer.Write(resourceRecord.ResourceRecordData);
            }
        }
    }

    /// <summary>
    /// Trivial class to group commonly needed objects for implementation.
    /// </summary>
    /// <param name="Reader"></param>
    /// <param name="Options"></param>
    private record class ReaderContext(DnsBitReader Reader, MessageSerializerOptions Options)
    {
    }

    public static Message Deserialize(byte[] buffer, MessageSerializerOptions? options = null)
    {
        options = options ?? MessageSerializerOptions.Default;

        var reader = new DnsBitReader(buffer);

        var context = new ReaderContext(reader, options);

        var headerResult = ReadHeader(context);

        var questions = ReadQuestions(context, headerResult.QuestionCount);
        var answers = ReadResourceRecords(context, headerResult.AnswerCount);
        var nameServerResourceRecords = ReadResourceRecords(context, headerResult.NameServerRRCount);
        var additionalResourceRecords = ReadResourceRecords(context, headerResult.AdditionalRRCount);

        return new Message { 
            Header = headerResult.Header,
        };
    }

    private static Question[] ReadQuestions(ReaderContext context, int count)
    {
        var questions = new Question[count];

        for (var index = 0; index < count; index++) 
        {
            questions[index] = ReadQuestion(context);
        }

        return questions.ToArray();
    }

    private static Question ReadQuestion(ReaderContext context)
    {
        return  new Question
        {
            Name = context.Reader.ReadString(),
            Type = context.Reader.ReadUInt16(),
            ClassCode = context.Reader.ReadUInt16()
        };
    }

    private static ResourceRecord[] ReadResourceRecords(ReaderContext context, int count)
    {
        var resourceRecords = new ResourceRecord[count];

        for(var index = 0; index < count; index++)
        {
            resourceRecords[index] = ReadResourceRecord(context);
        }

        return resourceRecords;
    }

    private static ResourceRecord ReadResourceRecord(ReaderContext context)
    {
        var resourceRecord = new ResourceRecord
        {
            Name = context.Reader.ReadString(),
            Type = context.Reader.ReadUInt16(),
            Class = context.Reader.ReadUInt16(),
            TimeToLive = context.Reader.ReadUInt32()
        };

        var length = context.Reader.ReadUInt16();

        if (length > 0)
        {
            resourceRecord.ResourceRecordData = context.Reader.ReadBytes(length);
        }

        return resourceRecord;
    }

    private static ReadHeaderResult ReadHeader(ReaderContext context)
    {
        var header = new Header
        {
            TransactionId = context.Reader.ReadUInt16(),
            Flags = ReadFlags(context),
        };

        return new ReadHeaderResult(
            header,
            context.Reader.ReadUInt16(),
            context.Reader.ReadUInt16(),
            context.Reader.ReadUInt16(),
            context.Reader.ReadUInt16());
    }



    private static HeaderFlags ReadFlags(ReaderContext context)
    {
        var value = context.Reader.ReadUInt16();

        return new HeaderFlags
        {
            QueryReply = value.GetBit(0) ? QueryReply.Reply : QueryReply.Query,
            OpCode = (OpCode)(value.GetQuadBits(1)),
            AuthoritativeAnswer = value.GetBit(5),
            Truncated = value.GetBit(6),
            RecursionDesired = value.GetBit(7),
            ResponseCode = value.GetQuadBits(12)
        };
    }

    private record class ReadHeaderResult(Header Header, ushort QuestionCount, ushort AnswerCount, ushort NameServerRRCount, ushort AdditionalRRCount)
    {
    }

    /*
       
     */
}

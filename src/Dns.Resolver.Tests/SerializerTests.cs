using Dns.Resolver.Messages;
using Dns.Resolver.Serialization;
using Shouldly;

namespace Dns.Resolver.Tests
{
    public class SerializerTests
    {
        [Fact]
        public void SerializeRequestExample1()
        {
            var message = new Message
            {
                Header = new Header
                {
                    TransactionId = 0xdb42,
                    Flags = new HeaderFlags
                    {
                        QueryReply = QueryReply.Query,
                        RecursionDesired = true
                    }
                },
                Questions = [
                    new Question {
                        Name = "www.northeastern.edu",
                        Type = QueryTypes.A,
                        ClassCode = QueryClasses.InternetAddress
                    }
                ]
            };

            var expectedBytes = new byte[] {
                0xdb, 0x42, 0x01, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x77, 0x77, 0x77,
                0x0c, 0x6e, 0x6f, 0x72, 0x74, 0x68, 0x65, 0x61, 0x73, 0x74, 0x65, 0x72, 0x6e, 0x03, 0x65, 0x64,
                0x75, 0x00, 0x00, 0x01, 0x00, 0x01
            };

            var actualBytes = MessageSerializer.Serialize(message).GetHexString();

            actualBytes.ShouldBe(expectedBytes.GetHexString());
        }

        [Fact]
        public void SerializeResponseExample1()
        {
            var message = new Message
            {
                Header = new Header
                {
                    TransactionId = 0xdb42,
                    Flags = new HeaderFlags
                    {
                        QueryReply = QueryReply.Reply,
                        RecursionDesired = true,
                        RecursionAvailable = true
                    }
                },
                Questions = [
                    new Question {
                        Name = "www.northeastern.edu",
                        Type = QueryTypes.A,
                        ClassCode = QueryClasses.InternetAddress
                    }
                ],
                Answers = [
                    new ResourceRecord {
                        Name = "www.northeastern.edu",
                        Type = QueryTypes.A,
                        Class = QueryClasses.InternetAddress,
                        TimeToLive = 600,
                        ResourceRecordData = [
                            155, 33, 17, 68
                        ]
                      }
                 ]
            };

            var expectedBytes = new byte[]
            {
                0xdb, 0x42, 0x81, 0x80, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x03, 0x77, 0x77, 0x77,
                0x0c, 0x6e, 0x6f, 0x72, 0x74, 0x68, 0x65, 0x61, 0x73, 0x74, 0x65, 0x72, 0x6e, 0x03, 0x65, 0x64,
                0x75, 0x00, 0x00, 0x01, 0x00, 0x01, 0xc0, 0x0c, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x02, 0x58,
                0x00, 0x04, 0x9b, 0x21, 0x11, 0x44
            };

            var actualBytes = MessageSerializer.Serialize(message).GetHexString();

            actualBytes.ShouldBe(expectedBytes.GetHexString());

        }
    }
}
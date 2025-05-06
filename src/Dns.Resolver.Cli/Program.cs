using Dns.Resolver;
using Dns.Resolver.Messages;
using System.Net;

var dnsClient = new DnsClient();

var dnsIp = new IPAddress(new byte[] { 10, 13, 5, 6 });

var dnsEndpoint = new IPEndPoint(dnsIp, 53);

var request = new Message
{
    Header = new Header
    {
        TransactionId = 0x0042,
        Flags = new HeaderFlags
        {
            QueryReply = QueryReply.Query,
            RecursionDesired = true
        }
    },
    Questions =
    [
        new Question
        {
            Name = "digitaltwins-api.atr.prod.acuitynext.io",
            Type = QueryTypes.A,
            ClassCode = QueryClasses.InternetAddress
        }
    ]
};

var response = await dnsClient.SendAsync(request, dnsEndpoint);
Console.WriteLine(response.Answers?.Length ?? 0);

await Task.Delay(TimeSpan.FromSeconds(2));

var response2 = await dnsClient.SendAsync(request, dnsEndpoint);
Console.WriteLine(response.Answers?.Length ?? 0);
using Dns.Resolver.Messages;
using Dns.Resolver.Serialization;
using System.Net;
using System.Net.Sockets;

namespace Dns.Resolver;

public class DnsClient
{
    private readonly UdpClient udpClient = new UdpClient();

    public async Task<Message> SendAsync(Message request, IPEndPoint dnsServerEndpoint, CancellationToken cancellationToken = default)
    {
        var requestBytes = MessageSerializer.Serialize(request);

        await udpClient.SendAsync(requestBytes, dnsServerEndpoint, cancellationToken);

        var receiveResult = await udpClient.ReceiveAsync(cancellationToken);

        return MessageSerializer.Deserialize(receiveResult.Buffer);
    }
}

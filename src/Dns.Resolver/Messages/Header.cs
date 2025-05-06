namespace Dns.Resolver.Messages;

public class Header
{
    /// <summary>
    /// Transaction ID.
    /// </summary>
    public required ushort TransactionId { get; set; }

    /// <summary>
    /// Flags.
    /// </summary>
    public required HeaderFlags Flags { get; set; }
}

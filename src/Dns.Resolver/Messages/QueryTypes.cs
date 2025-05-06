namespace Dns.Resolver.Messages;

public static class QueryTypes
{
    /// <summary>
    /// Host Addresses - A records
    /// </summary>
    public const ushort A = 0x0001;

    /// <summary>
    /// Mail servers - MX records.
    /// </summary>
    public const ushort MX = 0x0005;

    /// <summary>
    /// Name servers - NS records.
    /// </summary>
    public const ushort NS = 0x0002;
}

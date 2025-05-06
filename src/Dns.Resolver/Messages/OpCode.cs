namespace Dns.Resolver.Messages;

public enum OpCode : byte
{
    /// <summary>
    /// Standard Query
    /// </summary>
    Query = 0,

    /// <summary>
    /// Inverse Query
    /// </summary>
    IQuery = 1,

    Status = 2
}



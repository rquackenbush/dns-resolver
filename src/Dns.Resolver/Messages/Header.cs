namespace Dns.Resolver.Messages;

public class Header
{
    /// <summary>
    /// Transaction ID
    /// </summary>
    public required ushort TransactionId { get; set; }

    public required HeaderFlags Flags { get; set; }

    ///// <summary>
    ///// AD: 1 bit
    ///// Authentic Data, in a response, indicates if the replying DNS server verified the data.
    ///// </summary>
    //public bool AuthenticData { get; set; }

    ///// <summary>
    ///// CD: 1 bit
    ///// Checking Disabled, in a query, indicates that non-verified data is acceptable in a response.
    ///// </summary>
    //public bool CheckingDisabled { get; set; }
}

public class HeaderFlags
{
    /// <summary>
    /// QR: 1 bit
    /// Indicates if the message is a query (0) or a reply (1).
    /// </summary>
    public required QueryReply QueryReply { get; set; }

    /// <summary>
    /// OPCODE: 4 bits
    /// The type can be QUERY (standard query, 0), IQUERY (inverse query, 1), or STATUS (server status request, 2).
    /// </summary>
    public OpCode OpCode { get; set; }

    /// <summary>
    /// AA: 1 bit
    /// Authoritative Answer, in a response, indicates if the DNS server is authoritative for the queried hostname.
    /// </summary>
    public bool AuthoritativeAnswer { get; set; }

    /// <summary>
    /// TC: 1 bit
    /// TrunCation, indicates that this message was truncated due to excessive length.
    /// </summary>
    public bool TrunCation { get; set; }

    /// <summary>
    /// RD: 1 bit
    /// Recursion Desired, indicates if the client means a recursive query.
    /// </summary>
    public bool RecursionDesired { get; set; }

    /// <summary>
    /// RA: 1 bit
    /// Recursion Available, in a response, indicates if the replying DNS server supports recursion.
    /// </summary>
    public bool RecursionAvailable { get; set; }

    /// <summary>
    /// Z: 1 bit; (Z) == 0
    /// Zero, reserved for future use.
    /// </summary>
    public bool Reserved { get; set; }

    /// <summary>
    /// RCODE: 4 bits
    /// Response code, can be NOERROR(0), FORMERR(1, Format error), SERVFAIL(2), NXDOMAIN(3, Nonexistent domain), etc.[36]
    /// See <see cref="ResponseCodes"/>
    /// </summary>
    public byte ResponseCode { get; set; }
}

namespace Dns.Resolver.Messages;

/// <summary>
/// DNS Response Codes.
/// https://www.iana.org/assignments/dns-parameters/dns-parameters.xhtml#dns-parameters-6
/// </summary>
public static class ResponseCodes
{
    /// <summary>
    /// No Error [RFC1035]
    /// </summary>
    public const byte NoError = 0;

    /// <summary>
    /// Format Error [RFC1035]
    /// </summary>
    public const byte FormatError = 1;

    /// <summary>
    /// Server Failure [RFC1035]
    /// </summary>
    public const byte ServFail = 2;

    /// <summary>
    /// Non-Existent Domain [RFC1035]
    /// </summary>
    public const byte NXDomain = 3;

    /// <summary>
    /// Not implemented [RFC1035]
    /// </summary>
    public const byte NotImpl = 4;

    /// <summary>
    /// Refused [RFC1035]
    /// </summary>
    public const byte Refused = 5;

    /// <summary>
    /// Name Exists when it should not [RFC2136][RFC6672]
    /// </summary>
    public const byte YXDomain = 6;

    /// <summary>
    /// RR Set Exists when it should not [RFC2136]
    /// </summary>
    public const byte YXRRSet = 7;

    /// <summary>
    /// RR Set that should exist does not [RFC2136]
    /// </summary>
    public const byte NXRRSet = 8;

    /// <summary>
    /// Server Not Authoritative for zone [RFC2136]
    /// </summary>
    public const byte NotAuth = 9;

    /// <summary>
    /// Name not contained in zone [RFC2136]
    /// </summary>
    public const byte NotZone = 10;

    /// <summary>
    /// DSO-TYPE Not Implement [RFC8490]
    /// </summary>
    public const byte DSOTYPENI = 11;

    /// <summary>
    /// Bad OPT Version	[RFC6891]
    /// </summary>
    public const byte BADVERS = 16;

    /// <summary>
    /// TSIG Signature Failure [RFC8945]
    /// </summary>
    public const byte BADSIG = 16;

    /*
17	BADKEY	Key not recognized	[RFC8945]
18	BADTIME	Signature out of time window	[RFC8945]
19	BADMODE	Bad TKEY Mode	[RFC2930]
20	BADNAME	Duplicate key name	[RFC2930]
21	BADALG	Algorithm not supported	[RFC2930]
22	BADTRUNC	Bad Truncation	[RFC8945]
23	BADCOOKIE	Bad/missing Server Cookie	[RFC7873]
     */
}



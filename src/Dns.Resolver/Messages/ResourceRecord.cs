namespace Dns.Resolver.Messages;

public class ResourceRecord
{
    /// <summary>
    /// Name: Name of the node to which this record pertains
    /// 
    /// NAME is the fully qualified domain name of the node in the tree.[clarification needed] On the wire, 
    /// the name may be shortened using label compression where ends of domain names mentioned earlier 
    /// in the packet can be substituted for the end of the current domain name.
    /// 
    /// Length: variable
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// TYPE: Type of RR in numeric form (e.g., 15 for MX RRs)
    /// 
    /// TYPE is the record type. It indicates the format of the data and it gives a hint of its intended use. 
    /// For example, the A record is used to translate from a domain name to an IPv4 address, the NS record lists which name servers 
    /// can answer lookups on a DNS zone, and the MX record specifies the mail server used to handle mail for a domain specified 
    /// in an e-mail address.
    /// 
    /// Length: 2 octets
    /// </summary>
    public ushort Type { get; set; }

    /// <summary>
    /// CLASS: Class code.
    /// 
    /// The CLASS of a record is set to IN (for Internet) for common DNS records involving Internet hostnames, servers, 
    /// or IP addresses. In addition, the classes Chaos (CH) and Hesiod (HS) exist.[38]: 11  Each class is an independent 
    /// name space with potentially different delegations of DNS zones.
    ///
    /// Length: 2
    /// </summary>
    public ushort Class { get; set; }

    /// <summary>
    /// TTL: Count of seconds that the RR stays valid (The maximum is 231^−1, which is about 68 years)
    /// 
    /// Length 4 octets.
    /// </summary>
    public uint TimeToLive { get; set; }

    ///// <summary>
    ///// RDLENGTH:  of RDATA field (specified in octets) 
    ///// 
    ///// Length: 2 octets
    ///// </summary>
    //public ushort ResourceRecordDataLength { get; set; }

    /// <summary>
    /// RDATA: Additional RR-specific data. 
    /// 
    /// Length: Variable, as per RDLENGTH
    /// </summary>
    public byte[]? ResourceRecordData { get; set; }
}
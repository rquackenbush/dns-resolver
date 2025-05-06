namespace Dns.Resolver.Messages;

/// <summary>
/// The question section has a simpler format than the resource record format used in the other sections.
/// </summary>
public class Question
{
    /// <summary>
    /// NAME: Name of the requested resource. Length: variable
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// TYPE: Type of RR (A, AAAA, MX, TXT, etc.) Length: 2 Octets
    /// <seealso cref="QueryTypes"/>
    /// </summary>
    public required ushort Type { get; set; }

    /// <summary>
    /// CLASS:  Class Code. Length: 2 Octets
    /// <seealso cref="QueryClasses"/>
    /// </summary>
    public required ushort ClassCode { get; set; }
}

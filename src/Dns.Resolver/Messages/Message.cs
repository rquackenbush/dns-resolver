namespace Dns.Resolver.Messages;

public class Message
{
    /// <summary>
    /// Header is 12 bytes
    /// </summary>
    public required Header Header { get; set; }

    public Question[]? Questions { get; set; }

    public ResourceRecord[]? Answers { get; set; }

    public ResourceRecord[]? NameServerResourceRecords { get; set; }

    public ResourceRecord[]? AdditionalResourceRecords { get; set; }
}

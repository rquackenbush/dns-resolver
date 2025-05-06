namespace Dns.Resolver.Serialization;

public class MessageSerializerOptions
{
    // Default serialization options
    internal static MessageSerializerOptions Default = new MessageSerializerOptions();

    /// <summary>
    /// True to compress strings, false otherwise.
    /// </summary>
    public bool CompressStrings { get; set; } = true;
}

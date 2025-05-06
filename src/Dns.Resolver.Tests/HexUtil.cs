namespace Dns.Resolver.Tests;

internal static class HexUtil
{
    public static string GetHexString(this byte[] ba)
    {
        return System.BitConverter.ToString(ba).Replace("-", "");
    }
}

namespace HFM.Client.Internal;

internal static class TcpPort
{
    internal static bool Validate(int port) =>
        port >= 0 && port <= UInt16.MaxValue;
}

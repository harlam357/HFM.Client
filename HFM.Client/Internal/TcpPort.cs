
using System;

namespace HFM.Client.Internal
{
    internal static class TcpPort
    {
        internal static bool Validate(int port)
        {
            return port >= 0 && port <= UInt16.MaxValue;
        }
    }
}

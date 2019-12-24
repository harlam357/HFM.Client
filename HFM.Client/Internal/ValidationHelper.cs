
using System;

namespace HFM.Client.Internal
{
    internal static class ValidationHelper
    {
        internal static bool ValidateTcpPort(int port)
        {
            return port >= 0 && port <= UInt16.MaxValue;
        }
    }
}

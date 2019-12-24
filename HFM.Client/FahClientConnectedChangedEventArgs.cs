
using System;

namespace HFM.Client
{
    /// <summary>
    /// Provides event data for connection status events of a <see cref="FahClientConnection"/>. This class cannot be inherited.
    /// </summary>
    public sealed class FahClientConnectedChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets a value indicating whether the connection is connected to a client.
        /// </summary>
        public bool Connected { get; }

        internal FahClientConnectedChangedEventArgs(bool connected)
        {
            Connected = connected;
        }
    }
}
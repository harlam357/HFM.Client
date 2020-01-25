
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

        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientCommand"/> class.
        /// </summary>
        /// <param name="connected">Value indicating whether the connection is connected to a client.</param>
        public FahClientConnectedChangedEventArgs(bool connected)
        {
            Connected = connected;
        }
    }
}
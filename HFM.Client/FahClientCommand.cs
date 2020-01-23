﻿
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HFM.Client
{
    /// <summary>
    /// Provides the abstract base class for a Folding@Home client command statement.
    /// </summary>
    public abstract class FahClientCommandBase
    {
        /// <summary>
        /// Gets or sets the Folding@Home client command statement.
        /// </summary>
        public string CommandText { get; set; } = String.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientCommandBase"/> class.
        /// </summary>
        protected FahClientCommandBase()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientCommandBase"/> class.
        /// </summary>
        /// <param name="commandText">The Folding@Home client command statement.</param>
        protected FahClientCommandBase(string commandText)
        {
            CommandText = commandText;
        }

        /// <summary>
        /// Sends the <see cref="CommandText"/> to the Folding@Home client.
        /// </summary>
        /// <returns>The number of bytes transmitted to the Folding@Home client.</returns>
        public abstract int Execute();

        /// <summary>
        /// Asynchronously sends the <see cref="CommandText"/> to the Folding@Home client.
        /// </summary>
        /// <returns>A task representing the asynchronous operation that can return the number of bytes transmitted to the Folding@Home client.</returns>
        public abstract Task<int> ExecuteAsync();
    }

    /// <summary>
    /// Folding@Home client command statement.
    /// </summary>
    public class FahClientCommand : FahClientCommandBase
    {
        /// <summary>
        /// Gets the <see cref="FahClientTcpConnection"/> used by this <see cref="FahClientCommand"/>.
        /// </summary>
        public FahClientTcpConnection Connection { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientCommand"/> class.
        /// </summary>
        /// <param name="connection">A <see cref="FahClientTcpConnection"/> that represents the connection to a Folding@Home client.</param>
        public FahClientCommand(FahClientTcpConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientCommand"/> class.
        /// </summary>
        /// <param name="connection">A <see cref="FahClientTcpConnection"/> that represents the connection to a Folding@Home client.</param>
        /// <param name="commandText">The Folding@Home client command statement.</param>
        public FahClientCommand(FahClientTcpConnection connection, string commandText)
           : base(commandText)
        {
            Connection = connection;
        }

        /// <summary>
        /// Sends the <see cref="FahClientCommandBase.CommandText"/> to the Folding@Home client.
        /// </summary>
        /// <exception cref="InvalidOperationException">The connection is not open.</exception>
        /// <returns>The number of bytes transmitted to the Folding@Home client.</returns>
        public override int Execute()
        {
            if (!Connection.Connected) throw new InvalidOperationException("The connection is not open.");

            var stream = GetStream();
            byte[] buffer = GetBuffer(CommandText);

            try
            {
                stream.Write(buffer, 0, buffer.Length);
                return buffer.Length;
            }
            catch (Exception)
            {
                Connection.Close();
                throw;
            }
        }

        /// <summary>
        /// Asynchronously sends the <see cref="FahClientCommandBase.CommandText"/> to the Folding@Home client.
        /// </summary>
        /// <exception cref="InvalidOperationException">The connection is not open.</exception>
        /// <returns>A task representing the asynchronous operation that can return the number of bytes transmitted to the Folding@Home client.</returns>
        public override async Task<int> ExecuteAsync()
        {
            if (!Connection.Connected) throw new InvalidOperationException("The connection is not open.");

            var stream = GetStream();
            byte[] buffer = GetBuffer(CommandText);

            try
            {
                await stream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                return buffer.Length;
            }
            catch (Exception)
            {
                Connection.Close();
                throw;
            }
        }

        private Stream GetStream()
        {
            var stream = Connection.TcpConnection?.GetStream();
            if (stream is null)
            {
                throw new InvalidOperationException("The connection is not open.");
            }
            return stream;
        }

        private static byte[] GetBuffer(string commandText)
        {
            if (commandText is null)
            {
                return new byte[0];
            }
            if (!commandText.EndsWith("\n", StringComparison.Ordinal))
            {
                commandText += "\n";
            }
            return Encoding.ASCII.GetBytes(commandText);
        }
    }
}

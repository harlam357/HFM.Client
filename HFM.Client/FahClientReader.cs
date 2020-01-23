
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HFM.Client
{
    /// <summary>
    /// Provides the abstract base class for a Folding@Home client message reader.
    /// </summary>
    public abstract class FahClientReaderBase
    {
        /// <summary>
        /// Gets the last message read by the reader.
        /// </summary>
        public FahClientMessage Message { get; protected set; }

        /// <summary>
        /// Advances the reader to the next message received from the Folding@Home client and stores it in the <see cref="Message"/> property.
        /// </summary>
        /// <returns>true if a message was read; otherwise false.</returns>
        public abstract bool Read();

        /// <summary>
        /// Asynchronously advances the reader to the next message received from the Folding@Home client and stores it in the <see cref="Message"/> property.
        /// </summary>
        /// <returns>A task representing the asynchronous operation that can return true if a message was read; otherwise false.</returns>
        public abstract Task<bool> ReadAsync();
    }

    /// <summary>
    /// Folding@Home client message reader.
    /// </summary>
    public class FahClientReader : FahClientReaderBase
    {
        /// <summary>
        /// Gets the <see cref="FahClientConnection"/> used by this <see cref="FahClientReader"/>.
        /// </summary>
        public FahClientConnection Connection { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientReader"/> class.
        /// </summary>
        /// <param name="connection">A <see cref="FahClientConnection"/> that represents the connection to a Folding@Home client.</param>
        public FahClientReader(FahClientConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Gets or sets the size of the read buffer, in bytes. The default value is 1024 bytes.
        /// </summary>
        public int BufferSize { get; set; } = 1024;

        /// <summary>
        /// Advances the reader to the next message received from the Folding@Home client and stores it in the <see cref="FahClientReaderBase.Message"/> property.
        /// </summary>
        /// <exception cref="InvalidOperationException">The connection is not open.</exception>
        /// <returns>true if a message was read; otherwise false.</returns>
        public override bool Read()
        {
            if (!Connection.Connected) throw new InvalidOperationException("The connection is not open.");

            int bytesRead;
            var buffer = new byte[BufferSize];
            while ((bytesRead = ReadInternal(buffer)) != 0)
            {
                if (GetNextMessage(buffer, bytesRead))
                {
                    return true;
                }
            }
            return false;
        }

        private int ReadInternal(byte[] buffer)
        {
            var stream = GetStream();

            try
            {
                return stream.Read(buffer, 0, buffer.Length);
            }
            catch (Exception)
            {
                Connection.Close();
                throw;
            }
        }

        /// <summary>
        /// Asynchronously advances the reader to the next message received from the Folding@Home client and stores it in the <see cref="FahClientReaderBase.Message"/> property.
        /// </summary>
        /// <exception cref="InvalidOperationException">The connection is not open.</exception>
        /// <returns>A task representing the asynchronous operation that can return true if a message was read; otherwise false.</returns>
        public override async Task<bool> ReadAsync()
        {
            if (!Connection.Connected) throw new InvalidOperationException("The connection is not open.");

            int bytesRead;
            var buffer = new byte[BufferSize];
            while ((bytesRead = await ReadAsyncInternal(buffer).ConfigureAwait(false)) != 0)
            {
                if (GetNextMessage(buffer, bytesRead))
                {
                    return true;
                }
            }
            return false;
        }

        private async Task<int> ReadAsyncInternal(byte[] buffer)
        {
            var stream = GetStream();

            try
            {
                return await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
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

        private readonly StringBuilder _readBuffer = new StringBuilder();

        private FahClientMessageExtractorBase _messageExtractor;
        /// <summary>
        /// Gets or sets the message extractor responsible for extracting messages from the data received from the Folding@Home client.  The default implementation extracts messages as JSON text.
        /// </summary>
        public FahClientMessageExtractorBase MessageExtractor
        {
            get => _messageExtractor ?? (_messageExtractor = new FahClientJsonMessageExtractor());
            set => _messageExtractor = value;
        }

        private bool GetNextMessage(byte[] buffer, int bytesRead)
        {
            _readBuffer.Append(Encoding.ASCII.GetChars(buffer, 0, bytesRead));
            var nextMessage = MessageExtractor.Extract(_readBuffer);
            if (nextMessage != null)
            {
                Message = nextMessage;
            }
            return nextMessage != null;
        }
    }
}

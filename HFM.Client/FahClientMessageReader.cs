
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HFM.Client
{
    /// <summary>
    /// Folding@Home client message reader.
    /// </summary>
    public class FahClientMessageReader : FahClientReader
    {
        /// <summary>
        /// Gets the <see cref="FahClientTcpConnection"/> used by this <see cref="FahClientMessageReader"/>.
        /// </summary>
        public FahClientTcpConnection Connection { get; }

        /// <summary>
        /// Gets the last message read by the reader.
        /// </summary>
        public FahClientMessage Message { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientMessageReader"/> class.
        /// </summary>
        /// <param name="connection">A <see cref="FahClientTcpConnection"/> that represents the connection to a Folding@Home client.</param>
        public FahClientMessageReader(FahClientTcpConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Gets or sets the size of the read buffer, in bytes. The default value is 1024 bytes.
        /// </summary>
        public int BufferSize { get; set; } = 1024;

        /// <summary>
        /// Advances the reader to the next message received from the Folding@Home client and stores it in the <see cref="Message"/> property.
        /// </summary>
        /// <exception cref="InvalidOperationException">The connection is not open.</exception>
        /// <returns>true if a message was read; otherwise false.</returns>
        public override bool Read()
        {
            if (!Connection.Connected) throw new InvalidOperationException("The connection is not open.");

            int bytesRead;
            var buffer = GetBuffer();
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
            try
            {
                var stream = GetStream();
                return stream.Read(buffer, 0, buffer.Length);
            }
            catch (Exception)
            {
                Connection.Close();
                throw;
            }
        }

        /// <summary>
        /// Asynchronously advances the reader to the next message received from the Folding@Home client and stores it in the <see cref="Message"/> property.
        /// </summary>
        /// <exception cref="InvalidOperationException">The connection is not open.</exception>
        /// <returns>A task representing the asynchronous operation that can return true if a message was read; otherwise false.</returns>
        public override async Task<bool> ReadAsync()
        {
            if (!Connection.Connected) throw new InvalidOperationException("The connection is not open.");

            int bytesRead;
            var buffer = GetBuffer();
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
            try
            {
                var stream = GetStream();
                return await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            }
            catch (Exception)
            {
                Connection.Close();
                throw;
            }
        }

        private byte[] _buffer;

        private byte[] GetBuffer()
        {
            if (_buffer is null || _buffer.Length != BufferSize)
            {
                return _buffer = new byte[BufferSize];
            }
            return _buffer;
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

        private FahClientMessageExtractor _messageExtractor;
        /// <summary>
        /// Gets or sets the message extractor responsible for extracting messages from the data received from the Folding@Home client.  The default implementation extracts messages as JSON text.
        /// </summary>
        public FahClientMessageExtractor MessageExtractor
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

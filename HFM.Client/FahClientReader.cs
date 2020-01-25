
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HFM.Client
{
    /// <summary>
    /// Folding@Home client message reader.
    /// </summary>
    public class FahClientReader
    {
        /// <summary>
        /// Gets the <see cref="FahClientConnection"/> used by this <see cref="FahClientReader"/>.
        /// </summary>
        public FahClientConnection Connection { get; }

        /// <summary>
        /// Gets the <see cref="FahClientMessageExtractor"/> used by this <see cref="FahClientReader"/>.
        /// </summary>
        public FahClientMessageExtractor MessageExtractor { get; }

        /// <summary>
        /// Gets or sets the size of the read buffer, in bytes. The default value is 1024 bytes.
        /// </summary>
        public int BufferSize { get; set; } = 1024;

        /// <summary>
        /// Gets the last message read by the reader.
        /// </summary>
        public FahClientMessage Message { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientReader"/> class.
        /// </summary>
        /// <param name="connection">A <see cref="FahClientConnection"/> that represents the connection to a Folding@Home client.</param>
        public FahClientReader(FahClientConnection connection)
            : this(connection, FahClientMessageExtractor.Default)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientReader"/> class.
        /// </summary>
        /// <param name="connection">A <see cref="FahClientConnection"/> that represents the connection to a Folding@Home client.</param>
        /// <param name="extractor">A <see cref="FahClientMessageExtractor"/> that extracts <see cref="FahClientMessage"/> objects from the data read by this reader.</param>
        public FahClientReader(FahClientConnection connection, FahClientMessageExtractor extractor)
        {
            Connection = connection;
            MessageExtractor = extractor;
        }

        /// <summary>
        /// Advances the reader to the next message received from the Folding@Home client and stores it in the <see cref="Message"/> property.
        /// </summary>
        /// <exception cref="InvalidOperationException">The connection is not open.</exception>
        /// <returns>true if a message was read; otherwise false.</returns>
        public virtual bool Read()
        {
            if (!Connection.Connected) throw new InvalidOperationException("The connection is not open.");

            int bytesRead;
            var buffer = GetBuffer();
            while ((bytesRead = ReadInternal(buffer)) != 0)
            {
                if (ExtractMessage(buffer, bytesRead))
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
        public virtual async Task<bool> ReadAsync()
        {
            if (!Connection.Connected) throw new InvalidOperationException("The connection is not open.");

            int bytesRead;
            var buffer = GetBuffer();
            while ((bytesRead = await ReadAsyncInternal(buffer).ConfigureAwait(false)) != 0)
            {
                if (ExtractMessage(buffer, bytesRead))
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

        private bool ExtractMessage(byte[] buffer, int bytesRead)
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
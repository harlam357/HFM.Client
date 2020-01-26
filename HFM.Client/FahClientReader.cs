
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

            try
            {
                int bytesRead;
                var stream = GetStream();
                var buffer = GetBuffer();

                bool result = false;
                while ((bytesRead = OnReadStream(stream, buffer, 0, buffer.Length)) != 0)
                {
                    if (!ExtractMessage(buffer, bytesRead)) continue;
                    result = true;
                    break;
                }
                return result;
            }
            catch (Exception)
            {
                Connection.Close();
                throw;
            }
        }

        /// <summary>
        /// Reads a sequence of bytes from the given stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="stream">The <see cref="Stream" /> used to receive data.</param>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current stream.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available.</returns>
        protected virtual int OnReadStream(Stream stream, byte[] buffer, int offset, int count)
        {
            return stream.Read(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Asynchronously advances the reader to the next message received from the Folding@Home client and stores it in the <see cref="Message"/> property.
        /// </summary>
        /// <exception cref="InvalidOperationException">The connection is not open.</exception>
        /// <returns>A task representing the asynchronous operation that can return true if a message was read; otherwise false.</returns>
        public virtual async Task<bool> ReadAsync()
        {
            if (!Connection.Connected) throw new InvalidOperationException("The connection is not open.");

            try
            {
                int bytesRead;
                var stream = GetStream();
                var buffer = GetBuffer();

                bool result = false;
                while ((bytesRead = await OnReadStreamAsync(stream, buffer, 0, buffer.Length).ConfigureAwait(false)) != 0)
                {
                    if (!ExtractMessage(buffer, bytesRead)) continue;
                    result = true;
                    break;
                }
                return result;
            }
            catch (Exception)
            {
                Connection.Close();
                throw;
            }
        }

        /// <summary>
        /// Asynchronously reads a sequence of bytes from the given stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="stream">The <see cref="Stream" /> used to receive data.</param>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current stream.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>A task that represents the asynchronous read operation. The value of the TResult parameter contains the total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available.</returns>
        protected virtual async Task<int> OnReadStreamAsync(Stream stream, byte[] buffer, int offset, int count)
        {
            return await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
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

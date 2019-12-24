
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HFM.Client.Internal
{
    // TODO: Clean-up this class

    [Serializable]
    internal class StringBuilderReader : TextReader
    {
        private StringBuilder _s;
        private int _pos;
        private int _length;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringBuilderReader" /> class that reads from the specified string.
        /// </summary>
        /// <param name="s">The <see cref="StringBuilder"/> to which the <see cref="StringReader" /> should be initialized.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="s" /> parameter is <see langword="null" />.</exception>
        public StringBuilderReader(StringBuilder s)
        {
            _s = s ?? throw new ArgumentNullException(nameof(s));
            _length = s.Length;
        }

        /// <summary>
        /// Closes the <see cref="StringBuilderReader" />.
        /// </summary>
        public override void Close()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="StringBuilderReader" /> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _s = null;
            _pos = 0;
            _length = 0;
            base.Dispose(disposing);
        }

        /// <summary>Returns the next available character but does not consume it.</summary>
        /// <returns>An integer representing the next character to be read, or -1 if no more characters are available or the stream does not support seeking.</returns>
        /// <exception cref="ObjectDisposedException">The current reader is closed.</exception>
        public override int Peek()
        {
            if (_s == null) throw new ObjectDisposedException(ToString());
            return _pos == _length ? -1 : _s[_pos];
        }

        /// <summary>Reads the next character from the input string and advances the character position by one character.</summary>
        /// <returns>The next character from the underlying string, or -1 if no more characters are available.</returns>
        /// <exception cref="ObjectDisposedException">The current reader is closed.</exception>
        public override int Read()
        {
            if (_s == null) throw new ObjectDisposedException(ToString());
            return _pos == _length ? -1 : _s[_pos++];
        }

        /// <summary>Reads a block of characters from the input string and advances the character position by <paramref name="count" />.</summary>
        /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> - 1) replaced by the characters read from the current source.</param>
        /// <param name="index">The starting index in the buffer.</param>
        /// <param name="count">The number of characters to read.</param>
        /// <returns>The total number of characters read into the buffer. This can be less than the number of characters requested if that many characters are not currently available, or zero if the end of the underlying string has been reached.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
        /// <exception cref="ObjectDisposedException">The current reader is closed.</exception>
        public override int Read(char[] buffer, int index, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (buffer.Length - index < count)
                throw new ArgumentException();
            if (_s == null) throw new ObjectDisposedException(ToString());
            int count1 = _length - _pos;
            if (count1 > 0)
            {
                if (count1 > count)
                    count1 = count;
                _s.CopyTo(_pos, buffer, index, count1);
                _pos += count1;
            }
            return count1;
        }

        /// <summary>Reads all characters from the current position to the end of the string and returns them as a single string.</summary>
        /// <returns>The content from the current position to the end of the underlying string.</returns>
        /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
        /// <exception cref="ObjectDisposedException">The current reader is closed.</exception>
        public override string ReadToEnd()
        {
            if (_s == null) throw new ObjectDisposedException(ToString());
            string str = _pos != 0 ? _s.ToString(_pos, _length - _pos) : _s.ToString();
            _pos = _length;
            return str;
        }

        /// <summary>Reads a line of characters from the current string and returns the data as a string.</summary>
        /// <returns>The next line from the current string, or <see langword="null" /> if the end of the string is reached.</returns>
        /// <exception cref="ObjectDisposedException">The current reader is closed.</exception>
        /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
        public override string ReadLine()
        {
            if (_s == null) throw new ObjectDisposedException(ToString());
            int pos;
            for (pos = _pos; pos < _length; ++pos)
            {
                char ch = _s[pos];
                switch (ch)
                {
                    case '\n':
                    case '\r':
                        string str = _s.Substring(_pos, pos - _pos);
                        _pos = pos + 1;
                        if (ch == '\r' && _pos < _length && _s[_pos] == '\n')
                            ++_pos;
                        return str;
                    default:
                        continue;
                }
            }
            if (pos <= _pos)
                return null;
            string str1 = _s.Substring(_pos, pos - _pos);
            _pos = pos;
            return str1;
        }

        /// <summary>Reads a line of characters asynchronously from the current string and returns the data as a string.</summary>
        /// <returns>A task that represents the asynchronous read operation. The value of the TResult parameter contains the next line from the string reader, or is <see langword="null" /> if all the characters have been read.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The number of characters in the next line is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
        /// <exception cref="ObjectDisposedException">The string reader has been disposed.</exception>
        /// <exception cref="InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
        public override Task<string> ReadLineAsync()
        {
            return Task.FromResult(ReadLine());
        }

        /// <summary>Reads all characters from the current position to the end of the string asynchronously and returns them as a single string.</summary>
        /// <returns>A task that represents the asynchronous read operation. The value of the TResult parameter contains a string with the characters from the current position to the end of the string.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The number of characters is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
        /// <exception cref="ObjectDisposedException">The string reader has been disposed.</exception>
        /// <exception cref="InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
        public override Task<string> ReadToEndAsync()
        {
            return Task.FromResult(ReadToEnd());
        }

        /// <summary>Reads a specified maximum number of characters from the current string asynchronously and writes the data to a buffer, beginning at the specified index.</summary>
        /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> - 1) replaced by the characters read from the current source.</param>
        /// <param name="index">The position in <paramref name="buffer" /> at which to begin writing.</param>
        /// <param name="count">The maximum number of characters to read. If the end of the string is reached before the specified number of characters is written into the buffer, the method returns.</param>
        /// <returns>A task that represents the asynchronous read operation. The value of the TResult parameter contains the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the string has been reached.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
        /// <exception cref="ArgumentException">The sum of <paramref name="index" /> and <paramref name="count" /> is larger than the buffer length.</exception>
        /// <exception cref="ObjectDisposedException">The string reader has been disposed.</exception>
        /// <exception cref="InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
        public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException(index < 0 ? nameof(index) : nameof(count));
            if (buffer.Length - index < count)
                throw new ArgumentException();
            return Task.FromResult(ReadBlock(buffer, index, count));
        }

        /// <summary>Reads a specified maximum number of characters from the current string asynchronously and writes the data to a buffer, beginning at the specified index.</summary>
        /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> - 1) replaced by the characters read from the current source.</param>
        /// <param name="index">The position in <paramref name="buffer" /> at which to begin writing.</param>
        /// <param name="count">The maximum number of characters to read. If the end of the string is reached before the specified number of characters is written into the buffer, the method returns.</param>
        /// <returns>A task that represents the asynchronous read operation. The value of the TResult parameter contains the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the string has been reached.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
        /// <exception cref="ArgumentException">The sum of <paramref name="index" /> and <paramref name="count" /> is larger than the buffer length.</exception>
        /// <exception cref="ObjectDisposedException">The string reader has been disposed.</exception>
        /// <exception cref="InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
        public override Task<int> ReadAsync(char[] buffer, int index, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException(index < 0 ? nameof(index) : nameof(count));
            if (buffer.Length - index < count)
                throw new ArgumentException();
            return Task.FromResult(Read(buffer, index, count));
        }
    }
}

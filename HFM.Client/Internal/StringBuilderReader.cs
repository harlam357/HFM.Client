
using System;
using System.IO;
using System.Text;

namespace HFM.Client.Internal
{
    // Shim to provide JsonTextReader a TextReader implementation that reads content from a StringBuilder.
    [Serializable]
    internal class StringBuilderReader : TextReader
    {
        private StringBuilder _s;
        private int _pos;
        private int _length;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringBuilderReader" /> class that reads from the specified <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="s">The <see cref="StringBuilder"/> to which the <see cref="StringBuilderReader" /> should be initialized.</param>
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

        /// <summary>
        /// Reads a block of characters from the input string and advances the character position by <paramref name="count" />.
        /// </summary>
        /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> - 1) replaced by the characters read from the current source.</param>
        /// <param name="index">The starting index in the buffer.</param>
        /// <param name="count">The number of characters to read.</param>
        /// <exception cref="ArgumentNullException"><paramref name="buffer" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> or <paramref name="count" /> is negative.</exception>
        /// <exception cref="ObjectDisposedException">The current reader is closed.</exception>
        /// <returns>The total number of characters read into the buffer. This can be less than the number of characters requested if that many characters are not currently available, or zero if the end of the underlying string has been reached.</returns>
        public override int Read(char[] buffer, int index, int count)
        {
            if (buffer is null) throw new ArgumentNullException(nameof(buffer));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (buffer.Length - index < count) throw new ArgumentException("The buffer length minus index is less than count.");
            if (_s is null) throw new ObjectDisposedException(nameof(StringBuilderReader));

            int count1 = _length - _pos;
            if (count1 > 0)
            {
                if (count1 > count)
                {
                    count1 = count;
                }
                _s.CopyTo(_pos, buffer, index, count1);
                _pos += count1;
            }
            return count1;
        }
    }
}

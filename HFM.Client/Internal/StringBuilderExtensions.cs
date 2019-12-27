
using System;
using System.Text;

namespace HFM.Client.Internal
{
    internal static class StringBuilderExtensions
    {
        /// <summary>
        /// Reports the index of the first occurrence of the specified string in the current <see cref="StringBuilder"/> object. Parameters specify the starting search position in the current <see cref="StringBuilder"/> and the type of search to use for the specified string.
        /// </summary>
        /// <param name="source">The <see cref="StringBuilder"/> to search.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is negative. -or- startIndex specifies a position not within this instance.</exception>
        /// <returns>The zero-based index position of the value parameter if that string is found, or -1 if it is not. If value is System.String.Empty, the return value is startIndex.</returns>
        internal static int IndexOf(this StringBuilder source, string value, int startIndex)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (startIndex < 0 || startIndex > source.Length) throw new ArgumentOutOfRangeException(nameof(startIndex));

            if (value.Length == 0)
            {
                return startIndex;
            }

            int searchLength = source.Length - value.Length + 1;
            for (int i = startIndex; i < searchLength; ++i)
            {
                if (source[i] == value[0])
                {
                    int index = 1;
                    while (index < value.Length && source[i + index] == value[index])
                    {
                        ++index;
                    }

                    if (index == value.Length)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of characters specified in an array from the source instance and returns the same source instance.
        /// </summary>
        /// <param name="source">The <see cref="StringBuilder"/> source instance.</param>
        /// <param name="trimChars">An array of Unicode characters to remove.</param>
        /// <exception cref="ArgumentNullException">source -or- trimChars is null.</exception>
        /// <returns>The same source instance with the string content that remains after all occurrences of the characters in the trimChars parameter are removed from the start and end of the source instance.</returns>
        internal static StringBuilder Trim(this StringBuilder source, params char[] trimChars)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (trimChars == null) throw new ArgumentNullException(nameof(trimChars));
            
            TrimHelper(source, trimChars, 2);
            return source;
        }

        // modeled after System.String.TrimHelper
        private static void TrimHelper(StringBuilder source, char[] trimChars, int trimType)
        {
            int end = source.Length - 1;
            int start = 0;
            if (trimType != 1)
            {
                for (start = 0; start < source.Length; ++start)
                {
                    char ch = source[start];
                    int index = 0;
                    while (index < trimChars.Length && trimChars[index] != ch)
                    {
                        ++index;
                    }

                    if (index == trimChars.Length)
                    {
                        break;
                    }
                }
            }
            if (trimType != 0)
            {
                for (end = source.Length - 1; end >= start; --end)
                {
                    char ch = source[end];
                    int index = 0;
                    while (index < trimChars.Length && trimChars[index] != ch)
                    {
                        ++index;
                    }

                    if (index == trimChars.Length)
                    {
                        break;
                    }
                }
            }

            if (end < source.Length - 1)
            {
                source.Remove(end + 1, source.Length - (end + 1));
            }
            if (start > 0)
            {
                source.Remove(0, start);
            }
        }

        /// <summary>
        /// Copies the characters from a specified segment of the source instance to a specified segment of a destination <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="source">The <see cref="StringBuilder"/> source instance.</param>
        /// <param name="sourceIndex">The starting position in this instance where characters will be copied from. The index is zero-based.</param>
        /// <param name="destination">The <see cref="StringBuilder"/> where characters will be copied.</param>
        /// <param name="destinationIndex">The starting position in destination where characters will be copied. The index is zero-based.</param>
        /// <param name="count">The number of characters to be copied.</param>
        /// <exception cref="ArgumentNullException">source -or- destination is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">sourceIndex, destinationIndex, or count, is less than zero. -or- sourceIndex is greater than the length of this instance.</exception>
        /// <exception cref="ArgumentException">sourceIndex + count is greater than the length of this instance.</exception>
        internal static void CopyTo(this StringBuilder source, int sourceIndex, StringBuilder destination, int destinationIndex, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (sourceIndex < 0 || sourceIndex > source.Length) throw new ArgumentOutOfRangeException(nameof(sourceIndex));
            if (destinationIndex < 0) throw new ArgumentOutOfRangeException(nameof(destinationIndex));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (sourceIndex + count > source.Length) throw new ArgumentException($"{nameof(sourceIndex)} + {nameof(count)} is greater than the length of {nameof(source)}.");

            destination.EnsureCapacity(destinationIndex + count);
            while (destinationIndex > destination.Length)
            {
                destination.Append(' ');
            }

            int i = sourceIndex;
            int j = destinationIndex;
            while (i < sourceIndex + count)
            {
                if (j < destination.Length)
                {
                    destination[j] = source[i];
                }
                else
                {
                    destination.Append(source[i]);
                }

                i++;
                j++;
            }
        }
    }
}

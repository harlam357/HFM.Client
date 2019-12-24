
using System;
using System.Text;

namespace HFM.Client.Internal
{
    internal static class StringBuilderExtensions
    {
        /// <summary>
        /// Reports the index of the first occurrence of the specified string in the current System.String object. A parameter specifies the type of search to use for the specified string.
        /// </summary>
        /// <param name="sb">The System.Text.StringBuilder to search.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="ignoreCase">Ignore character case.</param>
        /// <returns>The index position of the value parameter if that string is found, or -1 if it is not. If value is System.String.Empty, the return value is 0.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        internal static int IndexOf(this StringBuilder sb, string value, bool ignoreCase)
        {
            return IndexOf(sb, value, 0, ignoreCase);
        }

        /// <summary>
        /// Reports the end index of the first occurrence of the specified string in the current System.String object. A parameter specifies the type of search to use for the specified string.
        /// </summary>
        /// <param name="sb">The System.Text.StringBuilder to search.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="ignoreCase">Ignore character case.</param>
        /// <returns>The index position of the value parameter if that string is found, or -1 if it is not. If value is System.String.Empty, the return value is 0.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        internal static int EndIndexOf(this StringBuilder sb, string value, bool ignoreCase)
        {
            return IndexOf(sb, value, 0, ignoreCase) + value.Length;
        }

        /// <summary>
        /// Reports the index of the first occurrence of the specified string in the current System.String object. Parameters specify the starting search position in the current string and the type of search to use for the specified string.
        /// </summary>
        /// <param name="sb">The System.Text.StringBuilder to search.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="ignoreCase">Ignore character case.</param>
        /// <returns>The zero-based index position of the value parameter if that string is found, or -1 if it is not. If value is System.String.Empty, the return value is startIndex.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">startIndex is negative. -or- startIndex specifies a position not within this instance.</exception>
        internal static int IndexOf(this StringBuilder sb, string value, int startIndex, bool ignoreCase)
        {
            if (sb == null) throw new ArgumentNullException(nameof(sb));
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (startIndex < 0 || startIndex > sb.Length) throw new ArgumentOutOfRangeException(nameof(startIndex));

            if (value.Length == 0)
            {
                return startIndex;
            }

            int index;
            int length = value.Length;
            int maxSearchLength = (sb.Length - length) + 1;

            if (ignoreCase)
            {
                for (int i = startIndex; i < maxSearchLength; ++i)
                {
                    if (Char.ToLower(sb[i]) == Char.ToLower(value[0]))
                    {
                        index = 1;
                        while ((index < length) && (Char.ToLower(sb[i + index]) == Char.ToLower(value[index])))
                        {
                            ++index;
                        }

                        if (index == length)
                        {
                            return i;
                        }
                    }
                }

                return -1;
            }

            for (int i = startIndex; i < maxSearchLength; ++i)
            {
                if (sb[i] == value[0])
                {
                    index = 1;
                    while ((index < length) && (sb[i + index] == value[index]))
                    {
                        ++index;
                    }

                    if (index == length)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position and has a specified length.
        /// </summary>
        /// <param name="sb">The System.Text.StringBuilder source instance.</param>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A string that is equivalent to the substring of length length that begins at startIndex in this instance, or System.String.Empty if startIndex is equal to the length of this instance and length is zero.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">startIndex plus length indicates a position not within this instance. -or- startIndex or length is less than zero.</exception>
        internal static string Substring(this StringBuilder sb, int startIndex, int length)
        {
            if (sb == null) throw new ArgumentNullException(nameof(sb));
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (startIndex + length > sb.Length) throw new ArgumentOutOfRangeException(nameof(length));

            var temp = new char[length];
            sb.CopyTo(startIndex, temp, 0, length);
            return new string(temp);
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position and has a specified length.
        /// </summary>
        /// <param name="sb">The System.Text.StringBuilder source instance.</param>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <param name="dest">The System.Text.StringBuilder destination instance. If null a new StringBuilder will be returned.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A System.Text.StringBuilder that is equivalent to the substring of length length that begins at startIndex in this instance.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">startIndex plus length indicates a position not within this instance. -or- startIndex or length is less than zero.</exception>
        internal static StringBuilder SubstringBuilder(this StringBuilder sb, int startIndex, StringBuilder dest, int length)
        {
            return SubstringBuilder(sb, startIndex, dest, length, false);
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position and has a specified length.
        /// </summary>
        /// <param name="sb">The System.Text.StringBuilder source instance.</param>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <param name="trimOnly">if true trim the existing StringBuilder instance. -or- if false create a new StringBuilder instance and leave the existing instance in tact.</param>
        /// <returns>A System.Text.StringBuilder that is equivalent to the substring of length length that begins at startIndex in this instance.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">startIndex plus length indicates a position not within this instance. -or- startIndex or length is less than zero.</exception>
        internal static StringBuilder SubstringBuilder(this StringBuilder sb, int startIndex, int length, bool trimOnly)
        {
            return SubstringBuilder(sb, startIndex, null, length, trimOnly);
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position and has a specified length.
        /// </summary>
        /// <param name="sb">The System.Text.StringBuilder source instance.</param>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <param name="dest">The System.Text.StringBuilder destination instance. If null a new StringBuilder will be returned.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <param name="trimOnly">if true trim the existing StringBuilder instance. -or- if false create a new StringBuilder instance and leave the existing instance in tact.</param>
        /// <returns>A System.Text.StringBuilder that is equivalent to the substring of length length that begins at startIndex in this instance.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">startIndex plus length indicates a position not within this instance. -or- startIndex or length is less than zero.</exception>
        private static StringBuilder SubstringBuilder(this StringBuilder sb, int startIndex, StringBuilder dest, int length, bool trimOnly)
        {
            if (sb == null) throw new ArgumentNullException(nameof(sb));
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (startIndex + length > sb.Length) throw new ArgumentOutOfRangeException(nameof(length));

            if (trimOnly)
            {
                sb.Remove(0, startIndex);
                sb.Remove(length, sb.Length - length);
                return sb;
            }

            var result = dest ?? new StringBuilder();
            sb.CopyTo(startIndex, result, length);
            return result;
        }

        internal static void CopyTo(this StringBuilder sb, StringBuilder dest)
        {
            CopyTo(sb, 0, dest, sb.Length);
        }

        private static void CopyTo(this StringBuilder sb, int startIndex, StringBuilder dest, int length)
        {
            if (sb == null) throw new ArgumentNullException(nameof(sb));
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (dest == null) throw new ArgumentNullException(nameof(dest));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (startIndex + length > sb.Length) throw new ArgumentOutOfRangeException(nameof(length));

            dest.Clear();
            dest.EnsureCapacity(length);
            for (int i = startIndex; i < (startIndex + length); i++)
            {
                dest.Append(sb[i]);
            }
        }

        internal static bool EndsWith(this StringBuilder sb, char value)
        {
            if (sb == null) throw new ArgumentNullException(nameof(sb));

            return sb.Length > 0 && sb[sb.Length - 1].Equals(value);
        }
    }
}

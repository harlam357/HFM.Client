using System;
using System.Text;

using static HFM.Client.Internal.StringBuilderExtensions;

namespace HFM.Client.ObjectModel.Internal
{
    [Flags]
    internal enum JsonHexDecoderOptions
    {
        None = 0,
        FilterNewLineCharacters = 1 << 0
    }

    internal class JsonHexDecoder : IJsonStringFilter
    {
        private readonly JsonHexDecoderOptions _options;

        internal JsonHexDecoder(JsonHexDecoderOptions options = JsonHexDecoderOptions.None)
        {
            _options = options;
        }

        public StringBuilder Filter(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return new StringBuilder(0);
            }
            if (!s.Contains(@"\"))
            {
                return new StringBuilder(s);
            }
            return Filter(new StringBuilder(s));
        }

        // https://github.com/JamesNK/Newtonsoft.Json/issues/980
        public StringBuilder Filter(StringBuilder s)
        {
            if (s is null || s.Length == 0)
            {
                return s;
            }
            if (s.IndexOf(@"\", 0) < 0)
            {
                return s;
            }

            bool filterNewLineCharacters = _options.HasFlag(JsonHexDecoderOptions.FilterNewLineCharacters);
            var builder = new StringBuilder(s.Length);
            int num = s.Length;
            int num2 = 0;

            while (num2 < num)
            {
                var ch = s[num2];
                if (ch != 0x5c)
                {
                    builder.Append(ch);
                }
                else if (num2 < num - 5 && s[num2 + 1] == 0x75)
                {
                    int num3 = HexToInt(s[num2 + 2]);
                    int num4 = HexToInt(s[num2 + 3]);
                    int num5 = HexToInt(s[num2 + 4]);
                    int num6 = HexToInt(s[num2 + 5]);
                    if (num3 < 0 || (num4 < 0) | (num5 < 0) || num6 < 0)
                    {
                        builder.Append(ch);
                    }
                    else
                    {
                        ch = (char)((num3 << 12) | (num4 << 8) | (num5 << 4) | num6);
                        num2 += 5;
                        builder.Append(ch);
                    }
                }
                else if (num2 < num - 3 && s[num2 + 1] == 0x78)
                {
                    int num7 = HexToInt(s[num2 + 2]);
                    int num8 = HexToInt(s[num2 + 3]);
                    if (num7 < 0 || num8 < 0)
                    {
                        builder.Append(ch);
                    }
                    else
                    {
                        ch = (char)((num7 << 4) | num8);
                        num2 += 3;
                        builder.Append(ch);
                    }
                }
                else
                {
                    if (num2 < num - 1)
                    {
                        var ch2 = s[num2 + 1];
                        if (ch2 == 0x5c)
                        {
                            builder.Append('\\');
                            builder.Append('\\');
                            num2 += 1;
                        }
                        else if (ch2 == 110)
                        {
                            if (!filterNewLineCharacters)
                            {
                                builder.Append('\\');
                                builder.Append('n');
                            }
                            num2 += 1;
                        }
                        else if (ch2 == 114)
                        {
                            if (!filterNewLineCharacters)
                            {
                                builder.Append('\\');
                                builder.Append('r');
                            }
                            num2 += 1;
                        }
                        else if (ch2 == 0x74)
                        {
                            builder.Append('\\');
                            builder.Append('\t');
                            num2 += 1;
                        }
                        else
                        {
                            builder.Append('\\');
                            builder.Append(ch2);
                            num2 += 1;
                        }
                    }
                }
                num2 += 1;
            }
            return builder;
        }

        private static int HexToInt(char h)
        {
            if (h < 0x30 || h > 0x39)
            {
                if (h < 0x61 || h > 0x66)
                {
                    if (h < 0x41 || h > 0x46)
                    {
                        return -1;
                    }
                    return h - 0x41 + 10;
                }
                return h - 0x61 + 10;
            }
            return h - 0x30;
        }
    }
}

using System;
using System.IO;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using HFM.Client.Internal;

namespace HFM.Client.ObjectModel.Internal
{
    internal abstract class ObjectLoader<T> where T : class
    {
        /// <summary>
        /// Creates a new object from a <see cref="string"/> that contains JSON.
        /// </summary>
        public virtual T Load(string json)
        {
            if (json is null) return null;

            using (var textReader = new StringBuilderReader(DecodeJson(json)))
            {
                return Load(textReader);
            }
        }

        /// <summary>
        /// Creates a new object from a <see cref="StringBuilder"/> that contains JSON.
        /// </summary>
        public virtual T Load(StringBuilder json)
        {
            if (json is null) return null;

            using (var textReader = new StringBuilderReader(DecodeJson(json)))
            {
                return Load(textReader);
            }
        }

        /// <summary>
        /// Creates a object from a <see cref="TextReader"/> that contains JSON.
        /// </summary>
        public abstract T Load(TextReader textReader);

        /// <summary>
        /// TODO: LoadJObject documentation
        /// </summary>
        protected virtual JObject LoadJObject(TextReader textReader)
        {
            using (var reader = new JsonTextReader(textReader))
            {
                reader.DateParseHandling = DateParseHandling.None;
                return JObject.Load(reader);
            }
        }

        /// <summary>
        /// TODO: LoadJArray documentation
        /// </summary>
        protected virtual JArray LoadJArray(TextReader textReader)
        {
            using (var reader = new JsonTextReader(textReader))
            {
                reader.DateParseHandling = DateParseHandling.None;
                return JArray.Load(reader);
            }
        }

        /// <summary>
        /// TODO: GetValue documentation
        /// </summary>
        protected virtual TResult GetValue<TResult>(JObject obj, params string[] names)
        {
            foreach (var name in names)
            {
                var token = obj[name];
                if (token != null)
                {
                    try
                    {
                        return token.Value<TResult>();
                    }
                    catch (FormatException)
                    {
                        // if the data changed in a way where the value can
                        // no longer be converted to the target CLR type
                    }
                }
            }
            return default;
        }

        protected static StringBuilder DecodeJson(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return new StringBuilder(0);
            }
            if (!s.Contains(@"\"))
            {
                return new StringBuilder(s);
            }
            return DecodeJson(new StringBuilder(s));
        }

        // https://github.com/JamesNK/Newtonsoft.Json/issues/980
        protected static StringBuilder DecodeJson(StringBuilder s)
        {
            if (s is null || s.Length == 0)
            {
                return s;
            }

            var builder = new StringBuilder();
            int num = s.Length;
            int num2 = 0;

            while (num2 < num)
            {
                var ch = s[num2];
                if (ch != 0x5c)
                {
                    builder.Append(ch);
                }
                else if (num2 < (num - 5) && s[num2 + 1] == 0x75)
                {
                    int num3 = HexToInt(s[num2 + 2]);
                    int num4 = HexToInt(s[num2 + 3]);
                    int num5 = HexToInt(s[num2 + 4]);
                    int num6 = HexToInt(s[num2 + 5]);
                    if (num3 < 0 || num4 < 0 | num5 < 0 || num6 < 0)
                    {
                        builder.Append(ch);
                    }
                    else
                    {
                        ch = (char)((((num3 << 12) | (num4 << 8)) | (num5 << 4)) | num6);
                        num2 += 5;
                        builder.Append(ch);
                    }
                }
                else if (num2 < (num - 3) && s[num2 + 1] == 0x78)
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
                    if (num2 < (num - 1))
                    {
                        var ch2 = s[num2 + 1];
                        if (ch2 == 0x5c)
                        {
                            builder.Append(@"\");
                            num2 += 1;
                        }
                        else if (ch2 == 110)
                        {
                            builder.Append("\n");
                            num2 += 1;
                        }
                        else if (ch2 == 0x74)
                        {
                            builder.Append("\t");
                            num2 += 1;
                        }
                    }
                    builder.Append(ch);
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

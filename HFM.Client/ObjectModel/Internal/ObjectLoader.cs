using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using HFM.Client.Internal;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HFM.Client.ObjectModel.Internal
{
    internal abstract class ObjectLoader<T> where T : class
    {
        /// <summary>
        /// Creates a new object from a <see cref="string"/> that contains JSON.
        /// </summary>
        public virtual T Load(string json, ObjectLoadOptions options = ObjectLoadOptions.Default)
        {
            if (json is null) return null;

            using (var textReader = new StringBuilderReader(FilterJson(json, options)))
            {
                return Load(textReader);
            }
        }

        /// <summary>
        /// Creates a new object from a <see cref="StringBuilder"/> that contains JSON.
        /// </summary>
        public virtual T Load(StringBuilder json, ObjectLoadOptions options = ObjectLoadOptions.Default)
        {
            if (json is null) return null;

            using (var textReader = new StringBuilderReader(FilterJson(json, options)))
            {
                return Load(textReader);
            }
        }

        private static StringBuilder FilterJson(string json, ObjectLoadOptions options)
        {
            StringBuilder sb = null;
            foreach (var f in EnumerateFilters(options))
            {
                sb = sb is null ? f.Filter(json) : f.Filter(sb);
            }
            return sb ?? new StringBuilder(json);
        }

        private static StringBuilder FilterJson(StringBuilder json, ObjectLoadOptions options)
        {
            StringBuilder sb = json;
            foreach (var f in EnumerateFilters(options))
            {
                sb = f.Filter(sb);
            }
            return sb;
        }

        private static IEnumerable<IJsonStringFilter> EnumerateFilters(ObjectLoadOptions options)
        {
            if (options.HasFlag(ObjectLoadOptions.DecodeHex)) yield return new JsonHexDecoder();
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
    }
}

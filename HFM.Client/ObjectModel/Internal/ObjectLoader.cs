using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

using HFM.Client.Internal;

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

        protected StringBuilder FilterJson(string json, ObjectLoadOptions options)
        {
            return FilterJson(json, EnumerateFilters(options));
        }

        protected static StringBuilder FilterJson(string json, IEnumerable<IJsonStringFilter> filters)
        {
            StringBuilder sb = null;
            foreach (var f in filters)
            {
                sb = sb is null ? f.Filter(json) : f.Filter(sb);
            }
            return sb ?? new StringBuilder(json);
        }

        protected StringBuilder FilterJson(StringBuilder json, ObjectLoadOptions options)
        {
            return FilterJson(json, EnumerateFilters(options));
        }

        protected static StringBuilder FilterJson(StringBuilder json, IEnumerable<IJsonStringFilter> filters)
        {
            StringBuilder sb = json;
            foreach (var f in filters)
            {
                sb = f.Filter(sb);
            }
            return sb;
        }

        protected virtual IEnumerable<IJsonStringFilter> EnumerateFilters(ObjectLoadOptions options)
        {
            if (options.HasFlag(ObjectLoadOptions.DecodeHex)) yield return new JsonHexDecoder(JsonHexDecoderOptions.FilterNewLineCharacters);
            if (options.HasFlag(ObjectLoadOptions.FilterNewLineCharacters)) yield return new JsonNewLineFilter();
        }

        /// <summary>
        /// Creates a object from a <see cref="TextReader"/> that contains JSON.
        /// </summary>
        public abstract T Load(TextReader textReader);

        /// <summary>
        /// TODO: LoadJsonObject documentation
        /// </summary>
        protected virtual JsonObject LoadJsonObject(TextReader textReader)
        {
            var memory = Read(textReader);
            var jsonDocument = JsonDocument.Parse(memory);
            return JsonObject.Create(jsonDocument.RootElement);
        }

        /// <summary>
        /// TODO: LoadJsonArray documentation
        /// </summary>
        protected virtual JsonArray LoadJsonArray(TextReader textReader)
        {
            var memory = Read(textReader);
            var jsonDocument = JsonDocument.Parse(memory);
            return JsonArray.Create(jsonDocument.RootElement);
        }

        private static ReadOnlyMemory<char> Read(TextReader textReader)
        {
            var buffer = new char[4096];
            int offset = 0;

            int bytesRead;
            while ((bytesRead = textReader.ReadBlock(buffer, offset, buffer.Length - offset)) > 0)
            {
                offset += bytesRead;

                if (offset == buffer.Length)
                {
                    Array.Resize(ref buffer, buffer.Length * 2);
                }
            }

            return buffer.AsMemory(0, offset);
        }

        /// <summary>
        /// TODO: GetValue documentation
        /// </summary>
        protected virtual TResult GetValue<TResult>(JsonObject obj, params string[] names)
        {
            foreach (var name in names)
            {
                var node = obj[name];
                if (node != null)
                {
                    try
                    {
                        return node.GetValue<TResult>();
                    }
                    catch (FormatException)
                    {
                        // The current JsonNode cannot be represented as a {T}.
                    }
                    catch (InvalidOperationException)
                    {
                        // The current JsonNode is not a JsonValue or is not compatible with {T}.
                    }
                }
            }
            return default;
        }
    }
}

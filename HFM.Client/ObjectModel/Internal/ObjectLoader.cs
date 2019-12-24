
using System;
using System.IO;
using System.Text;

using HFM.Client.Internal;

using Newtonsoft.Json.Linq;

namespace HFM.Client.ObjectModel.Internal
{
    internal abstract class ObjectLoader<T>
    {
        /// <summary>
        /// Creates a new object from a <see cref="string"/> that contains JSON.
        /// </summary>
        public virtual T Load(string json)
        {
            using (var textReader = new StringReader(json))
            {
                return Load(textReader);
            }
        }

        /// <summary>
        /// Creates a new object from a <see cref="StringBuilder"/> that contains JSON.
        /// </summary>
        public virtual T Load(StringBuilder json)
        {
            using (var textReader = new StringBuilderReader(json))
            {
                return Load(textReader);
            }
        }

        /// <summary>
        /// Creates a object from a <see cref="TextReader"/> that contains JSON.
        /// </summary>
        public abstract T Load(TextReader textReader);

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
            return default(TResult);
        }
    }
}

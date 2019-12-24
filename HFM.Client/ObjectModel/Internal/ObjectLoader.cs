using System.IO;
using System.Text;

using HFM.Client.Internal;

namespace HFM.Client.ObjectModel.Internal
{
    internal abstract class ObjectLoader<T>
    {
        /// <summary>
        /// Creates a new <see cref="Info"/> object from a <see cref="string"/> that contains JSON.
        /// </summary>
        public virtual T Load(string json)
        {
            using (var textReader = new StringReader(json))
            {
                return Load(textReader);
            }
        }

        /// <summary>
        /// Creates a new <see cref="Info"/> object from a <see cref="StringBuilder"/> that contains JSON.
        /// </summary>
        public virtual T Load(StringBuilder json)
        {
            using (var textReader = new StringBuilderReader(json))
            {
                return Load(textReader);
            }
        }

        /// <summary>
        /// Creates a new <see cref="Info"/> object from a <see cref="TextReader"/> that contains JSON.
        /// </summary>
        public abstract T Load(TextReader textReader);
    }
}
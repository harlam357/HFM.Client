
using System.Threading.Tasks;

namespace HFM.Client
{
    /// <summary>
    /// Provides the abstract base class for a Folding@Home client reader.
    /// </summary>
    public abstract class FahClientReader
    {
        /// <summary>
        /// Advances the reader to the next data received from the Folding@Home client.
        /// </summary>
        /// <returns>true if data was read; otherwise false.</returns>
        public abstract bool Read();

        /// <summary>
        /// Asynchronously advances the reader to the next data received from the Folding@Home client.
        /// </summary>
        /// <returns>A task representing the asynchronous operation that can return true if data was read; otherwise false.</returns>
        public abstract Task<bool> ReadAsync();
    }
}
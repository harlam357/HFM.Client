
using System.Text;

namespace HFM.Client
{
    /// <summary>
    /// Provides the abstract base class for a Folding@Home client message extraction.
    /// </summary>
    public abstract class FahClientMessageExtractor
    {
        /// <summary>
        /// Extracts a new <see cref="FahClientMessage"/> from the <paramref name="buffer"/> if a message is available.
        /// </summary>
        /// <returns>A new <see cref="FahClientMessage"/> or null if a message is not available.</returns>
        public abstract FahClientMessage Extract(StringBuilder buffer);
    }
}
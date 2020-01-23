
using System.Threading.Tasks;

namespace HFM.Client
{
    /// <summary>
    /// Provides the abstract base class for a Folding@Home client command statement.
    /// </summary>
    public abstract class FahClientCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientCommand"/> class.
        /// </summary>
        protected FahClientCommand()
        {

        }

        /// <summary>
        /// Sends the command to the Folding@Home client.
        /// </summary>
        /// <returns>The number of bytes transmitted to the Folding@Home client.</returns>
        public abstract int Execute();

        /// <summary>
        /// Asynchronously sends the command to the Folding@Home client.
        /// </summary>
        /// <returns>A task representing the asynchronous operation that can return the number of bytes transmitted to the Folding@Home client.</returns>
        public abstract Task<int> ExecuteAsync();
    }
}
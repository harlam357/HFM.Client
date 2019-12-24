﻿
namespace HFM.Client.ObjectModel
{
    /// <summary>
    /// Represents the status of a Folding@Home client slot.
    /// </summary>
    public enum SlotStatus
    {
        /// <summary>
        /// The status of the slot is unknown.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The slot is paused.
        /// </summary>
        Paused = 1,
        /// <summary>
        /// The slot is running.
        /// </summary>
        Running = 2,
        /// <summary>
        /// The slot is finishing.
        /// </summary>
        Finishing = 3,
        /// <summary>
        /// The slot is ready for work.
        /// </summary>
        Ready = 4,
        /// <summary>
        /// The slot is stopping.
        /// </summary>
        Stopping = 5,
        /// <summary>
        /// The slot work has failed.
        /// </summary>
        Failed = 6
    }
}

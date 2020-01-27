
using System;

namespace HFM.Client
{
    /// <summary>
    /// Uniquely identifies a Folding@Home client message by the message type and date received.
    /// </summary>
    public struct FahClientMessageIdentifier : IEquatable<FahClientMessageIdentifier>
    {
        /// <summary>
        /// Gets the message type.
        /// </summary>
        public string MessageType { get; }

        /// <summary>
        /// Gets the date and time the message was received.
        /// </summary>
        public DateTime Received { get; }

        /// <summary>
        /// Initializes a new <see cref="FahClientMessageIdentifier"/>.
        /// </summary>
        /// <param name="messageType">The message type string.</param>
        /// <param name="received">The date and time the message was received.</param>
        public FahClientMessageIdentifier(string messageType, DateTime received)
        {
            MessageType = messageType;
            Received = received;
        }

        /// <summary>
        /// Indicates whether the current <see cref="FahClientMessageIdentifier"/> is equal to another <see cref="FahClientMessageIdentifier"/>.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current <see cref="FahClientMessageIdentifier"/> is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(FahClientMessageIdentifier other)
        {
            return String.Equals(MessageType, other.MessageType, StringComparison.Ordinal) && Received.Equals(other.Received);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is FahClientMessageIdentifier other && Equals(other);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((MessageType != null ? MessageType.GetHashCode() : 0) * 397) ^ Received.GetHashCode();
            }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="FahClientMessageIdentifier"/>.
        /// </summary>
        public override string ToString()
        {
            return $"Message Type: {MessageType} - Received at: {Received}";
        }
        
#pragma warning disable 1591
        public static bool operator ==(FahClientMessageIdentifier left, FahClientMessageIdentifier right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(FahClientMessageIdentifier left, FahClientMessageIdentifier right)
        {
            return !(left == right);
        }
#pragma warning restore 1591
    }
}


using System;
using System.Globalization;
using System.Text;

namespace HFM.Client
{
   /// <summary>
   /// Folding@Home client message type.
   /// </summary>
   public static class FahClientMessageType
   {
      public const string Heartbeat = "heartbeat";
      public const string Info = "info";
      public const string Options = "options";
      public const string SimulationInfo = "simulation-info";
      public const string SlotInfo = "slots";
      public const string SlotOptions = "slot-options";
      public const string QueueInfo = "units";
      public const string LogRestart = "log-restart";
      public const string LogUpdate = "log-update";
   }

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
         return String.Equals(MessageType, other.MessageType) && Received.Equals(other.Received);
      }

      /// <summary>
      /// Indicates whether this instance and a specified object are equal.
      /// </summary>
      /// <param name="obj">The object to compare with the current instance.</param>
      /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
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
   }

   /// <summary>
   /// Folding@Home client message.
   /// </summary>
   public class FahClientMessage
   {
      /// <summary>
      /// Gets the message identifier.
      /// </summary>
      public FahClientMessageIdentifier Identifier { get; }

      /// <summary>
      /// Gets the text value of the message.
      /// </summary>
      public string MessageText { get; }

      /// <summary>
      /// Initializes a new instance of the <see cref="FahClientMessage"/> class.
      /// </summary>
      /// <param name="identifier">The message identifier.</param>
      /// <param name="messageText">The text value of the message.</param>
      public FahClientMessage(FahClientMessageIdentifier identifier, string messageText)
      {
         Identifier = identifier;
         MessageText = messageText;
      }

      /// <summary>
      /// Returns a <see cref="String"/> that represents the current <see cref="FahClientMessage"/>.
      /// </summary>
      public override string ToString()
      {
         var sb = new StringBuilder();
         sb.AppendLine($"{Identifier} - Length: {MessageText.Length}");
         sb.AppendLine(MessageText);
         sb.AppendLine();
         return sb.ToString();
      }
   }
}

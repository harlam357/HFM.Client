
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

      public FahClientMessageIdentifier(string messageType, DateTime received)
      {
         MessageType = messageType;
         Received = received;
      }

      public bool Equals(FahClientMessageIdentifier other)
      {
         return String.Equals(MessageType, other.MessageType) && Received.Equals(other.Received);
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         return obj is FahClientMessageIdentifier other && Equals(other);
      }

      public override int GetHashCode()
      {
         unchecked
         {
            return ((MessageType != null ? MessageType.GetHashCode() : 0) * 397) ^ Received.GetHashCode();
         }
      }

      /// <summary>
      /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
      /// </returns>
      /// <filterpriority>2</filterpriority>
      public override string ToString()
      {
         return String.Format(CultureInfo.CurrentCulture, "Message Type: {0} - Received at: {1}", MessageType, Received);
      }
   }

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

      public FahClientMessage(FahClientMessageIdentifier identifier, string messageText)
      {
         Identifier = identifier;
         MessageText = messageText;
      }

      /// <summary>
      /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
      /// </returns>
      /// <filterpriority>2</filterpriority>
      public override string ToString()
      {
         var sb = new StringBuilder();
         sb.AppendLine(String.Format(CultureInfo.CurrentCulture, "{0} - Length: {1}", Identifier, MessageText.Length));
         sb.AppendLine(MessageText);
         sb.AppendLine();
         return sb.ToString();
      }
   }
}


using System;
using System.Text;

namespace HFM.Client
{
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
        public StringBuilder MessageText { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientMessage"/> class.
        /// </summary>
        /// <param name="identifier">The message identifier.</param>
        /// <param name="messageText">The text value of the message.</param>
        public FahClientMessage(FahClientMessageIdentifier identifier, StringBuilder messageText)
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
            sb.AppendLine(MessageText.ToString());
            sb.AppendLine();
            return sb.ToString();
        }
    }
}

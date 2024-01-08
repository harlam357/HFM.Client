namespace HFM.Client;

/// <summary>
/// Uniquely identifies a Folding@Home client message by the message type and date received.
/// </summary>
public readonly record struct FahClientMessageIdentifier
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
    /// Returns a <see cref="String"/> that represents the current <see cref="FahClientMessageIdentifier"/>.
    /// </summary>
    public override string ToString() => $"Message Type: {MessageType} - Received at: {Received}";
}

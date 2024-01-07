namespace HFM.Client;

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

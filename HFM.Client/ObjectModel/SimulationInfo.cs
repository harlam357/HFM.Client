using System.Text;

namespace HFM.Client.ObjectModel;

/// <summary>
/// Folding@Home client simulation info message.
/// </summary>
public class SimulationInfo
{
    /// <summary>
    /// Creates a new <see cref="SimulationInfo"/> object from a <see cref="String"/> that contains JSON.
    /// </summary>
    public static SimulationInfo? Load(string json) => new Internal.SimulationInfoObjectLoader().Load(json);

    /// <summary>
    /// Creates a new <see cref="SimulationInfo"/> object from a <see cref="String"/> that contains JSON.
    /// </summary>
    public static SimulationInfo? Load(string json, ObjectLoadOptions options) => new Internal.SimulationInfoObjectLoader().Load(json, options);

    /// <summary>
    /// Creates a new <see cref="SimulationInfo"/> object from a <see cref="StringBuilder"/> that contains JSON.
    /// </summary>
    public static SimulationInfo? Load(StringBuilder json) => new Internal.SimulationInfoObjectLoader().Load(json);

    /// <summary>
    /// Creates a new <see cref="SimulationInfo"/> object from a <see cref="StringBuilder"/> that contains JSON.
    /// </summary>
    public static SimulationInfo? Load(StringBuilder json, ObjectLoadOptions options) => new Internal.SimulationInfoObjectLoader().Load(json, options);

    /// <summary>
    /// Creates a new <see cref="SimulationInfo"/> object from a <see cref="TextReader"/> that contains JSON.
    /// </summary>
    public static SimulationInfo? Load(TextReader textReader) => new Internal.SimulationInfoObjectLoader().Load(textReader);

    public string? User { get; set; }
    public int? Team { get; set; }
    public int? Project { get; set; }
    public int? Run { get; set; }
    public int? Clone { get; set; }
    public int? Gen { get; set; }
    public int? CoreType { get; set; }
    public string? Core { get; set; }
    public int? TotalIterations { get; set; }
    public int? IterationsDone { get; set; }
    public int? Energy { get; set; }
    public int? Temperature { get; set; }
    public string? StartTime { get; set; }
    public DateTime? StartTimeDateTime { get; set; }
    public int? Timeout { get; set; }
    public DateTime? TimeoutDateTime { get; set; }
    public int? Deadline { get; set; }
    public DateTime? DeadlineDateTime { get; set; }
    public int? ETA { get; set; }
    public TimeSpan? ETATimeSpan { get; set; }
    public double? Progress { get; set; }
    public int? Slot { get; set; }
}

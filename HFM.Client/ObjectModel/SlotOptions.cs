using System.Text;

namespace HFM.Client.ObjectModel;

/// <summary>
/// Folding@Home client slot options message.
/// </summary>
public class SlotOptions : OptionsBase
{
    /// <summary>
    /// Creates a new <see cref="SlotOptions"/> object from a <see cref="string"/> that contains JSON.
    /// </summary>
    public static SlotOptions? Load(string json) => new Internal.SlotOptionsObjectLoader().Load(json);

    /// <summary>
    /// Creates a new <see cref="SlotOptions"/> object from a <see cref="string"/> that contains JSON.
    /// </summary>
    public static SlotOptions? Load(string json, ObjectLoadOptions options) => new Internal.SlotOptionsObjectLoader().Load(json, options);

    /// <summary>
    /// Creates a new <see cref="SlotOptions"/> object from a <see cref="StringBuilder"/> that contains JSON.
    /// </summary>
    public static SlotOptions? Load(StringBuilder json) => new Internal.SlotOptionsObjectLoader().Load(json);

    /// <summary>
    /// Creates a new <see cref="SlotOptions"/> object from a <see cref="StringBuilder"/> that contains JSON.
    /// </summary>
    public static SlotOptions? Load(StringBuilder json, ObjectLoadOptions options) => new Internal.SlotOptionsObjectLoader().Load(json, options);

    /// <summary>
    /// Creates a new <see cref="SlotOptions"/> object from a <see cref="TextReader"/> that contains JSON.
    /// </summary>
    public static SlotOptions? Load(TextReader textReader) => new Internal.SlotOptionsObjectLoader().Load(textReader);
}

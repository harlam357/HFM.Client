using System.Text;

namespace HFM.Client.ObjectModel;

public class LogUpdate
{
    /// <summary>
    /// Folding@Home client log update message.
    /// </summary>
    public StringBuilder? Value { get; set; }

    /// <summary>
    /// Creates a new <see cref="LogUpdate"/> object from a <see cref="String"/> that contains the log text.
    /// </summary>
    public static LogUpdate? Load(string? text) => new Internal.LogUpdateObjectLoader().Load(text);

    /// <summary>
    /// Creates a new <see cref="LogUpdate"/> object from a <see cref="String"/> that contains the log text.
    /// </summary>
    public static LogUpdate? Load(string? text, ObjectLoadOptions options) => new Internal.LogUpdateObjectLoader().Load(text, options);

    /// <summary>
    /// Creates a new <see cref="LogUpdate"/> object from a <see cref="StringBuilder"/> that contains the log text.
    /// </summary>
    public static LogUpdate? Load(StringBuilder? text) => new Internal.LogUpdateObjectLoader().Load(text);

    /// <summary>
    /// Creates a new <see cref="LogUpdate"/> object from a <see cref="StringBuilder"/> that contains the log text.
    /// </summary>
    public static LogUpdate? Load(StringBuilder? text, ObjectLoadOptions options) => new Internal.LogUpdateObjectLoader().Load(text, options);

    /// <summary>
    /// Creates a new <see cref="LogUpdate"/> object from a <see cref="TextReader"/> that contains the log text.
    /// </summary>
    public static LogUpdate? Load(TextReader? textReader) => new Internal.LogUpdateObjectLoader().Load(textReader);
}

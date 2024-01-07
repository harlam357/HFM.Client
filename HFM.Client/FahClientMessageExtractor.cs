using System.Text;

using HFM.Client.Internal;

namespace HFM.Client;

/// <summary>
/// Provides the abstract base class for a Folding@Home client message extraction.
/// </summary>
public abstract class FahClientMessageExtractor
{
    /// <summary>
    /// Gets the default <see cref="FahClientMessageExtractor"/> that extracts <see cref="FahClientMessage"/> objects.
    /// </summary>
    public static FahClientMessageExtractor Default { get; } = new FahClientJsonMessageExtractor();

    /// <summary>
    /// Extracts a new <see cref="FahClientMessage"/> from the <paramref name="buffer"/> if a message is available.
    /// </summary>
    /// <returns>A new <see cref="FahClientMessage"/> or null if a message is not available.</returns>
    public abstract FahClientMessage Extract(StringBuilder buffer);

    /// <summary>
    /// Reports the indexes of the first occurrence of the specified strings in the given StringBuilder object.
    /// </summary>
    protected static (int Start, int End) IndexesOfAny(StringBuilder buffer, IEnumerable<string> values, int startIndex)
    {
        if (values != null)
        {
            foreach (var value in values)
            {
                int index = buffer.IndexOf(value, startIndex);
                if (index >= 0)
                {
                    return (index, index + value.Length);
                }
            }
        }

        return NoIndexTuple;
    }

    /// <summary>
    /// Reports the index of the first occurrence of the specified strings in the given StringBuilder object.
    /// </summary>
    protected static int IndexOfAny(StringBuilder buffer, IEnumerable<string> values, int startIndex)
    {
        if (values != null)
        {
            foreach (var value in values)
            {
                int index = buffer.IndexOf(value, startIndex);
                if (index >= 0)
                {
                    return index;
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// Reports the end index of the first occurrence of the specified strings in the given StringBuilder object.
    /// </summary>
    protected static int EndIndexOfAny(StringBuilder buffer, IEnumerable<string> values, int startIndex)
    {
        if (values != null)
        {
            foreach (var value in values)
            {
                int index = buffer.IndexOf(value, startIndex);
                if (index >= 0)
                {
                    return index + value.Length;
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// Reports the indexes of the first occurrence of the specified string in the given StringBuilder object.
    /// </summary>
    protected static (int Start, int End) IndexesOf(StringBuilder buffer, string value, int startIndex)
    {
        if (value != null)
        {
            int index = buffer.IndexOf(value, startIndex);
            if (index >= 0)
            {
                return (index, index + value.Length);
            }
        }

        return NoIndexTuple;
    }

    private static readonly (int, int) NoIndexTuple = (-1, -1);
}

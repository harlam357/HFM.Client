using System.Globalization;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

using HFM.Client.Internal;

namespace HFM.Client.ObjectModel.Internal;

internal class UnitCollectionObjectLoader : ObjectLoader<UnitCollection>
{
    public override UnitCollection Load(TextReader textReader)
    {
        if (textReader is null) return null;
            
        var array = LoadJsonArray(textReader);

        var collection = new UnitCollection();
        foreach (var obj in array.Select(x => x.AsObject()).Where(x => x.Count != 0))
        {
            collection.Add(LoadUnit(obj));
        }
        return collection;
    }

    // ReSharper disable StringLiteralTypo

    private Unit LoadUnit(JsonObject obj)
    {
        var slot = new Unit();
        slot.ID = GetValue<string>(obj, "id").ToNullableInt32();
        slot.State = GetValue<string>(obj, "state");
        slot.Error = GetValue<string>(obj, "error");
        slot.Project = GetValue<int?>(obj, "project");
        slot.Run = GetValue<int?>(obj, "run");
        slot.Clone = GetValue<int?>(obj, "clone");
        slot.Gen = GetValue<int?>(obj, "gen");
        slot.Core = GetValue<string>(obj, "core");
        slot.UnitHex = GetValue<string>(obj, "unit");
        slot.PercentDone = GetValue<string>(obj, "percentdone");
        slot.TotalFrames = GetValue<int?>(obj, "totalframes");
        slot.FramesDone = GetValue<int?>(obj, "framesdone");
        slot.Assigned = GetValue<string>(obj, "assigned");
        slot.AssignedDateTime = DateTimeConverter.ConvertToDateTime(slot.Assigned);
        slot.Timeout = GetValue<string>(obj, "timeout");
        slot.TimeoutDateTime = DateTimeConverter.ConvertToDateTime(slot.Timeout);
        slot.Deadline = GetValue<string>(obj, "deadline");
        slot.DeadlineDateTime = DateTimeConverter.ConvertToDateTime(slot.Deadline);
        slot.WorkServer = GetValue<string>(obj, "ws");
        slot.CollectionServer = GetValue<string>(obj, "cs");
        slot.WaitingOn = GetValue<string>(obj, "waitingon");
        slot.Attempts = GetValue<int?>(obj, "attempts");
        slot.NextAttempt = GetValue<string>(obj, "nextattempt");
        slot.NextAttemptTimeSpan = ConvertToTimeSpan(slot.NextAttempt);
        slot.Slot = GetValue<int?>(obj, "slot");
        slot.ETA = GetValue<string>(obj, "eta");
        slot.ETATimeSpan = ConvertToTimeSpan(slot.ETA);
        slot.PPD = GetValue<string>(obj, "ppd").ToNullableDouble();
        slot.TPF = GetValue<string>(obj, "tpf");
        slot.TPFTimeSpan = ConvertToTimeSpan(slot.TPF);
        slot.BaseCredit = GetValue<string>(obj, "basecredit").ToNullableDouble();
        slot.CreditEstimate = GetValue<string>(obj, "creditestimate").ToNullableDouble();
        return slot;
    }

    // ReSharper restore StringLiteralTypo

    private static TimeSpan? ConvertToTimeSpan(string value)
    {
        if (value is null) return null;

        var options = RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.Singleline;
        try
        {
            Match match;
            var regex = new Regex("(?<Hours>.+) hours (?<Minutes>.+) mins (?<Seconds>.+) secs", options);
            if ((match = regex.Match(value)).Success)
            {
                return new TimeSpan(Convert.ToInt32(match.Groups["Hours"].Value, CultureInfo.InvariantCulture),
                    Convert.ToInt32(match.Groups["Minutes"].Value, CultureInfo.InvariantCulture),
                    Convert.ToInt32(match.Groups["Seconds"].Value, CultureInfo.InvariantCulture));
            }

            regex = new Regex("(?<Hours>.+) hours (?<Minutes>.+) mins", options);
            if ((match = regex.Match(value)).Success)
            {
                return new TimeSpan(Convert.ToInt32(match.Groups["Hours"].Value, CultureInfo.InvariantCulture),
                    Convert.ToInt32(match.Groups["Minutes"].Value, CultureInfo.InvariantCulture),
                    0);
            }

            regex = new Regex("(?<Minutes>.+) mins (?<Seconds>.+) secs", options);
            if ((match = regex.Match(value)).Success)
            {
                return new TimeSpan(0,
                    Convert.ToInt32(match.Groups["Minutes"].Value, CultureInfo.InvariantCulture),
                    Convert.ToInt32(match.Groups["Seconds"].Value, CultureInfo.InvariantCulture));
            }

            regex = new Regex("(?<Seconds>.+) secs", options);
            if ((match = regex.Match(value)).Success)
            {
                return TimeSpan.FromSeconds(Convert.ToDouble(match.Groups["Seconds"].Value, CultureInfo.InvariantCulture));
            }

            regex = new Regex("(?<Days>.+) days", options);
            if ((match = regex.Match(value)).Success)
            {
                return TimeSpan.FromDays(Convert.ToDouble(match.Groups["Days"].Value, CultureInfo.InvariantCulture));
            }
        }
        catch (FormatException)
        {
            // if the data changed in a way where the value can
            // no longer be converted to the target CLR type
        }

        return null;
    }
}

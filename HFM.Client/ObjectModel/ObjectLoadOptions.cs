namespace HFM.Client.ObjectModel;

[Flags]
public enum ObjectLoadOptions
{
    None = 0,
    DecodeHex = 1 << 0,
    FilterNewLineCharacters = 1 << 1,
    Default = DecodeHex | FilterNewLineCharacters
}

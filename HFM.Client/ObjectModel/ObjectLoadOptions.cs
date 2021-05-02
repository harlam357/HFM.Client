using System;

namespace HFM.Client.ObjectModel
{
    [Flags]
    public enum ObjectLoadOptions
    {
        None = 0,
        DecodeHex = 1 << 0,
        Default = DecodeHex
    }
}

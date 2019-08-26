
using NUnit.Framework;

namespace HFM.Client.ObjectModel
{
   [TestFixture]
   public class InfoTests
   {
      private const string InfoText = @"[
  [
    ""Folding @home Client"",
    [
      ""Website"",
      ""http://folding.stanford.edu/""
    ],
    [
      ""Copyright"",
      ""(c) 2009,2010 Stanford University""
    ],
    [
      ""Author"",
      ""Joseph Coffland <joseph@cauldrondevelopment.com>""
    ],
    [
      ""Args"",
      "" --lifeline 1232 --command-port=36330""
    ],
    [
      ""Config"",
      ""C:/Documents and Settings/user/Application Data/FAHClient/config.xml""
    ]
  ],
  [
    ""Build"",
    [
      ""Version"",
      ""7.1.24""
    ],
    [
      ""Date"",
      ""Apr  6 2011""
    ],
    [
      ""Time"",
      ""21:37:58""
    ],
    [
      ""SVN Rev"",
      ""2908""
    ],
    [
      ""Branch"",
      ""fah/trunk/client""
    ],
    [
      ""Compiler"",
      ""Intel(R) C++ MSVC 1500 mode 1110""
    ],
    [
      ""Options"",
      ""/TP /nologo /EHa /wd4297 /wd4103 /wd1786 /Ox -arch:SSE2 /QaxSSE3,SSSE3,SSE4.1,SSE4.2 /Qrestrict /MT""
    ],
    [
      ""Platform"",
      ""win32 Vista""
    ],
    [
      ""Bits"",
      ""32""
    ],
    [
      ""Mode"",
      ""Release""
    ]
  ],
  [
    ""System"",
    [
      ""OS"",
      ""Microsoft(R) Windows(R) XP Professional x64 Edition""
    ],
    [
      ""CPU"",
      ""Intel(R) Core(TM)2 Quad CPU    Q6600  @ 2.40GHz""
    ],
    [
      ""CPU ID"",
      ""GenuineIntel Family 6 Model 15 Stepping 11""
    ],
    [
      ""CPUs"",
      ""4""
    ],
    [
      ""Memory"",
      ""4.00GiB""
    ],
    [
      ""Free Memory"",
      ""3.10GiB""
    ],
    [
      ""Threads"",
      ""WINDOWS_THREADS""
    ],
    [
      ""GPUs"",
      ""1""
    ],
    [
      ""GPU 0"",
      ""ATI:2 Mobility Radeon HD 3600 Series""
    ],
    [
      ""CUDA"",
      ""Not detected""
    ],
    [
      ""On Battery"",
      ""false""
    ],
    [
      ""UTC offset"",
      ""-5""
    ],
    [
      ""PID"",
      ""3080""
    ],
    [
      ""CWD"",
      ""C:/Documents and Settings/user/Application Data/FAHClient""
    ],
    [
      ""Win32 Service"",
      ""true""
    ]
  ]
]";

      [Test]
      public void Info_FromJson_Test()
      {
         var info = Info.FromJson(InfoText);
         Assert.AreEqual("http://folding.stanford.edu/", info.Client.Website);
         Assert.AreEqual("(c) 2009,2010 Stanford University", info.Client.Copyright);
         Assert.AreEqual("Joseph Coffland <joseph@cauldrondevelopment.com>", info.Client.Author);
         Assert.AreEqual("--lifeline 1232 --command-port=36330", info.Client.Args);
         Assert.AreEqual("C:/Documents and Settings/user/Application Data/FAHClient/config.xml", info.Client.Config);
         Assert.AreEqual("7.1.24", info.Build.Version);
         Assert.AreEqual("Apr  6 2011", info.Build.Date);
         Assert.AreEqual("21:37:58", info.Build.Time);
         Assert.AreEqual(2908, info.Build.SvnRev);
         Assert.AreEqual("fah/trunk/client", info.Build.Branch);
         Assert.AreEqual("Intel(R) C++ MSVC 1500 mode 1110", info.Build.Compiler);
         Assert.AreEqual("/TP /nologo /EHa /wd4297 /wd4103 /wd1786 /Ox -arch:SSE2 /QaxSSE3,SSSE3,SSE4.1,SSE4.2 /Qrestrict /MT", info.Build.Options);
         Assert.AreEqual("win32 Vista", info.Build.Platform);
         Assert.AreEqual(32, info.Build.Bits);
         Assert.AreEqual("Release", info.Build.Mode);
         Assert.AreEqual("Microsoft(R) Windows(R) XP Professional x64 Edition", info.System.OperatingSystem);
         Assert.AreEqual("Intel(R) Core(TM)2 Quad CPU    Q6600  @ 2.40GHz", info.System.Cpu);
         Assert.AreEqual("GenuineIntel Family 6 Model 15 Stepping 11", info.System.CpuId);
         Assert.AreEqual(4, info.System.CpuCount);
         Assert.AreEqual("4.00GiB", info.System.Memory);
         Assert.AreEqual(4.0, info.System.MemoryValue);
         Assert.AreEqual("3.10GiB", info.System.FreeMemory);
         Assert.AreEqual(3.1, info.System.FreeMemoryValue);
      }
   }
}


using NUnit.Framework;

namespace HFM.Client.ObjectModel
{
    [TestFixture]
    public class InfoTests
    {
        [Test]
        public void Info_Load_FromWindowsClientVersion_7_1_24()
        {
            var info = Info.Load(TestDataReader.ReadStringBuilder("Client_7_1_24_info_Windows.txt"));
            Assert.AreEqual("http://folding.stanford.edu/", info.Client.Website);
            Assert.AreEqual("(c) 2009,2010 Stanford University", info.Client.Copyright);
            Assert.AreEqual("Joseph Coffland <joseph@cauldrondevelopment.com>", info.Client.Author);
            Assert.AreEqual("--lifeline 1232 --command-port=36330", info.Client.Args);
            Assert.AreEqual("C:/Documents and Settings/user/Application Data/FAHClient/config.xml", info.Client.Config);
            Assert.AreEqual("7.1.24", info.Build.Version);
            Assert.AreEqual("Apr  6 2011", info.Build.Date);
            Assert.AreEqual("21:37:58", info.Build.Time);
            Assert.AreEqual(2908, info.Build.SVNRev);
            Assert.AreEqual("fah/trunk/client", info.Build.Branch);
            Assert.AreEqual("Intel(R) C++ MSVC 1500 mode 1110", info.Build.Compiler);
            Assert.AreEqual("/TP /nologo /EHa /wd4297 /wd4103 /wd1786 /Ox -arch:SSE2 /QaxSSE3,SSSE3,SSE4.1,SSE4.2 /Qrestrict /MT", info.Build.Options);
            Assert.AreEqual("win32 Vista", info.Build.Platform);
            Assert.AreEqual(32, info.Build.Bits);
            Assert.AreEqual("Release", info.Build.Mode);
            Assert.AreEqual("Microsoft(R) Windows(R) XP Professional x64 Edition", info.System.OS);
            Assert.AreEqual("Intel(R) Core(TM)2 Quad CPU    Q6600  @ 2.40GHz", info.System.CPU);
            Assert.AreEqual("GenuineIntel Family 6 Model 15 Stepping 11", info.System.CPUID);
            Assert.AreEqual(4, info.System.CPUs);
            Assert.AreEqual("4.00GiB", info.System.Memory);
            Assert.AreEqual(4.0, info.System.MemoryValue);
            Assert.AreEqual("3.10GiB", info.System.FreeMemory);
            Assert.AreEqual(3.1, info.System.FreeMemoryValue);
            Assert.AreEqual("WINDOWS_THREADS", info.System.Threads);
            Assert.AreEqual(1, info.System.GPUs);
            Assert.IsNotNull(info.System.GPUInfos);
            Assert.AreEqual(1, info.System.GPUInfos.Count);
            Assert.AreEqual("ATI:2 Mobility Radeon HD 3600 Series", info.System.GPUInfos[0].GPU);
            Assert.AreEqual("ATI:2 Mobility Radeon HD 3600 Series", info.System.GPUInfos[0].FriendlyName);
            Assert.AreEqual("Not detected", info.System.CUDA);
            Assert.AreEqual(null, info.System.CUDADriver);
            Assert.AreEqual(false, info.System.HasBattery);
            Assert.AreEqual(false, info.System.OnBattery);
            Assert.AreEqual(-5, info.System.UtcOffset);
            Assert.AreEqual(3080, info.System.PID);
            Assert.AreEqual("C:/Documents and Settings/user/Application Data/FAHClient", info.System.CWD);
            Assert.AreEqual(true, info.System.Win32Service);
        }
    }
}

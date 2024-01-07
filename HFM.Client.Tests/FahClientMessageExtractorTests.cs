using System.Text;

using NUnit.Framework;

namespace HFM.Client;

[TestFixture]
public class FahClientMessageExtractorTests
{
    private const string Info = @"PyON 1 info
[
  [
    ""Folding @home Client"",
      [
         ""Website"",
         ""http://folding.stanford.edu/""
      ],
      [
         ""Copyright"",
         ""(c) 2009-2013 Stanford University""
      ],
      [
         ""Author"",
         ""Joseph Coffland <joseph@cauldrondevelopment.com>""
      ],
      [
         ""Args"",
         """"
      ],
      [
         ""Config"",
         ""C:/Program Files (x86)/FAHClient/config.xml""
      ]
   ],
   [
      ""Build"",
      [
         ""Version"",
         ""7.3.2""
      ],
      [
         ""Date"",
         ""Feb  1 2013""
      ],
      [
         ""Time"",
         ""01:46:52""
      ],
      [
         ""SVN Rev"",
         ""3852""
      ],
      [
         ""Branch"",
         ""fah/trunk/client""
      ],
      [
         ""Compiler"",
         ""Intel(R) C++ MSVC 1500 mode 1200""
      ],
      [
         ""Options"",
         ""/TP /nologo /EHa /Qdiag-disable:4297,4103,1786,279 /Ox -arch:SSE /QaxSSE2,SSE3,SSSE3,SSE4.1,SSE4.2 /Qopenmp /Qrestrict /MT /Qmkl""
      ],
      [
         ""Platform"",
         ""win32 XP""
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
         ""CPU"",
         ""       Intel(R) Core(TM) i5-3450S CPU @ 2.80GHz""
      ],
      [
         ""CPU ID"",
         ""GenuineIntel Family 6 Model 58 Stepping 9""
      ],
      [
         ""CPUs"",
         ""4""
      ],
      [
         ""Memory"",
         ""3.45GiB""
      ],
      [
         ""Free Memory"",
         ""2.80GiB""
      ],
      [
         ""Threads"",
         ""WINDOWS_THREADS""
      ],
      [
         ""Has Battery"",
         ""false""
      ],
      [
         ""On Battery"",
         ""false""
      ],
      [
         ""UTC offset"",
         ""-8""
      ],
      [
         ""PID"",
         ""584""
      ],
      [
         ""CWD"",
         ""C:/Program Files (x86)/FAHClient""
      ],
      [
         ""OS"",
         ""Windows 8 Pro""
      ],
      [
         ""OS Arch"",
         ""AMD64""
      ],
      [
         ""GPUs"",
         ""0""
      ],
      [
         ""CUDA"",
         ""Not detected""
      ],
      [
         ""Win32 Service"",
         ""false""
      ]
   ]
]
---
";

    private const string SimulationInfo = @"PyON 1 simulation-info
{
   ""user"": ""harlam357"",
   ""team"": ""32"",
   ""project"": 7006,
   ""run"": 1,
   ""clone"": 540,
   ""gen"": 6,
   ""core_type"": 164,
   ""core"": ""GRO_A4"",
   ""total_iterations"": 0,
   ""iterations_done"": 0,
   ""energy"": 0,
   ""temperature"": 0,
   ""start_time"": ""2013-02-02T16:44:18Z"",
   ""timeout"": 1360514658,
   ""deadline"": 1360773858,
   ""eta"": 9975,
   ""progress"": 0.43,
   ""news"": """",
   ""slot"": 0
}
---
";

    private const string SimulationInfoJson = @"{
   ""user"": ""harlam357"",
   ""team"": ""32"",
   ""project"": 7006,
   ""run"": 1,
   ""clone"": 540,
   ""gen"": 6,
   ""core_type"": 164,
   ""core"": ""GRO_A4"",
   ""total_iterations"": 0,
   ""iterations_done"": 0,
   ""energy"": 0,
   ""temperature"": 0,
   ""start_time"": ""2013-02-02T16:44:18Z"",
   ""timeout"": 1360514658,
   ""deadline"": 1360773858,
   ""eta"": 9975,
   ""progress"": 0.43,
   ""news"": """",
   ""slot"": 0
}";

    [Test]
    public void FahClientPyonMessageExtractor_ReturnsNullWhenBufferIsNull()
    {
        // Arrange
        var extractor = new FahClientPyonMessageExtractor();
        // Act
        var result = extractor.Extract(null);
        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void FahClientPyonMessageExtractor_ExtractsSingleMessageFromSingleMessage()
    {
        // Arrange
        var extractor = new FahClientPyonMessageExtractor();
        var buffer = new StringBuilder(SimulationInfo);
        // Act
        var result = extractor.Extract(buffer);
        // Assert
        Assert.AreEqual("simulation-info", result.Identifier.MessageType);
        Assert.AreEqual(SimulationInfo, result.MessageText.ToString());
    }

    [Test]
    public void FahClientPyonMessageExtractor_ExtractsSingleMessageFromMultipleMessages()
    {
        // Arrange
        var extractor = new FahClientPyonMessageExtractor();
        var buffer = new StringBuilder(Info + SimulationInfo);
        // Act
        var result = extractor.Extract(buffer);
        // Assert
        Assert.AreEqual("info", result.Identifier.MessageType);
        Assert.AreEqual(Info, result.MessageText.ToString());
    }

    [Test]
    public void FahClientPyonMessageExtractor_ExtractsMultipleMessagesFromMultipleMessages()
    {
        // Arrange
        var extractor = new FahClientPyonMessageExtractor();
        var buffer = new StringBuilder(Info + SimulationInfo);
        // Act (First Message)
        var result = extractor.Extract(buffer);
        // Assert (First Message)
        Assert.AreEqual("info", result.Identifier.MessageType);
        Assert.AreEqual(Info, result.MessageText.ToString());
        // Act (Second Message)
        result = extractor.Extract(buffer);
        // Assert (Second Message)
        Assert.AreEqual("simulation-info", result.Identifier.MessageType);
        Assert.AreEqual(SimulationInfo, result.MessageText.ToString());
    }

    [Test]
    public void FahClientPyonMessageExtractor_CannotExtractMessageWithNoHeader()
    {
        // Arrange
        var extractor = new FahClientPyonMessageExtractor();
        var buffer = new StringBuilder();
        // Act
        var result = extractor.Extract(buffer);
        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void FahClientPyonMessageExtractor_CannotExtractMessageWithOnlyHeader()
    {
        // Arrange
        var extractor = new FahClientPyonMessageExtractor();
        var buffer = new StringBuilder(Info.Substring(0, 11));
        // Act
        var result = extractor.Extract(buffer);
        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void FahClientPyonMessageExtractor_CannotExtractMessageWithNoFooter()
    {
        // Arrange
        var extractor = new FahClientPyonMessageExtractor();
        var buffer = new StringBuilder(Info.Substring(0, Info.Length / 2));
        // Act
        var result = extractor.Extract(buffer);
        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void FahClientJsonMessageExtractor_ExtractsMessageTextAsJson()
    {
        // Arrange
        var extractor = new FahClientJsonMessageExtractor();
        var buffer = new StringBuilder(SimulationInfo);
        // Act
        var result = extractor.Extract(buffer);
        // Assert
        Assert.AreEqual("simulation-info", result.Identifier.MessageType);
        Assert.AreEqual(SimulationInfoJson, result.MessageText.ToString());
    }

    [Test]
    public void FahClientJsonMessageExtractor_CannotExtractMessageWithNoHeader()
    {
        // Arrange
        var extractor = new FahClientJsonMessageExtractor();
        var buffer = new StringBuilder();
        // Act
        var result = extractor.Extract(buffer);
        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void FahClientJsonMessageExtractor_CannotExtractMessageWithOnlyHeader()
    {
        // Arrange
        var extractor = new FahClientJsonMessageExtractor();
        var buffer = new StringBuilder(Info.Substring(0, 11));
        // Act
        var result = extractor.Extract(buffer);
        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void FahClientJsonMessageExtractor_CannotExtractMessageWithNoFooter()
    {
        // Arrange
        var extractor = new FahClientJsonMessageExtractor();
        var buffer = new StringBuilder(Info.Substring(0, Info.Length / 2));
        // Act
        var result = extractor.Extract(buffer);
        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void FahClientPyonMessageExtractor_HandlesNullMessageType()
    {
        // Arrange
        var extractor = new PyonMessageExtractorReturnsNullMessageType();
        var buffer = new StringBuilder(Info);
        // Act
        var result = extractor.Extract(buffer);
        // Assert
        Assert.IsNull(result);
    }

    private class PyonMessageExtractorReturnsNullMessageType : FahClientPyonMessageExtractor
    {
        protected override string ExtractMessageType(StringBuilder buffer, IDictionary<string, int> indexes)
        {
            return null;
        }
    }

    [Test]
    public void FahClientPyonMessageExtractor_HandlesNullMessageText()
    {
        // Arrange
        var extractor = new PyonMessageExtractorReturnsNullMessageText();
        var buffer = new StringBuilder(Info);
        // Act
        var result = extractor.Extract(buffer);
        // Assert
        Assert.IsNull(result);
    }

    private class PyonMessageExtractorReturnsNullMessageText : FahClientPyonMessageExtractor
    {
        protected override StringBuilder ExtractMessageText(StringBuilder buffer, IDictionary<string, int> indexes)
        {
            return null;
        }
    }

    [Test]
    public void FahClientPyonMessageExtractor_ExtractIndexes_ThrowsWhenBufferIsNull()
    {
        // Arrange
        var extractor = new PyonMessageExtractorExtractIndexesThrowsWhenBufferIsNull();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => extractor.Extract(null));
    }

    private class PyonMessageExtractorExtractIndexesThrowsWhenBufferIsNull : FahClientPyonMessageExtractor
    {
        public override FahClientMessage Extract(StringBuilder buffer)
        {
            ExtractIndexes(null, new Dictionary<string, int>());
            return null;
        }
    }

    [Test]
    public void FahClientPyonMessageExtractor_ExtractIndexes_ThrowsWhenIndexesIsNull()
    {
        // Arrange
        var extractor = new PyonMessageExtractorExtractIndexesThrowsWhenIndexesIsNull();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => extractor.Extract(null));
    }

    private class PyonMessageExtractorExtractIndexesThrowsWhenIndexesIsNull : FahClientPyonMessageExtractor
    {
        public override FahClientMessage Extract(StringBuilder buffer)
        {
            ExtractIndexes(new StringBuilder(), null);
            return null;
        }
    }

    [Test]
    public void FahClientPyonMessageExtractor_ExtractMessageType_ThrowsWhenBufferIsNull()
    {
        // Arrange
        var extractor = new PyonMessageExtractorExtractMessageTypeThrowsWhenBufferIsNull();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => extractor.Extract(null));
    }

    private class PyonMessageExtractorExtractMessageTypeThrowsWhenBufferIsNull : FahClientPyonMessageExtractor
    {
        public override FahClientMessage Extract(StringBuilder buffer)
        {
            ExtractMessageType(null, new Dictionary<string, int>());
            return null;
        }
    }

    [Test]
    public void FahClientPyonMessageExtractor_ExtractMessageType_ThrowsWhenIndexesIsNull()
    {
        // Arrange
        var extractor = new PyonMessageExtractorExtractMessageTypeThrowsWhenIndexesIsNull();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => extractor.Extract(null));
    }

    private class PyonMessageExtractorExtractMessageTypeThrowsWhenIndexesIsNull : FahClientPyonMessageExtractor
    {
        public override FahClientMessage Extract(StringBuilder buffer)
        {
            ExtractMessageType(new StringBuilder(), null);
            return null;
        }
    }

    [Test]
    public void FahClientPyonMessageExtractor_ExtractMessageText_ThrowsWhenBufferIsNull()
    {
        // Arrange
        var extractor = new PyonMessageExtractorExtractMessageTextThrowsWhenBufferIsNull();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => extractor.Extract(null));
    }

    private class PyonMessageExtractorExtractMessageTextThrowsWhenBufferIsNull : FahClientPyonMessageExtractor
    {
        public override FahClientMessage Extract(StringBuilder buffer)
        {
            ExtractMessageText(null, new Dictionary<string, int>());
            return null;
        }
    }

    [Test]
    public void FahClientPyonMessageExtractor_ExtractMessageText_ThrowsWhenIndexesIsNull()
    {
        // Arrange
        var extractor = new PyonMessageExtractorExtractMessageTextThrowsWhenIndexesIsNull();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => extractor.Extract(null));
    }

    private class PyonMessageExtractorExtractMessageTextThrowsWhenIndexesIsNull : FahClientPyonMessageExtractor
    {
        public override FahClientMessage Extract(StringBuilder buffer)
        {
            ExtractMessageText(new StringBuilder(), null);
            return null;
        }
    }

    [Test]
    public void FahClientJsonMessageExtractor_ExtractIndexes_ThrowsWhenBufferIsNull()
    {
        // Arrange
        var extractor = new JsonMessageExtractorExtractIndexesThrowsWhenBufferIsNull();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => extractor.Extract(null));
    }

    private class JsonMessageExtractorExtractIndexesThrowsWhenBufferIsNull : FahClientJsonMessageExtractor
    {
        public override FahClientMessage Extract(StringBuilder buffer)
        {
            ExtractIndexes(null, new Dictionary<string, int>());
            return null;
        }
    }

    [Test]
    public void FahClientJsonMessageExtractor_ExtractIndexes_ThrowsWhenIndexesIsNull()
    {
        // Arrange
        var extractor = new JsonMessageExtractorExtractIndexesThrowsWhenIndexesIsNull();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => extractor.Extract(null));
    }

    private class JsonMessageExtractorExtractIndexesThrowsWhenIndexesIsNull : FahClientJsonMessageExtractor
    {
        public override FahClientMessage Extract(StringBuilder buffer)
        {
            ExtractIndexes(new StringBuilder(), null);
            return null;
        }
    }

    [Test]
    public void FahClientJsonMessageExtractor_ExtractMessageType_ThrowsWhenBufferIsNull()
    {
        // Arrange
        var extractor = new JsonMessageExtractorExtractMessageTypeThrowsWhenBufferIsNull();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => extractor.Extract(null));
    }

    private class JsonMessageExtractorExtractMessageTypeThrowsWhenBufferIsNull : FahClientJsonMessageExtractor
    {
        public override FahClientMessage Extract(StringBuilder buffer)
        {
            ExtractMessageType(null, new Dictionary<string, int>());
            return null;
        }
    }

    [Test]
    public void FahClientJsonMessageExtractor_ExtractMessageType_ThrowsWhenIndexesIsNull()
    {
        // Arrange
        var extractor = new JsonMessageExtractorExtractMessageTypeThrowsWhenIndexesIsNull();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => extractor.Extract(null));
    }

    private class JsonMessageExtractorExtractMessageTypeThrowsWhenIndexesIsNull : FahClientJsonMessageExtractor
    {
        public override FahClientMessage Extract(StringBuilder buffer)
        {
            ExtractMessageType(new StringBuilder(), null);
            return null;
        }
    }

    [Test]
    public void FahClientJsonMessageExtractor_ExtractMessageText_ThrowsWhenBufferIsNull()
    {
        // Arrange
        var extractor = new JsonMessageExtractorExtractMessageTextThrowsWhenBufferIsNull();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => extractor.Extract(null));
    }

    private class JsonMessageExtractorExtractMessageTextThrowsWhenBufferIsNull : FahClientJsonMessageExtractor
    {
        public override FahClientMessage Extract(StringBuilder buffer)
        {
            ExtractMessageText(null, new Dictionary<string, int>());
            return null;
        }
    }

    [Test]
    public void FahClientJsonMessageExtractor_ExtractMessageText_ThrowsWhenIndexesIsNull()
    {
        // Arrange
        var extractor = new JsonMessageExtractorExtractMessageTextThrowsWhenIndexesIsNull();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => extractor.Extract(null));
    }

    private class JsonMessageExtractorExtractMessageTextThrowsWhenIndexesIsNull : FahClientJsonMessageExtractor
    {
        public override FahClientMessage Extract(StringBuilder buffer)
        {
            ExtractMessageText(new StringBuilder(), null);
            return null;
        }
    }
}

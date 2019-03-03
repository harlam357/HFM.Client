
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace HFM.Client
{
   [TestFixture]
   public class FahClientDataWriterTests
   {
      [Test]
      public void FahClientDataWriter_CreateThrowsArgumentNullExceptionWhenStreamIsNull()
      {
         Assert.Throws<ArgumentNullException>(() => FahClientDataWriter.Create(null));
      }

      [Test]
      public void FahClientStreamWriter_WritesToStream()
      {
         // Arrange
         using (var stream = new MemoryStream())
         {
            var dataWriter = FahClientDataWriter.Create(stream);
            // Act
            dataWriter.Write(Encoding.ASCII.GetBytes("command text"));
            // Assert
            Assert.AreEqual("command text", Encoding.ASCII.GetString(stream.ToArray()));
         }
      }

      [Test]
      public async Task FahClientStreamWriter_WritesAsynchronouslyToStream()
      {
         // Arrange
         using (var stream = new MemoryStream())
         {
            var dataWriter = FahClientDataWriter.Create(stream);
            // Act
            await dataWriter.WriteAsync(Encoding.ASCII.GetBytes("command text"));
            // Assert
            Assert.AreEqual("command text", Encoding.ASCII.GetString(stream.ToArray()));
         }
      }
   }
}

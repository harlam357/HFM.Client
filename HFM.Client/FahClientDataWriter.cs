
using System;
using System.IO;
using System.Threading.Tasks;

namespace HFM.Client
{
   /// <summary>
   /// Represents a writer that can write sequence of bytes to a Folding@Home client.
   /// </summary>
   public abstract class FahClientDataWriter
   {
      /// <summary>
      /// Writes a sequence of bytes to to a Folding@Home client.
      /// </summary>
      /// <param name="buffer">The buffer to write data from.</param>
      public abstract void Write(byte[] buffer);

      /// <summary>
      /// Asynchronously writes a sequence of bytes to to a Folding@Home client.
      /// </summary>
      /// <param name="buffer">The buffer to write data from.</param>
      public abstract Task WriteAsync(byte[] buffer);

      /// <summary>
      /// Creates a <see cref="FahClientDataWriter"/> that writes to the given <see cref="Stream"/> object.
      /// </summary>
      /// <param name="stream">The <see cref="Stream"/> that will be written to.</param>
      /// <exception cref="ArgumentNullException">stream is null.</exception>
      public static FahClientDataWriter Create(Stream stream)
      {
         return new FahClientStreamWriter(stream);
      }

      private class FahClientStreamWriter : FahClientDataWriter
      {
         private readonly Stream _stream;

         public FahClientStreamWriter(Stream stream)
         {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
         }

         public override void Write(byte[] buffer)
         {
            _stream.Write(buffer, 0, buffer.Length);
         }

         public override async Task WriteAsync(byte[] buffer)
         {
            await _stream.WriteAsync(buffer, 0, buffer.Length);
         }
      }
   }
}
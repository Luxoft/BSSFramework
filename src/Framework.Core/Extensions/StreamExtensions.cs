using System;
using System.IO;

namespace Framework.Core
{
    public static class StreamExtensions
    {
        public static byte[] ToArray(this Stream input, int bufferSize = 16 * 1024)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms, bufferSize);

                return ms.ToArray();
            }
        }

        public static byte[] ToArray(this FileStream stream)
        {
            return stream.ToArray((int) stream.Length);
        }
    }
}
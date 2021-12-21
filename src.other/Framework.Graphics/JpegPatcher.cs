namespace Framework.Graphics
{
    using System.IO;
    /// <summary>
    /// Remove JPEG EXIF data http://techmikael.blogspot.com/2009/07/removing-exif-data-continued.html
    /// </summary>
    public class JpegPatcher
    {
        public static byte[] PatchAwayExif(byte[] bytes)
        {
            if (!IsJpeg(bytes))
            {
                return bytes;
            }

            var sourceMemoryStream = new MemoryStream(bytes);
            var patchedMemoryStream = new MemoryStream();
            PatchAwayExif(sourceMemoryStream, patchedMemoryStream);
            patchedMemoryStream.Seek(0, SeekOrigin.Begin);
            return patchedMemoryStream.ToArray();
        }

        public static bool IsJpeg(byte[] bytes)
        {
            if (null == bytes || bytes.Length == 0)
            {
                return false;
            }
            if (bytes.Length < 4)
            {
                return false;
            }
            if (bytes[0] != 0xFF || bytes[1] != 0xD8 || bytes[2] != 0xFF)
            {
                return false;
            }
            return true;
        }

        public static Stream PatchAwayExif(Stream inStream, Stream outStream)
        {
            byte[] jpegHeader = new byte[2];
            jpegHeader[0] = (byte)inStream.ReadByte();
            jpegHeader[1] = (byte)inStream.ReadByte();
            if (jpegHeader[0] == 0xff && jpegHeader[1] == 0xd8) //check if it's a jpeg file
            {
                SkipAppHeaderSection(inStream);
            }
            outStream.WriteByte(0xff);
            outStream.WriteByte(0xd8);

            int readCount;
            byte[] readBuffer = new byte[4096];
            while ((readCount = inStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                outStream.Write(readBuffer, 0, readCount);

            return outStream;
        }

        private static void SkipAppHeaderSection(Stream inStream)
        {
            byte[] header = new byte[2];
            header[0] = (byte)inStream.ReadByte();
            header[1] = (byte)inStream.ReadByte();

            while (header[0] == 0xff && (header[1] >= 0xe0 && header[1] <= 0xef))
            {
                int exifLength = inStream.ReadByte();
                exifLength = exifLength << 8;
                exifLength |= inStream.ReadByte();

                for (int i = 0; i < exifLength - 2; i++)
                {
                    inStream.ReadByte();
                }
                header[0] = (byte)inStream.ReadByte();
                header[1] = (byte)inStream.ReadByte();
            }
            inStream.Position -= 2; //skip back two bytes
        }
    }
}
namespace Framework.Graphics
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    public static class ImageConverters
    {
        public static byte[] ConvertToJpeg(Bitmap Image)
        {

            MemoryStream objStream = new MemoryStream();
            ImageCodecInfo objImageCodecInfo = GetEncoderInfo("image/jpeg");
            EncoderParameters objEncoderParameters;

                if (Image == null)
                    throw new Exception("ImageObject is not initialized.");
                objEncoderParameters = new EncoderParameters(3);
                objEncoderParameters.Param[0] = new EncoderParameter(Encoder.Compression,
                 (long)EncoderValue.CompressionLZW);
                objEncoderParameters.Param[1] = new EncoderParameter(Encoder.Quality, 100L);
                objEncoderParameters.Param[2] = new EncoderParameter(Encoder.ColorDepth, 24L);
                Image.Save(objStream, objImageCodecInfo, objEncoderParameters);

            return objStream.ToArray();
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

    }
}

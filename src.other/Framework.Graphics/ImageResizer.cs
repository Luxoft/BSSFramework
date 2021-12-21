namespace Framework.Graphics
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;

    public static class ImageResizer
    {
        /// <summary>
        /// Resize JPEG image to some file size
        /// </summary>
        /// <param name="imageBytes">Image</param>
        /// <param name="maxImageSizeInBytes">Max file size in bytes for new image</param>
        /// <returns></returns>
        public static byte[] ResizeJpeg(byte[] imageBytes, int maxImageSizeInBytes)
        {
            if (imageBytes == null)
            {
                throw new ArgumentNullException(nameof(imageBytes));
            }
            if (imageBytes.Length == 0)
            {
                throw new ArgumentException("imageBytes");
            }
            if (maxImageSizeInBytes <= 15000)
            {
                throw new ArgumentException("maxImageSizeInBytes less 15000 bytes", nameof(maxImageSizeInBytes));
            }
            using (var fromStream = Image.FromStream(new MemoryStream(imageBytes)))
            {
                byte[] photoForSave = imageBytes;
                var isSaved = false;
                for (double i = 1; i > 0; i = i - 0.1)
                {
                    double scale = i;
                    using (var newImage = Resize(fromStream, scale))
                    {
                        var newMemStream = new MemoryStream();
                        newImage.Save(newMemStream, ImageFormat.Jpeg);
                        var newArray = newMemStream.ToArray();
                        if (newArray.Length > maxImageSizeInBytes)
                        {
                            continue;
                        }
                        photoForSave = newArray;
                        isSaved = true;
                    }
                    break;

                }
                if (!isSaved)
                {
                    using (var newImage = fromStream.GetThumbnailImage(126, 162, () => false, IntPtr.Zero))
                    {
                        var newMemStream = new MemoryStream();
                        newImage.Save(newMemStream, ImageFormat.Jpeg);
                        var newArray = newMemStream.ToArray();
                        photoForSave = newArray;
                    }
                }

                return photoForSave;
            }
        }

        private static Image Resize(Image img, double percentage)
        {
            //get the height and width of the image
            int originalW = img.Width;
            int originalH = img.Height;

            //get the new size based on the percentage change
            int resizedW = (int)(originalW * percentage);
            int resizedH = (int)(originalH * percentage);

            //create a new Bitmap the size of the new image
            Bitmap bmp = new Bitmap(resizedW, resizedH);
            //create a new graphic from the Bitmap
            using (Graphics graphic = Graphics.FromImage((Image)bmp))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //draw the newly resized image
                graphic.DrawImage(img, 0, 0, resizedW, resizedH);
                //dispose and free up the resources
            }
            //return the image
            return (Image)bmp;
        }
    }
}
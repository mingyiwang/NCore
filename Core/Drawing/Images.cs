using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace Core.Drawing
{
    public sealed class Images
    {

        public static byte[] Resize(byte[] image, int width, int height)
        {
            var ig = Image.FromStream(new MemoryStream(image));
            var resized = Resize(ig, width, height, true);
            using(var stream = new MemoryStream())
            {
                resized.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public static Image Resize(Image image, int width, int height, bool keepRatio)
        {
            Preconditions.CheckNotNull(image, "Source Image can not be null.");
            Preconditions.CheckNotEquals(0, width, "width can not be zero.");
            Preconditions.CheckNotEquals(0, height, "height can not be zero.");

            if(keepRatio)
            {
                return Scale(image, width, height);
            }

            var b = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            b.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using(var g = Graphics.FromImage(b))
            {
                g.Clear(Color.Transparent);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image,
                            new Rectangle(0, 0, width, height),
                            new Rectangle(0, 0, image.Width, image.Height),
                            GraphicsUnit.Pixel)
                ;
            }

            return b;
        }

        // Rsize Image by Percent
        public static Image Scale(Image image, int width, int height)
        {
            var srcWidth = image.Width;
            var srcHeight = image.Height;

            var nPercentW = (width / (float)srcWidth);
            var nPercentH = (height / (float)srcHeight);

            var percent = nPercentH < nPercentW ? nPercentH : nPercentW;

            var newWidth = (int)(srcWidth * percent);
            var newHeight = (int)(srcWidth * percent);

            var b = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
            b.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using(var g = Graphics.FromImage(b))
            {
                g.Clear(Color.Transparent);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, new Rectangle(0, 0, newWidth, newHeight),
                                   new Rectangle(0, 0, srcWidth, srcHeight),
                                   GraphicsUnit.Pixel)
                ;
            }

            return b;
        }

        public static Image Crop(Image image, int x, int y, int width, int height)
        {
            Preconditions.CheckNotNull(image, "Source Image can not be null.");
            Preconditions.CheckNotEquals(0, width, "width can not be zero.");
            Preconditions.CheckNotEquals(0, height, "height can not be zero.");

            var bitMap = GetBitmap(image);

            var srcWidth = image.Width;
            var srcHeight = image.Height;

            // When crop size is bigger than original size, OutofMemory exception will throw
            if(width > srcWidth || height > srcHeight)
            {
                throw new ArgumentException("Crop size is bigger than original size.");
            }

            return bitMap.Clone(new Rectangle(x, y, width, height), bitMap.PixelFormat);
        }

        public static Image Rotate(Image image, RotateFlipType flipType)
        {
            Preconditions.CheckNotNull(image, "Source Image can not be null.");
            var bitMap = GetBitmap(image);
            var cloned = bitMap.Clone(new Rectangle(0, 0, image.Width, image.Height), bitMap.PixelFormat);
            cloned.RotateFlip(flipType);
            return cloned;
        }

        public static Image AppendTextTop(Image image, string text)
        {
            var bitmap = GetBitmap(image);
            var cloned = bitmap.Clone(new Rectangle(0, 0, image.Width, image.Height), bitmap.PixelFormat);

            using(var g = Graphics.FromImage(cloned))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                g.DrawString(text, new Font("Arial", 12), Brushes.White, new PointF(0, 0));
                return cloned;
            }
        }

        public static Bitmap GetBitmap(Image image)
        {
            var bitMap = image as Bitmap;
            if(bitMap == null)
            {
                throw new ArgumentException("Image can not be convert to Bitmap");
            }

            return bitMap;
        }

    }

}
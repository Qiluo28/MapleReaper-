using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using MapleReaper.Models;

namespace MapleReaper.Libraries
{
    internal static class OpenCV
    {
        public static bool TemplateMatch(Bitmap template, out Point location)
        {
            using var source = CaptureGameToBitmap(State.Size);
            return TemplateMatch(source, template, out location);
        }

        public static bool TemplateMatch(Bitmap template)
        {
            using var source = CaptureGameToBitmap(State.Size);
            return TemplateMatch(source, template);
        }

        public static bool TemplateMatch(Bitmap source, Bitmap template)
        {
            var sourceImage = source.ToImage<Gray, byte>();
            var templateImage = template.ToImage<Gray, byte>();
            var output = new Mat();
            CvInvoke.MatchTemplate(sourceImage, templateImage, output, TemplateMatchingType.SqdiffNormed);
            var minValue = 0.0;
            var maxValue = 0.0;
            var minLocation = new Point();
            var maxLocation = new Point();
            CvInvoke.MinMaxLoc(output, ref minValue, ref maxValue, ref minLocation, ref maxLocation);
            return minValue < 0.04;
        }

        public static bool TemplateMatch(Bitmap source, Bitmap template, out Point location)
        {
            var sourceImage = source.ToImage<Gray, byte>();
            var templateImage = template.ToImage<Gray, byte>();
            var output = new Mat();
            CvInvoke.MatchTemplate(sourceImage, templateImage, output, TemplateMatchingType.SqdiffNormed);
            var minValue = 0.0;
            var maxValue = 0.0;
            var minLocation = new Point();
            var maxLocation = new Point();
            CvInvoke.MinMaxLoc(output, ref minValue, ref maxValue, ref minLocation, ref maxLocation);
            if (minValue >= 0.04)
            {
                location = new Point(-1, -1);
                return false;
            }
            location = minLocation;
            return true;
        }

        public static Bitmap InRange(Bitmap source, Rgb rgb)
        {
            var sourceImage = source.ToImage<Rgb, byte>();
            return sourceImage.InRange(rgb, rgb).ToBitmap<Gray, byte>();
        }

        public static Bitmap CaptureGameToBitmap(Size size, Point point)
        {
            if (size.Width == 0 || size.Height == 0) return null;
            var bitmap = new Bitmap(size.Width, size.Height);
            using var graphic = Graphics.FromImage(bitmap);
            graphic.CopyFromScreen(State.X + point.X, State.Y + point.Y, 0, 0, size, CopyPixelOperation.SourceCopy);
            return bitmap;
        }

        public static Bitmap CaptureGameToBitmap(Size size)
        {
            if (size.Width == 0 || size.Height == 0) return null;
            var bitmap = new Bitmap(size.Width, size.Height);
            using var graphic = Graphics.FromImage(bitmap);
            graphic.CopyFromScreen(State.X, State.Y, 0, 0, size, CopyPixelOperation.SourceCopy);
            return bitmap;
        }
    }
}
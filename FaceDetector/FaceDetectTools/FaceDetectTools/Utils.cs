using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;

namespace FaceDetectTools
{
    static class Utils
    {
        public static OpenCvSharp.Size ConvertFdSizeToCvSize(FaceDetectTools.Size s)
        {
            return new OpenCvSharp.Size() { Width = s.Width, Height = s.Height };
        }

        public static void CropRect(Mat img, Mat dst, Rect r, double scale)
        {
            if (img == null || dst == null)
                throw new ArgumentNullException();

            Rect crop = new Rect();

            crop.Width = (int)(r.Width * scale);
            crop.Height = (int)(r.Height * scale);
            crop.X = r.X - (crop.Width - r.Width)/2;
            crop.Y = r.Y - (crop.Height - r.Height) / 2;

            if (crop.X < 0)
                crop.X = 0;
            if (crop.Y < 0)
                crop.Y = 0;
            if (crop.X + crop.Width > img.Width)
                crop.Width = img.Width - crop.X;
            if (crop.Y + crop.Height > img.Height)
                crop.Height = img.Height - crop.Y;

            using(Mat roi = new Mat(img, crop))
            {
                roi.CopyTo(dst);
            }
        }
    }
}

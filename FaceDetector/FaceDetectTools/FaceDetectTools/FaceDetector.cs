using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace FaceDetectTools
{
    public class FaceDetector : IDisposable
    {
        CascadeClassifier Classifier;

        public FaceDetector()
        {
            Classifier = null;
        }

        public FaceDetector(string classfierFileName)
        {
            Classifier = new CascadeClassifier(classfierFileName);
        }

        public List<Bitmap> Detect(Bitmap img, double FaceScale = 1.0, double scaleFactor = 1.1, int minNeighbors = 3, HaarDetectionType flags = 0, Size? minSize = null, Size? maxSize = null)
        {
            //メンバーと引数チェック
            if (Classifier == null)
                throw new ClassfierUnLoadedException();
            if (img == null)
                throw new ArgumentNullException();

            //引数の変換
            OpenCvSharp.Size? cvMinSize = null;
            if (minSize != null)
                cvMinSize = Utils.ConvertFdSizeToCvSize(minSize.Value);

            OpenCvSharp.Size? cvMaxSize = null;
            if (maxSize != null)
                cvMaxSize = Utils.ConvertFdSizeToCvSize(maxSize.Value);

            //処理
            List<Bitmap> dst = new List<Bitmap>();

            using (Mat src = BitmapConverter.ToMat(img))
            using (Mat gray = new Mat())
            {
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

                var faceRects = Classifier.DetectMultiScale(gray, scaleFactor, minNeighbors, (OpenCvSharp.HaarDetectionType)flags, cvMinSize, cvMaxSize);
                if (faceRects.Count() == 0)
                    return null;

                foreach(var r in faceRects)
                {
                    using (Mat face = new Mat())
                    {
                        Utils.CropRect(src, face, r, FaceScale);
                        dst.Add(BitmapConverter.ToBitmap(face));
                    }
                }
            }

            return dst;
        }

        public void LoadClassfier(string classfierFileName)
        {
            if (Classifier == null)
            {
                Classifier = new CascadeClassifier(classfierFileName);
            }
            else
            {
                Classifier.Load(classfierFileName);
            }
        }

        #region Dispose
        ~FaceDetector() => Dispose(false);

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                //マネージド
            }

            //アンマネージド
            Classifier?.Dispose();
        }
        #endregion
    }
}

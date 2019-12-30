using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using OpenCvSharp;
using FaceDetectTools;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load Bitmap
            Bitmap bmp = new Bitmap("./test.bmp");

            //FaceDetector
            FaceDetector fd = new FaceDetector("./haarcascade_frontalface_alt2.xml");
            var result = fd.Detect(bmp, 2.0);
            
            //Output Bitmap
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Save("./result/result_" + i + ".bmp");
            }
        }
    }
}

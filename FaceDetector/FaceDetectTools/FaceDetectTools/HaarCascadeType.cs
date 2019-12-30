using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenCvSharp;

namespace FaceDetectTools
{
    public enum HaarDetectionType
    {
        DoCunnyPruning = 1,
        ScaleImage = 2,
        FindBiggestObject = 4,
        DoRoughSearch = 8
    }
}

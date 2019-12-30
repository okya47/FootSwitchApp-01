using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceDetectTools
{
    public class ClassfierUnLoadedException : Exception
    {
        public ClassfierUnLoadedException() : base("分類器が設定されていません。コンストラクタあるいはLoadClassfierメソッドで分類器を読み込む必要があります。")
        {

        }

        public ClassfierUnLoadedException(string message): base(message)
        {

        }

        public ClassfierUnLoadedException(string message, Exception innnerException): base(message, innnerException)
        {

        }
    }
}

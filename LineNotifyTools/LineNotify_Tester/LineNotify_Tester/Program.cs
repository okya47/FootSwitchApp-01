using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using LineNotifyTools;

namespace LineNotify_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            //アクセストークンと共にコンストラクタ呼び出し
            string token = "vR4xywNu0dlRJZYMawuNDzWImzH42TGd4Xty5GhDWNH";
            LineNotifyClient client = new LineNotifyClient(token);

            #region GetMessage
            Console.Write("Message-->");

            #endregion

            //LineNotifyPayloadに送りたいデータを詰める
            //現在、テキストメッセージ（Message, 必須）と画像ファイル(ImageFile, 任意)に対応
            var message = Console.ReadLine();
            LineNotifyPayload payload = new LineNotifyPayload(message);  //Messageのセットは必須
                                                                         //引数無しでも作成できるが、セットを忘れると以降の処理に失敗する

            var imageFile = Bitmap.FromFile("./test.bmp");
            payload.ImageFile = imageFile;                              //Imageだけでなく、Imageを継承したBitmap等も可

            //payloadを渡してPOST＆結果受け取り（非同期）
            var task = client.PostMessageAsync(payload);
            var result = task.Result;

            #region Print

            Console.WriteLine(result);
            #endregion

            //破棄
            client.Dispose();

            //！！！！注意！！！！
            //メッセージを送信する度にLineNotifyClientの作成と破棄を繰り返さないこと！
            //
            //ソケットの作成と破棄により大きな遅延になる場合があります
            //可能な限り使いまわしてください
        }
    }
}

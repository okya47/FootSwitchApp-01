using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using LineNotifyTools;

namespace LINENotify_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            LineNotifyClient client = new LineNotifyClient();
            string token = "80bL6xBp173t3E4A42XWeeKgZdf16oMHHkpLGGBUaiU";   //小野さん
            //string token = "vR4xywNu0dlRJZYMawuNDzWImzH42TGd4Xty5GhDWNH"; //藤掛
            client.Token = token;

            LineNotifyPayload payload = new LineNotifyPayload();

            var dt = DateTime.Now;
            string message = "小野さん 来店\n前回来店日時: " + dt.ToString("MM月dd日HH時");
            message += "\n詳細: https://www.google.co.jp/";
            payload.Message = message;
            payload.ImageFile = Image.FromFile("./test.jpg");

            var post = client.PostMessageAsync(payload);
            var response = post.Result;

            Console.WriteLine(response);
        }
    }
}

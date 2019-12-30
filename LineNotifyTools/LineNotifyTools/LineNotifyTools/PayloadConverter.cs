using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace LineNotifyTools
{
    public static class PayloadConverter
    {
        public static MultipartFormDataContent PayloadToMultipartFromDataContent(LineNotifyPayload payload)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();

            if (payload.Message != null)
            {
                var message = new StringContent(payload.Message);
                message.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "message"
                };
                content.Add(message);
            }
            else
                throw new ArgumentNullException();

            if (payload.ImageFile != null)
            {
                var image = ImageToContent(payload.ImageFile);
                image.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "imageFile",
                    FileName = "image.png"
                };
                image.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                content.Add(image);
            }

            return content;
        }

        public static StringContent StringToContent(string str)
        {
            StringContent content = new StringContent(str ?? throw new ArgumentNullException());
            return content;
        }

        public static StreamContent BitmapToContent(Bitmap bmp)
        {
            if (bmp == null)
                return null;

            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            StreamContent content = new StreamContent(ms);

            return content;
        }

        public static StreamContent ImageToContent(Image img)
        {
            if (img == null)
                return null;

            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);
            StreamContent content = new StreamContent(ms);

            return content;
        }
    }
}

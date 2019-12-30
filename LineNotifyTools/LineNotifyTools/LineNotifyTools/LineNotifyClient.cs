using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace LineNotifyTools
{
    public class LineNotifyClient : IDisposable
    {
        public string LineNotifyEndPoint { get; private set; } = @"https://notify-api.line.me/api/notify";
        public string Token { get; set; }
        HttpClient client;

        public LineNotifyClient()
        {
            Token = null;
            client = new HttpClient();
        }

        public LineNotifyClient(string token)
        {
            Token = token ?? throw new ArgumentNullException();
            client = new HttpClient();
        }

        public async Task<string> PostMessageAsync(LineNotifyPayload payload)
        {
            try
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token ?? throw new ArgumentNullException());

                using (var response = await client.PostAsync(LineNotifyEndPoint, PayloadConverter.PayloadToMultipartFromDataContent(payload)))
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch(HttpRequestException re)
            {
                Console.WriteLine(re);
                return null;
            }
            catch
            {
                return null;
            }
        }

        #region Dispose()
        public bool IsDisposed { get; protected set; } = false;

        ~LineNotifyClient() => Dispose(false);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual  void Dispose(bool isDisposing)
        {
            if (IsDisposed)
                return;

            if (isDisposing)
            {
                client?.Dispose();
            }

            //アンマネ

            IsDisposed = true;
        }
        #endregion
    }
}

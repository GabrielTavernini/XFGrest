using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ModernHttpClient;

namespace XFGrest.Classes
{
    public class Utility //: HttpRequest
    {
        static public async Task<string> GetPageAsync(string url)
        {
            string pageSource;
            HttpRequestMessage getRequest = new HttpRequestMessage();
            getRequest.RequestUri = new Uri(url);
            //getRequest.Headers.Add("Cookie", HttpRequest.cookies);
            getRequest.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            getRequest.Headers.Add("UserAgent", "Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 63.0.3239.84 Safari / 537.36");

            HttpResponseMessage getResponse = await new HttpClient(new NativeMessageHandler()).SendAsync(getRequest);

            pageSource = await getResponse.Content.ReadAsStringAsync();

            getRequest.Dispose();
            getResponse.Dispose();

            return pageSource;
        }

        static public async Task<string> PostPageAsync(string url, string parms, string Referer)
        {
            HttpRequestMessage req = new HttpRequestMessage();
            Uri uri = new Uri(url);
            req.RequestUri = uri;
            req.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            //req.Headers.Add("Cookie", HttpRequest.cookies);
            req.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            req.Headers.Add("UserAgent", "Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 63.0.3239.84 Safari / 537.36");
            req.Headers.Add("Referer", Referer);
            req.Method = HttpMethod.Post;

            byte[] bytes = Encoding.UTF8.GetBytes(parms);
            req.Headers.TryAddWithoutValidation("Content-Length", bytes.Length.ToString());
            req.Content = new StringContent(parms, Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage resp = await new HttpClient(new NativeMessageHandler()).SendAsync(req);
            String pageSource = await resp.Content.ReadAsStringAsync();

            resp.Dispose();
            req.Dispose();
            return pageSource;
        }

        static public bool isConnectedToInternet()
        {
            try
            {
                Task<bool> t = Task.Run(async () =>
                {
                    using (HttpClient client = new HttpClient())
                    using (HttpResponseMessage response = await client.GetAsync(new Uri("http://www.google.com")))
                    using (HttpContent content = response.Content)
                    {
                        return true;
                    }
                });

                if (t.Wait(TimeSpan.FromSeconds(10)))
                    return t.Result;
                else
                    throw new Exception("Timed out");
            }
            catch { return false; }
        }

        static public string hex_md5(string input)

        {

            // step 1, calculate MD5 hash from input

            MD5 md5 = MD5.Create();

            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)

            {

                sb.Append(hash[i].ToString("x2"));

            }

            return sb.ToString();
        }

        public static String votoToString(float voto)
        {
            if (voto == 99.00)
            {
                return "";
            }
            else
            {
                int a = (int)voto;
                double Decimal = (10 * voto - 10 * a) / 10;

                if (Decimal == 0.25)
                {
                    return a + "+";
                }
                else if (Decimal == 0.50)
                {
                    return a + "\u00BD";
                }
                else if (Decimal == 0.75)
                {
                    return (a + 1) + "-";
                }

                return "" + a;
            }
        }
    }
}
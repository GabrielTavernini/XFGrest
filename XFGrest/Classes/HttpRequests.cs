using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ModernHttpClient;
using Supremes;
using Supremes.Nodes;
using Xamarin.Forms;
namespace XFGrest.Classes
{
    public class HttpRequests
    {
        private static String cookies;
        private static String indexUrl = "http://192.168.0.101/grest/index.php";
        public HttpRequests()
        {
        }

        public static async Task<Boolean> LoginAsync(String lab, String pass)
        {
            App.Users = new List<User>();

            if (cookies == null || cookies == "")
                if (await getCookiesAsync() == "failed")
                    return false;

            try
            {
                string formParams = "username=admin&password=password&sub=";//String.Format("username={0}&password={1}&sub=", lab.ToLower(), pass);

                HttpRequestMessage req = new HttpRequestMessage();
                Uri uri = new Uri(indexUrl);
                req.RequestUri = uri;
                req.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                req.Headers.Add("Cookie", cookies);
                req.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                req.Headers.Add("UserAgent", "Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 63.0.3239.84 Safari / 537.36");
                req.Headers.Add("Referer", "http://192.168.0.100/grest/index.php");
                req.Method = HttpMethod.Post;

                byte[] bytes = Encoding.UTF8.GetBytes(formParams);
                req.Headers.TryAddWithoutValidation("Content-Length", bytes.Length.ToString());
                req.Content = new StringContent(formParams, Encoding.UTF8, "application/x-www-form-urlencoded");


                HttpResponseMessage resp = await new HttpClient(new NativeMessageHandler()).SendAsync(req);
                String html = await resp.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(html);
                resp.Dispose();
                req.Dispose();
                extratMarks(html);
                return true;
            }
            catch
            {
                cookies = null;
                return false;
            }
        }

        public static async Task<string> getCookiesAsync()
        {
            string cookieHeader = "";
            HttpRequestMessage req = new HttpRequestMessage();
            req.RequestUri = new Uri(indexUrl);

            HttpResponseMessage resp = new HttpResponseMessage();
            resp = await new HttpClient(new NativeMessageHandler()).SendAsync(req);
            HttpHeaders headers = resp.Headers;
            IEnumerable<string> values;
            if (headers.TryGetValues("Set-Cookie", out values))
            {
                cookieHeader = values.First();
            }

            String[] temp = cookieHeader.Split(';');
            cookies = temp[0];

            req.Dispose();
            resp.Dispose();
            System.Diagnostics.Debug.WriteLine("<--------------------------------Cookies--------------------------------->");
            System.Diagnostics.Debug.WriteLine(cookies);
            return cookies;
        }

        public static void extratMarks(String html)
        {
            System.Diagnostics.Debug.WriteLine(html);
            Document doc = Dcsoup.ParseBodyFragment(html, "");

            User currentUser = new User(false);

            int Column = 0;
            for (int i = 1; ; i++)
            {
                Supremes.Nodes.Element table = doc.Select("body > div.content > div.scrolling-content > table > tbody > tr:nth-child(" + i + ")").First;

                if (table == null)
                    break;


                Elements inputElements = table.GetElementsByTag("td");
                foreach (Supremes.Nodes.Element inputElement in inputElements)
                {
                    if (Column == 0)
                    {
                        currentUser = new User(true);
                        currentUser.name = inputElement.Text;
                        System.Diagnostics.Debug.WriteLine(inputElement.Text);
                        Column++;
                    }
                    else if (Column == 1)
                    {
                        currentUser.eta = Convert.ToInt16(inputElement.Text);
                        Column++;
                    }
                    else if (Column == 2)
                    {
                        currentUser.lab = inputElement.Text;
                        Column++;
                    }
                    else if (Column == 3)
                    {
                        if ("1".Equals(inputElement.Text))
                            currentUser.presences[0] = true;
                        else
                            currentUser.presences[0] = false;

                        Column++;
                    }
                    else if (Column == 4)
                    {
                        currentUser.sports[0] = inputElement.Text;
                        Column++;
                    }
                    else if (Column == 5)
                    {
                        if ("1".Equals(inputElement.Text))
                            currentUser.presences[1] = true;
                        else
                            currentUser.presences[1] = false;

                        Column++;
                    }
                    else if (Column == 6)
                    {
                        currentUser.sports[1] = inputElement.Text;
                        Column++;
                    }
                    else if (Column == 7)
                    {
                        if ("1".Equals(inputElement.Text))
                            currentUser.presences[2] = true;
                        else
                            currentUser.presences[2] = false;

                        Column++;
                    }
                    else if (Column == 8)
                    {
                        currentUser.sports[2] = inputElement.Text;
                        Column++;
                    }
                    else if (Column == 9)
                    {
                        if ("1".Equals(inputElement.Text))
                            currentUser.presences[3] = true;
                        else
                            currentUser.presences[3] = false;

                        Column++;
                    }
                    else if (Column == 10)
                    {
                        currentUser.sports[3] = inputElement.Text;
                        Column = 0;
                    }

                }
            }
        }
    
        public static async void getTables()
        {
            HttpClient client = new HttpClient(new NativeMessageHandler());

            using (client)
            {
                using (HttpResponseMessage response = await client.GetAsync(indexUrl.Replace("index.php", "tables.php")))
                {
                    using (HttpContent content = response.Content)
                    {
                        var page = await content.ReadAsStringAsync();
                        extractTables(page);
                    }
                }
            }
        }

        public static void extractTables(String html)
        {
            System.Diagnostics.Debug.WriteLine(html);
            Document doc = Dcsoup.ParseBodyFragment(html, "");

            
            for (int i = 2; ; i++)
            {
                Supremes.Nodes.Element table = doc.Select("body > div:nth-child(1) > table:nth-child(1) > tbody > tr:nth-child(" + i + ")").First;

                if (table == null)
                    break;


                Elements inputElements = table.GetElementsByTag("td");
                System.Diagnostics.Debug.WriteLine(inputElements[1].Text);
                App.dates[i - 2] = DateTime.ParseExact(inputElements[1].Text, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            }

            for (int i = 2; ; i++)
            {
                Supremes.Nodes.Element table = doc.Select("body > div:nth-child(1) > table:nth-child(2) > tbody > tr:nth-child(" + i + ")").First;

                if (table == null)
                    break;


                Elements inputElements = table.GetElementsByTag("td");
                System.Diagnostics.Debug.WriteLine(inputElements[1].Text);
                App.labs[i - 2] = inputElements[1].Text;
            }
        }
    }
}

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
        public static int lab;
        public static String pass;

        private static String cookies;
        private static String indexUrl = "http://192.168.0.101/RegistroGrest/index.php";

        static async Task<String> LoginAsync()
        {
            App.Users = new List<User>();

            if (cookies == null || cookies == "")
                if (await getCookiesAsync() == "failed")
                    return "failed";

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
                resp.Dispose();
                req.Dispose();
                return html;
            }
            catch
            {
                cookies = null;
                return "failed";
            }
        }

        public static async Task<Boolean> extaractUsers()
        {
            String generalPage = "";
            if(cookies == "fsgfs")
            {
                generalPage = await LoginAsync();
                System.Diagnostics.Debug.WriteLine(generalPage);
                if (generalPage == "failed")
                    return false;
            }else{
                generalPage = await getGeneral();
                System.Diagnostics.Debug.WriteLine(generalPage);
                if (generalPage == "failed")
                    return false;
            }

            extratMarks(generalPage);
            return true;
        }

        public static async Task<Boolean> PostUserEdit(User user)
        {
            if(cookies == null)
                await LoginAsync();
            
            try{
                Uri uri = new Uri(indexUrl.Replace("index.php", String.Format("edit.php?nome={0}&eta={1}", user.name, user.eta)));
                string formParams = String.Format("nome={0}&eta={1}&lab={2}&pre1={3}&sport1={4}&pre2={5}&sport2={6}&pre3={7}&sport3={8}&pre4={9}&sport4={10}&update=", 
                                              user.name,
                                              user.eta,
                                              user.lab,
                                              Convert.ToInt16(user.presences[0]),
                                              user.sports[0],
                                              Convert.ToInt16(user.presences[1]),
                                              user.sports[1],
                                              Convert.ToInt16(user.presences[2]),
                                              user.sports[2],
                                              Convert.ToInt16(user.presences[3]),
                                              user.sports[3]);

            
                HttpRequestMessage req = new HttpRequestMessage();
                req.RequestUri = uri;
                req.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                req.Headers.Add("Cookie", cookies);
                req.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                req.Headers.Add("UserAgent", "Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 63.0.3239.84 Safari / 537.36");
                req.Headers.Add("Referer", "http://localhost/grest/edit.php?nome=Denis&eta=16");
                req.Method = HttpMethod.Post;

                byte[] bytes = Encoding.UTF8.GetBytes(formParams);
                req.Headers.TryAddWithoutValidation("Content-Length", bytes.Length.ToString());
                req.Content = new StringContent(formParams, Encoding.UTF8, "application/x-www-form-urlencoded");


                HttpResponseMessage resp = await new HttpClient(new NativeMessageHandler()).SendAsync(req);
                resp.Dispose();
                req.Dispose();
                return true;
            }
            catch
            {
                cookies = null;
                return false;
            }
        }

        static async Task<string> getCookiesAsync()
        {
            try
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
                return cookies;            
            }
            catch { return "failed"; }
        }

        static void extratMarks(String html)
        {
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
                        currentUser.lab = Convert.ToInt32(inputElement.Attr("value"));
                        Column++;
                    }
                    else if (Column == 3)
                    {
                        if (Convert.ToInt32(inputElement.Attr("value")) == 1)
                            currentUser.presences[0] = true;
                        else
                            currentUser.presences[0] = false;

                        Column++;
                    }
                    else if (Column == 4)
                    {
                        currentUser.sports[0] = Convert.ToInt32(inputElement.Attr("value"));
                        Column++;
                    }
                    else if (Column == 5)
                    {
                        if (Convert.ToInt32(inputElement.Attr("value")) == 1)
                            currentUser.presences[1] = true;
                        else
                            currentUser.presences[1] = false;

                        Column++;
                    }
                    else if (Column == 6)
                    {
                        currentUser.sports[1] = Convert.ToInt32(inputElement.Attr("value"));
                        Column++;
                    }
                    else if (Column == 7)
                    {
                        if (Convert.ToInt32(inputElement.Attr("value")) == 1)
                            currentUser.presences[2] = true;
                        else
                            currentUser.presences[2] = false;

                        Column++;
                    }
                    else if (Column == 8)
                    {
                        currentUser.sports[2] = Convert.ToInt32(inputElement.Attr("value"));
                        Column++;
                    }
                    else if (Column == 9)
                    {
                        if (Convert.ToInt32(inputElement.Attr("value")) == 1)
                            currentUser.presences[3] = true;
                        else
                            currentUser.presences[3] = false;

                        Column++;
                    }
                    else if (Column == 10)
                    {
                        currentUser.sports[3] = Convert.ToInt32(inputElement.Attr("value"));
                        Column = 0;
                    }

                }
            }
        }
    
        public static async Task<Boolean> getTables()
        {
            HttpClient client = new HttpClient(new NativeMessageHandler());

            try
            {
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

                return true;
            }
            catch { return false; }

        }

        static void extractTables(String html)
        {
            Document doc = Dcsoup.ParseBodyFragment(html, "");

            App.dates = new List<DateTime>();
            App.labs = new List<string>();
            App.sports = new List<string>();

            System.Diagnostics.Debug.WriteLine("Dates:");
            Supremes.Nodes.Element table = doc.Select("body > div:nth-child(2)").First;
            if(table != null)
            {
                string[] pairs = table.Text.Split(';');
                foreach(string s in pairs)
                {
                    if (s != "" && s != null)
                    {
                        int i = Convert.ToInt16(s.Split(':').First());
                        App.dates.Add(DateTime.ParseExact(s.Split(':').Last(), "yyyy-MM-dd", CultureInfo.InvariantCulture));
                        System.Diagnostics.Debug.WriteLine(s);
                    }
                }
            }


            System.Diagnostics.Debug.WriteLine("Labs:");
            table = doc.Select("body > div:nth-child(3)").First;
            if (table != null)
            {
                string[] pairs = table.Text.Split(';');
                foreach (string s in pairs)
                {
                    if (s != "" && s != null)
                    {
                        int i = Convert.ToInt16(s.Split(':').First());
                        App.labs.Add(s.Split(':').Last());
                        System.Diagnostics.Debug.WriteLine(s);
                    }
                }
            }


            System.Diagnostics.Debug.WriteLine("Sports:");
            table = doc.Select("body > div:nth-child(4)").First;
            if (table != null)
            {
                string[] pairs = table.Text.Split(';');
                foreach (string s in pairs)
                {
                    if(s != "" && s != null)
                    {
                        int i = Convert.ToInt16(s.Split(':').First());
                        App.sports.Add(s.Split(':').Last());
                        System.Diagnostics.Debug.WriteLine(s);
                    }
                }
            }

        }

        public static async Task<String> getGeneral()
        {
            HttpClient client = new HttpClient(new NativeMessageHandler());

            try
            {
                using (client)
                {
                    using (HttpResponseMessage response = await client.GetAsync(indexUrl.Replace("index.php", "general.php")))
                    {
                        using (HttpContent content = response.Content)
                        {
                            var page = await content.ReadAsStringAsync();
                            return page;
                        }
                    }
                }
            }
            catch { cookies = null; return "failed"; }
        }
    } 
}
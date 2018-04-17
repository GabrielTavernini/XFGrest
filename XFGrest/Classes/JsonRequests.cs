using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace XFGrest.Classes
{
    public class JsonRequests
    {
        static public int labID;
        static public String password;
        static private JObject dati;
        static private JObject datiTables;
        
        static public async Task<bool> getUsers()
        {
            try
            {
                String querryUrl = "http://192.168.0.102/RegistroGrest/app/jsonlogin.php?username=" + labID
                + "&password=" + password;
                Debug.WriteLine(querryUrl);
                String json = await Utility.GetPageAsync(querryUrl);
                Debug.WriteLine(json);
                dati = JObject.Parse(json);
            }
            catch(Exception e){
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                return false;
            }

            App.Users = decodeUsers();
            return true;
        }

        static public async Task<bool> getTables()
        {
            try
            {
                String querryUrl = "http://192.168.0.102/RegistroGrest/tables.php";
                Debug.WriteLine(querryUrl);
                String json = await Utility.GetPageAsync(querryUrl);
                Debug.WriteLine(json);
                datiTables = JObject.Parse(json);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                return false;
            }

            decodeTables();
            return true;
        }

        static private bool decodeTables()
        {
            JArray date, sports, labs;

            try
            {
                App.Day = Convert.ToInt16(datiTables["today"].ToString());

                sports = JArray.Parse(datiTables["sports"].ToString());
                date = JArray.Parse(datiTables["dates"].ToString());
                labs = JArray.Parse(datiTables["labs"].ToString());

                App.labs = new List<string>();
                App.dates = new List<DateTime>();
                App.sports = new List<string>();

                for (int i = 0; i < labs.Count; i++)
                {
                    App.labs.Add(labs[i].ToString());
                }
                App.labs.Add("Admin");

                for (int i = 0; i < date.Count; i++)
                {
                    App.dates.Add(DateTime.ParseExact(date[i].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture) );
                }

                for (int i = 0; i < sports.Count; i++)
                {
                    App.sports.Add(sports[i].ToString());
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        static private List<User> decodeUsers()
        {
            List<User> users = new List<User>();

            JArray names, eta, labs, p1, s1, p2, s2, p3, s3, p4, s4;

            try
            {
                names = JArray.Parse(dati["names"].ToString());
                eta = JArray.Parse(dati["eta"].ToString());
                labs = JArray.Parse(dati["labs"].ToString());
                p1 = JArray.Parse(dati["pre1"].ToString());
                s1 = JArray.Parse(dati["spo1"].ToString());
                p2 = JArray.Parse(dati["pre2"].ToString());
                s2 = JArray.Parse(dati["spo2"].ToString());
                p3 = JArray.Parse(dati["pre3"].ToString());
                s3 = JArray.Parse(dati["spo3"].ToString());
                p4 = JArray.Parse(dati["pre4"].ToString());
                s4 = JArray.Parse(dati["spo4"].ToString());


                for (int i = 0; i < names.Count; i++)
                {
                    User user = new User(true);
                    user.name = names[i].ToString();
                    user.eta = Convert.ToInt32(eta[i].ToString());
                    user.lab = Convert.ToInt32(labs[i].ToString());
                    user.presences[0] = !p1[i].ToString().Equals("0");
                    user.sports[0] = Convert.ToInt32(s1[i].ToString());
                    user.presences[1] = !p2[i].ToString().Equals("0");
                    user.sports[1] = Convert.ToInt32(s2[i].ToString());
                    user.presences[2] = !p3[i].ToString().Equals("0");
                    user.sports[2] = Convert.ToInt32(s3[i].ToString());
                    user.presences[3] = !p4[i].ToString().Equals("0");
                    user.sports[3] = Convert.ToInt32(s4[i].ToString());
                    users.Add(user);

                    Debug.WriteLine(user.name);
                }
            }
            catch (Exception e)
            {
                
            }

            return users;
        }


        static public async Task<bool> PostUserEdit(User u)
        {
            if (u == null)
                return false;

            try
            {
                String querryUrl = String.Format("http://192.168.0.102/RegistroGrest/app/appedit.php?username={0}" +
                                                 "&password={1}&Nome={2}&Eta={3}&Lab={4}&P1={5}&S1={6}&P2={7}&S2={8}" +
                                                 "&P3={9}&S3={10}&P4={11}&S4={12}", labID, password, u.name, u.eta,
                                                 u.lab, u.presences[0] ? 1 : 0, u.sports[0] , u.presences[1]? 1 : 0, 
                                                 u.sports[1], u.presences[2] ? 1 : 0, u.sports[2], u.presences[3] ? 1 : 0,
                                                 u.sports[3]);
                Debug.WriteLine(querryUrl);
                await Utility.GetPageAsync(querryUrl);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                return false;
            }

            return true;
        }
    }
}

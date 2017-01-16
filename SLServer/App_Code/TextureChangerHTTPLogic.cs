using System.Collections.Specialized;
using System.Net;
using System;

namespace TextureChanger
{
    public class HTTPLogic
    {
        public HTTPLogic()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static byte[] Post(string uri, NameValueCollection pairs)
        {
            byte[] response = null;
            using (WebClient client = new WebClient())
            {
                response = client.UploadValues(uri, pairs);
            }
            return response;
        }

        public static string getCurrentTexture(string _url)
        {
            string x = "Error";

            try
            {
                var response = HTTPLogic.Post(_url, new NameValueCollection() { { "action", "getcurrenttheme" } });
                x = System.Text.Encoding.UTF8.GetString(response);
            }
            catch
            {

            }

            return x;
        }

        public static string[] getAllThemes(string _url)
        {
            string[] retValue = { };

            try
            {
                var response = HTTPLogic.Post(_url, new NameValueCollection() { { "action", "getallthemes" } });
                retValue = System.Text.Encoding.UTF8.GetString(response).Split(',');
            }
            catch
            {

            }

            return retValue;
        }

        public static int isRented(string _url)
        {
            int rented = 2;

            try
            {
                var response = HTTPLogic.Post(_url, new NameValueCollection() { { "action", "isrented" } });

                string x = System.Text.Encoding.UTF8.GetString(response);

                if (x.ToLower() == "no unit linked" || x.ToLower() == string.Empty)
                {
                    rented = 0;
                }
                else
                {
                    string[] split = x.Split('|');

                    if (Decimal.Parse(split[1].ToString()) == 0)
                    {
                        rented = 1;
                    }
                }
            }
            catch
            {
                rented = -1;
            }

            return rented;
        }

        public static string setTheme(string _themeName, string _url)
        {
            string x = "Error";

            try
            {
                var response = HTTPLogic.Post(_url, new NameValueCollection() { { "action", "setcurrenttheme" }, { "texturename", _themeName } });

                x = System.Text.Encoding.UTF8.GetString(response);
            }
            catch
            {

            }
            return x;
        }

        public static string setNearbyTheme(string _nearbyObjectGUID, string _url, string _themeName)
        {
            string x = "Error";

            try
            {
                var response = HTTPLogic.Post(_url, new NameValueCollection() { { "action", "setnearbyobjecttheme" }, { "nearbyguid", _nearbyObjectGUID }, { "texturename", _themeName } });

                x = System.Text.Encoding.UTF8.GetString(response);
            }
            catch
            {

            }
            return x;
        }
    }
}
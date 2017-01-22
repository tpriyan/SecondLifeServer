using System.Collections.Specialized;
using System.Net;
using System;

namespace TextureChanger
{
    public class UnitDetails
    {
        public string rented;
        public string currentTexture;
        public string[] themesList;

        public UnitDetails()
        {
            rented = "Error";
            currentTexture = "Error";
        }
         
    }

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

        // Format - RENTED|CURRENTTHEME|THEME1,THEME2,THEME3
        public static UnitDetails getAllDetails(string _url)
        {
            UnitDetails unitDetails = new UnitDetails();

            string x = string.Empty;

            int rented = 2;

            try
            {
                var response = HTTPLogic.Post(_url, new NameValueCollection() { { "action", "getalldetails" } } );

                x = System.Text.Encoding.UTF8.GetString(response);

                string[] splitValue = x.Split(',');

                if (splitValue[0].ToLower() == "no unit linked" || splitValue[0] == string.Empty)
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

                switch (rented)
                {
                    case 0:
                        unitDetails.rented = "Not linked";
                        break;
                    case 1:
                        unitDetails.rented = "Not Rented";
                        break;
                    case 2:
                        unitDetails.rented = "Rented";
                        break;
                    default:
                        unitDetails.rented = "Error";
                        break;

                }

                unitDetails.currentTexture = splitValue[1];

                unitDetails.themesList = splitValue[2].Split(';');

               // if(unitDetails.themesList = null)

            }
            catch
            {
            }

            return unitDetails;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TextureChangerSessionHandler
/// </summary>
namespace TextureChanger
{
    public class SessionHandler
    {
        public static Boolean IsSessionValid(System.Web.SessionState.HttpSessionState _sessionState, System.Web.HttpResponse _response)
        {
            if(_sessionState["ownerid"] == null)
            {
                InitState(_sessionState);
            }
            else
            {
                DateTime dt = DateTime.Parse(_sessionState["lastactivetime"].ToString());

                if(DateTime.UtcNow > dt.AddMinutes(5) )
                         
                {
                    _sessionState["ownerid"] = null;
                    _sessionState["logintime"] = null;
                    _sessionState["lastactivetime"] = null;
                    //_response.Redirect("Login.aspx");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public static void InitState(System.Web.SessionState.HttpSessionState _sessionState)
        {
            _sessionState["ownerid"] = "";
            _sessionState["logintime"] = DateTime.UtcNow;
            _sessionState["lastactivetime"] = DateTime.UtcNow;
        }

        public static Boolean login(string _username, string password, System.Web.SessionState.HttpSessionState _sessionState)
        {
            string sqlQueryRead = "select * from users where username = '{0}' and password = '{1}'";

            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + HttpContext.Current.Server.MapPath("~/App_Data/" + TextureChanger.Variables.DatabaseName)))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();
                    com.CommandText = String.Format(sqlQueryRead, _username, password);
                    using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            _sessionState["ownerid"] = reader["ownerid"].ToString();
                            _sessionState["logintime"] = DateTime.UtcNow;
                            _sessionState["lastactivetime"] = DateTime.UtcNow;
                            return true;  
                        }
                    }
                }
            }

            return false;
        }
    }
}
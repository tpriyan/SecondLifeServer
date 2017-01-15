using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TextureChangesSettingsHandler
/// </summary>
namespace TextureChanger
{
    public class Settings
    {
        public static void setSettings(Boolean skipSkyboxThemesFetch,
                                Boolean skipFetchThemeDataForRentedBoxes,
                                Boolean skipFetchCurrentTheme,
                                string themes)
        {
            string sql = "update GlobalSettings set skipSkyboxThemesFetch = '{0}', skipFetchThemeDataForRentedBoxes =  '{1}', skipFetchCurrentTheme = '{2}', themes = '{3}'";

            sql = string.Format(sql, skipSkyboxThemesFetch, skipFetchThemeDataForRentedBoxes, skipFetchCurrentTheme, themes);

            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + HttpContext.Current.Server.MapPath("~/App_Data/" + TextureChanger.Variables.DatabaseName)))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();
                    com.CommandText = sql;
                    com.ExecuteNonQuery();
                }
            }
        }

        public static GlobalSettings getSettings()
        {
            string sql = "select * from GlobalSettings";
            GlobalSettings s = new GlobalSettings();
            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + HttpContext.Current.Server.MapPath("~/App_Data/" + TextureChanger.Variables.DatabaseName)))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();
                    com.CommandText = sql;
                    using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            s.skipSkyboxThemesFetch = Boolean.Parse(reader["skipSkyboxThemesFetch"].ToString());
                            s.skipFetchThemeDataForRentedBoxes = Boolean.Parse(reader["skipFetchThemeDataForRentedBoxes"].ToString());
                            s.skipFetchCurrentTheme = Boolean.Parse(reader["skipFetchCurrentTheme"].ToString());
                            s.themes = reader["themes"].ToString();
                        }
                    }
                }
            }

            return s;
        }



    }

    public class GlobalSettings
    {
        public Boolean skipSkyboxThemesFetch;
        public Boolean skipFetchThemeDataForRentedBoxes;
        public Boolean skipFetchCurrentTheme;
        public string themes;
    }
}
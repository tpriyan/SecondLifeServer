using System;
using System.Web;
using TextureChanger.BaseTypes;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TextureChanger.Logic
{
    public class DataBaseStaging
    {
        private static DataTable DataTableLinkedUnits()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("ObjectGUID");
            dt.Columns.Add("LinkedObjectGUID");
            dt.Columns.Add("TextureName");
            dt.Columns.Add("ObjectName");
            dt.Columns.Add("ObjectType");


            return dt;
        }

        private static DataTable DataTableUnits()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("ObjectGUID");
            dt.Columns.Add("URL");
            dt.Columns.Add("Name");
            dt.Columns.Add("Type");
            dt.Columns.Add("Owner");
            dt.Columns.Add("CurrentTheme");
            dt.Columns.Add("RentedStatus");
            dt.Columns.Add("ThemesAvailable");
            dt.Columns.Add("DefTheme");
            dt.Columns.Add("Version");

            return dt;
        }

        public static  DataTable GetUnitsDataTable(System.Web.SessionState.HttpSessionState _sessionState)
        {
            //GlobalSettings settings = Settings.getSettings();
            DataTable dt = ReadData(_sessionState);

            foreach (DataRow dr in dt.Rows)
            {
                UnitDetails details = HTTPLogic.getAllDetails(dr["URL"].ToString());

                dr["CurrentTheme"] = details.currentTexture;
                dr["RentedStatus"] = details.rented;
                if(details.themesList != null)
                    dr["ThemesAvailable"] = string.Join(",", details.themesList); 
            }

            return dt;
        }

        #region ObsoleteCode
        /*public static async Task<Boolean> LoadDataAsync(System.Web.UI.WebControls.GridView gridView, System.Web.SessionState.HttpSessionState _sessionState)
        {
            //GlobalSettings settings = Settings.getSettings();
            DataTable dt = ReadData(_sessionState);
            List<Task> TaskList = new List<Task>();

            foreach (DataRow dr in dt.Rows)
            {
                    var LastTask = Task.Run(() => doFetchInfo(dr, settings));
                    TaskList.Add(LastTask);
            }

            Task.WaitAll(TaskList.ToArray());


            gridView.DataSource = dt;
            gridView.DataBind();
            

            return true;
        }

        public static void doFetchInfo(DataRow dr, GlobalSettings settings)
        {
            
            Int32 rented = 0;
            rented = TextureChanger.HTTPLogic.isRented(dr["URL"].ToString());
            switch (rented)
            {
                case 0:
                    dr["RentedStatus"] = "Not linked";
                    break;
                case 1:
                    dr["RentedStatus"] = "Not Rented";
                    break;
                case 2:
                    dr["RentedStatus"] = "Rented";
                    break;
                default:
                    dr["RentedStatus"] = "Error";
                    break;

            }

            string[] tmp;
            if (settings.skipSkyboxThemesFetch)
            {
                tmp = settings.themes.Split(',');
            }
            else
            {
                tmp = TextureChanger.HTTPLogic.getAllThemes(dr["URL"].ToString());
            }
            dr["ThemesAvailable"] = string.Join(",", tmp);


            if (settings.skipFetchCurrentTheme || (settings.skipFetchThemeDataForRentedBoxes && (rented == 2)))
            {
               
                dr["CurrentTheme"] = "Skipped in settings";
            }
            else
                dr["CurrentTheme"] = TextureChanger.HTTPLogic.getCurrentTexture(dr["URL"].ToString());

            }*/

        #endregion

        public static DataTable ReadData(System.Web.SessionState.HttpSessionState _sessionState, string ownerId = "")
        {
            string sqlQueryRead = TextureChanger.Constants.QueryReadAll;
            System.Data.DataTable dataTable = DataTableUnits();
            System.Data.DataRow dataRow = null;

            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + HttpContext.Current.Server.MapPath("~/App_Data/" + TextureChanger.Variables.DatabaseName)))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();
                    if (ownerId != "")
                    {
                        com.CommandText = String.Format(sqlQueryRead, ownerId);
                    }
                    else
                    {
                        com.CommandText = String.Format(sqlQueryRead, _sessionState["ownerid"].ToString());
                    }
                    using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dataRow = dataTable.NewRow();
                            dataRow[0] = reader.GetValue(0);
                            dataRow[1] = reader.GetValue(1);
                            dataRow[2] = reader.GetValue(2);
                            dataRow[3] = reader.GetValue(3);
                            if(reader["DefTheme"] != null)
                                dataRow["DefTheme"] = reader["DefTheme"].ToString();
                            dataRow["Version"] = reader["Version"];
                            dataTable.Rows.Add(dataRow);
                        }
                    }
                }
            }

            return dataTable;
        }

        public static DataTable readLinkedData(string _objectGUID, string databasePath = "")
        {
            DataTable dt = DataTableLinkedUnits();

            string sqlQueryRead = "select * from InWorldLinkedObjects where ObjectGuid = '{0}'";

            System.Data.DataRow dataRow = null;
            if(databasePath == "")
            {
                databasePath = HttpContext.Current.Server.MapPath("~/App_Data/");
            }
            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + databasePath + TextureChanger.Variables.DatabaseName))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();
                    com.CommandText = String.Format(sqlQueryRead, _objectGUID);
                    using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dataRow = dt.NewRow();
                            dataRow[0] = reader.GetValue(0);
                            dataRow[1] = reader.GetValue(1);
                            dataRow[2] = reader.GetValue(2);
                            dataRow[3] = reader.GetValue(3);
                            dataRow[4] = reader.GetValue(4);


                            dt.Rows.Add(dataRow);
                        }
                    }
                }
            }

            return dt;
        }
    }
    
    public class BulkOperations
    {
        public static Boolean bulkSetThemeUnrentedAsync(System.Web.SessionState.HttpSessionState _sessionState, string _themeName = "")
        {

            DataTable table = DataBaseStaging.ReadData(_sessionState);

            List<Task> TaskList = new List<Task>();

            //  TaskList.Add(LastTask);
            string path = HttpContext.Current.Server.MapPath("~/App_Data/");

            //ask.WaitAll(TaskList.ToArray());

            foreach (DataRow dr in table.Rows)
            {
                DataTable linkedUnits = null;

                UnitDetails details = HTTPLogic.getAllDetails(dr["URL"].ToString());
                if (details.rented == "Not Rented") // not rented
                {
                    if (_themeName == "")
                    {
                        if (dr["DefTheme"] != null)
                            _themeName = dr["DefTheme"].ToString();
                    }

                    if (_themeName != "")
                        HTTPLogic.setTheme(_themeName, dr["URL"].ToString());

                    // reset the nearby objects
                    linkedUnits = DataBaseStaging.readLinkedData(dr[0].ToString(), path);
                    foreach (DataRow dr1 in linkedUnits.Rows)
                    {
                        if (dr1["ObjectType"].ToString() == "Scenery")
                        {
                            HTTPLogic.setNearbyScenery(dr1["LinkedObjectGUID"].ToString(), dr["URL"].ToString(), dr1["TextureName"].ToString());
                        }
                        else if (dr1["ObjectType"].ToString() == "Multiscene")
                        {
                            HTTPLogic.setNearbyMultiScene(dr1["LinkedObjectGUID"].ToString(), dr["URL"].ToString(), dr1["TextureName"].ToString());
                        }
                    }

                }
            }

            // Task.WaitAll(TaskList.ToArray());

            return true;
        }

        /*public static void doTask(DataRow dr, string _themeName, string path)
        {
            DataTable linkedUnits = null;

            UnitDetails details = HTTPLogic.getAllDetails(dr["URL"].ToString());
            if (details.rented == "Not Rented") // not rented
            {
                if (_themeName == "")
                {
                    if(dr["DefTheme"] != null)
                        _themeName = dr["DefTheme"].ToString();
                }
                    
                if(_themeName != "")
                    HTTPLogic.setTheme(_themeName, dr["URL"].ToString());

                // reset the nearby objects
                linkedUnits = DataBaseStaging.readLinkedData(dr[0].ToString(), path);
                foreach (DataRow dr1 in linkedUnits.Rows)
                {
                    if (dr["ObjectType"].ToString() == "Scenery")
                    {
                        HTTPLogic.setNearbyScenery(dr1["LinkedObjectGUID"].ToString(), dr["URL"].ToString(), dr1["TextureName"].ToString());
                    }
                    else if(dr["ObjectType"].ToString() == "Multiscene")
                    {
                        HTTPLogic.setNearbyMultiScene(dr1["LinkedObjectGUID"].ToString(), dr["URL"].ToString(), dr1["TextureName"].ToString());
                    }
                }

            }
        }*/

        #region ObsoleteCode
        /*public static void bulkSetThemeUnrented(System.Web.SessionState.HttpSessionState _sessionState, string _themeName = "")
        {
            DataTable table = DataBaseStaging.ReadData(_sessionState);
            DataTable linkedUnits = null;

            foreach (DataRow dr in table.Rows)
            {
                UnitDetails details = HTTPLogic.getAllDetails(dr["URL"].ToString());
                if (details.rented == "Not Rented") // not rented
                { 
                    if (_themeName == "")
                        _themeName = dr["DefTheme"].ToString();
                    HTTPLogic.setTheme(_themeName, dr["URL"].ToString());

                    // reset the nearby objects
                    linkedUnits = DataBaseStaging.readLinkedData(dr[0].ToString());
                    foreach (DataRow dr1 in linkedUnits.Rows)
                    {
                        if (dr1["ObjectType"].ToString() == "Scenery")
                        {
                            HTTPLogic.setNearbyScenery(dr1[1].ToString(), dr["URL"].ToString(), dr1[2].ToString());
                        }
                        else if (dr1["ObjectType"].ToString() == "Multiscene")
                        {
                            HTTPLogic.setNearbyMultiScene(dr1[1].ToString(), dr["URL"].ToString(), dr1[2].ToString());
                        }
                    }

                }
            }

        }*/
        #endregion
    }

    public class CRUDOperations
    {

        public static string getUnitsList(string ownerid)
        {
            DataTable dt = DataBaseStaging.ReadData(null, ownerid);
            string unitsList = String.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                if (unitsList == "")
                {
                    unitsList = dr["ObjectGUID"].ToString();
                }
                else
                {
                    unitsList += "," + dr["ObjectGUID"].ToString();
                }
            }
            return unitsList;
        }

        public static BaseEnums.URLStatus createOrUpdateURL(string objectGuid, string URL, string OwnerName, string type, string ObjectName, string rentalUnitId, string isInitialCall, string version)
        {
            string sqlQueryRead = TextureChanger.Constants.QueryReadFilterObjectId;
            string sqlQueryInsert = TextureChanger.Constants.QueryInsert;
            string sqlQueryUpdate = TextureChanger.Constants.QueryUpdate;

            BaseEnums.URLStatus status;

            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + HttpContext.Current.Server.MapPath("~/App_Data/" + TextureChanger.Variables.DatabaseName)))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();

                    com.CommandText = String.Format(sqlQueryRead, objectGuid);

                    using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (isInitialCall != "yes")
                            {
                                if (reader["URL"].ToString() == URL &&
                                   reader["Type"].ToString() == type &&
                                   reader["Name"].ToString() == ObjectName &&
                                   reader["Owner"].ToString() == OwnerName &&
                                   reader["LinkedRentalUnitId"].ToString() == rentalUnitId)
                                {
                                    status = BaseEnums.URLStatus.AlreadyUpdate;
                                }
                                else
                                {
                                    reader.Close();
                                    com.CommandText = String.Format(sqlQueryUpdate, URL, ObjectName, type, OwnerName, rentalUnitId, objectGuid, version);
                                    com.ExecuteNonQuery();
                                    status = BaseEnums.URLStatus.Updated;
                                }
                            }
                            else
                            {
                                if (reader["URL"].ToString() == URL &&
                                    reader["Type"].ToString() == type &&
                                reader["Name"].ToString() == ObjectName &&
                                reader["Owner"].ToString() == OwnerName)
                                {
                                    status = BaseEnums.URLStatus.AlreadyUpdate;
                                }
                                else
                                {
                                    reader.Close();
                                    com.CommandText = String.Format(TextureChanger.Constants.QueryUpdateNoLinkedUnit, URL, ObjectName, type, OwnerName, objectGuid, version);

                                    com.ExecuteNonQuery();
                                    status = BaseEnums.URLStatus.Updated;
                                }

                            }

                        }
                        else
                        {
                            reader.Close();
                            com.CommandText = String.Format(sqlQueryInsert, objectGuid, URL, ObjectName, type, OwnerName, rentalUnitId, version);
                            com.ExecuteNonQuery();
                            status = BaseEnums.URLStatus.Created;
                        }
                    }
                    con.Close();
                }
            }
            return status;
        }
        
        public static string getLinkedRentalUnitGUID(string objectGuid)
        {
            string sqlQueryRead = TextureChanger.Constants.QueryReadFilterObjectId;
            string retValue = string.Empty;
            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + HttpContext.Current.Server.MapPath("~/App_Data/" + TextureChanger.Variables.DatabaseName)))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();

                    com.CommandText = String.Format(sqlQueryRead, objectGuid);

                    using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            retValue = reader["LinkedRentalUnitId"].ToString();

                        }
                    }
                }
            }

            if (retValue == string.Empty)
                retValue = "GUIDMISSING";

            return retValue;
        }

        public static BaseEnums.URLStatus addRemoveLinkedUnit(string _sourceGUID, string _childGUID, string _theme, string _objectName, string _objectType, string _clear)
        {
            string sqlQueryRead = @"select * from InWorldLinkedObjects where ObjectGuid = '{0}' 
                and LinkedObjectGUID = '{1}'";
            string sqlQueryInsert = @"insert into InWorldLinkedObjects(ObjectGuid,LinkedObjectGUID, TextureName, ObjectName, ObjectType) 
                                        values('{0}','{1}','{2}','{3}','{4}')";
            string sqlQueryUpdate = @"update InWorldLinkedObjects set TextureName='{0}',ObjectName='{3}', ObjectType='{4}'  where ObjectGuid = '{1}' 
                and LinkedObjectGUID = '{2}'";

            string sqlClearLinkedUnits = "delete * from InWorldLinkedObjects where ObjectGuid = '{0}'";

            BaseEnums.URLStatus status;

            if (_clear == "true")
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + HttpContext.Current.Server.MapPath("~/App_Data/" + TextureChanger.Variables.DatabaseName)))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();

                        com.CommandText = String.Format(sqlClearLinkedUnits, _sourceGUID);
                    }
                }
                return BaseEnums.URLStatus.Deleted;
            }

            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + HttpContext.Current.Server.MapPath("~/App_Data/" + TextureChanger.Variables.DatabaseName)))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();

                    com.CommandText = String.Format(sqlQueryRead, _sourceGUID, _childGUID);

                    using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["defaulttheme"].ToString() == _theme &&
                                reader["ObjectName"].ToString() == _objectName &&
                                reader["ObjectType"].ToString() == _objectType)

                            {
                                status = BaseEnums.URLStatus.AlreadyUpdate;
                            }
                            else
                            {
                                reader.Close();
                                com.CommandText = String.Format(sqlQueryUpdate, _theme, _sourceGUID, _childGUID, _objectName, _objectType);
                                com.ExecuteNonQuery();
                                status = BaseEnums.URLStatus.Updated;
                            }
                        }
                        else
                        {
                            reader.Close();
                            com.CommandText = String.Format(sqlQueryInsert, _sourceGUID, _childGUID, _theme, _objectName, _objectType);

                            com.ExecuteNonQuery();
                            status = BaseEnums.URLStatus.Created;
                        }

                    }
                }
            }
            return status;
        }

        public static string ListLinkedUnits(string _sourceGUID)
        {
            string sqlQueryRead = @"select * from InWorldLinkedObjects where ObjectGuid = '{0}'";

            string response = string.Empty;
 
            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + HttpContext.Current.Server.MapPath("~/App_Data/" + TextureChanger.Variables.DatabaseName)))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();

                    com.CommandText = String.Format(sqlQueryRead, _sourceGUID);

                    using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if(response != string.Empty)
                            {
                                response += "\n";
                            }

                            response += "Object Name:" + reader["ObjectName"].ToString() + 
                                "\nObject Type:" + reader["ObjectType"].ToString() +
                                "\nLinked Object GUID:" + reader["LinkedObjectGUID"].ToString() +
                                "\nDefault Texture/ Default scene:" + reader["TextureName"].ToString();
                        }
                    }
                }
            }
            return response;
        }

        public static Boolean setDefaultTheme(System.Web.SessionState.HttpSessionState _sessionState, string _objectGuid)
        {

            DataTable table = DataBaseStaging.ReadData(_sessionState);

            foreach (DataRow dr in table.Rows)
            {
                if(dr["ObjectGUID"].ToString() == _objectGuid)
                {
                    DataTable linkedUnits = null;

                    UnitDetails details = HTTPLogic.getAllDetails(dr["URL"].ToString());

                    if (details.rented == "Not Rented") // not rented
                    {
                        
                        if (dr["DefTheme"] != null && dr["DefTheme"].ToString() != string.Empty)
                        {
                            HTTPLogic.setTheme(dr["DefTheme"].ToString(), dr["URL"].ToString());
                        }

                        // reset the nearby objects
                        linkedUnits = DataBaseStaging.readLinkedData(dr[0].ToString());
                        foreach (DataRow dr1 in linkedUnits.Rows)
                        {
                            if (dr1["ObjectType"].ToString() == "Scenery")
                            {
                                HTTPLogic.setNearbyScenery(dr1["LinkedObjectGUID"].ToString(), dr["URL"].ToString(), dr1["TextureName"].ToString());
                            }
                            else if (dr1["ObjectType"].ToString() == "Multiscene")
                            {
                                HTTPLogic.setNearbyMultiScene(dr1["LinkedObjectGUID"].ToString(), dr["URL"].ToString(), dr1["TextureName"].ToString());
                            }
                        }

                    }
                    break;
                }
            }

            return true;
        }

        /*public static void doTask(DataRow dr, string _themeName, string path)
        {
            DataTable linkedUnits = null;

            UnitDetails details = HTTPLogic.getAllDetails(dr["URL"].ToString());
            if (details.rented == "Not Rented") // not rented
            {
                if (_themeName == "")
                {
                    if (dr["DefTheme"] != null)
                        _themeName = dr["DefTheme"].ToString();
                }

                if (_themeName != "")
                    HTTPLogic.setTheme(_themeName, dr["URL"].ToString());

                // reset the nearby objects
                linkedUnits = DataBaseStaging.readLinkedData(dr[0].ToString(), path);
                foreach (DataRow dr1 in linkedUnits.Rows)
                {
                    HTTPLogic.setNearbyTheme(dr1[1].ToString(), dr["URL"].ToString(), dr1[2].ToString());
                }

            }
        }*/
    }
}

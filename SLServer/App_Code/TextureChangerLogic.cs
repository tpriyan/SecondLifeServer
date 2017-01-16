using System;
using System.Web;
using TextureChanger.BaseTypes;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TextureChanger.Logic
{
    public class GridOperations
    {

        public GridOperations()
        {

        }

        private static DataTable DataTableLinkedUnits()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("ObjectGUID");
            dt.Columns.Add("LinkedObjectGUID");
            dt.Columns.Add("TextureName");

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

            return dt;
        }

        public static void LoadData(System.Web.UI.WebControls.GridView gridView, System.Web.SessionState.HttpSessionState _sessionState)
        {
            {
                gridView.DataSource = ReadData(_sessionState);
                gridView.DataBind();
            }
        }

        public static DataTable ReadData(System.Web.SessionState.HttpSessionState _sessionState)
        {
            string sqlQueryRead = TextureChanger.Constants.QueryReadAll;
            System.Data.DataTable dataTable = DataTableUnits();
            System.Data.DataRow dataRow = null;

            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + HttpContext.Current.Server.MapPath("~/App_Data/" + TextureChanger.Variables.DatabaseName)))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();
                    com.CommandText = String.Format(sqlQueryRead, _sessionState["ownerid"].ToString());
                    using (System.Data.SQLite.SQLiteDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dataRow = dataTable.NewRow();
                            dataRow[0] = reader.GetValue(0);
                            dataRow[1] = reader.GetValue(1);
                            dataRow[2] = reader.GetValue(2);
                            dataRow[3] = reader.GetValue(3);

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

            string sqlQueryRead = "select * from DefTextureLinkedObjects where ObjectGuid = '{0}'";
            string path = "";

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
        public static async Task<Boolean> bulkSetThemeUnrentedAsync(string _themeName, System.Web.SessionState.HttpSessionState _sessionState)
        {
            DataTable table = GridOperations.ReadData(_sessionState);
            
            List<Task> TaskList = new List<Task>();

            //  TaskList.Add(LastTask);
            string path = HttpContext.Current.Server.MapPath("~/App_Data/");

           //ask.WaitAll(TaskList.ToArray());

            foreach (DataRow dr in table.Rows)
            {
                var LastTask = Task.Run(()=> doTask(dr, _themeName, path));
                TaskList.Add(LastTask);
            }

            Task.WaitAll(TaskList.ToArray());

            return true;
        }
        public static void doTask(DataRow dr, string _themeName, string path)
        {
            DataTable linkedUnits = null;

            if (HTTPLogic.isRented(dr["URL"].ToString()) == 1) // not rented
            {
                HTTPLogic.setTheme(_themeName, dr["URL"].ToString());

                // reset the nearby objects
                linkedUnits = GridOperations.readLinkedData(dr[0].ToString(), path);
                foreach (DataRow dr1 in linkedUnits.Rows)
                {
                    HTTPLogic.setNearbyTheme(dr1[1].ToString(), dr["URL"].ToString(), dr1[2].ToString());
                }

            }
        }

        public static void bulkSetThemeUnrented(string _themeName, System.Web.SessionState.HttpSessionState _sessionState)
        {
            DataTable table = GridOperations.ReadData(_sessionState);
            DataTable linkedUnits = null;

            foreach (DataRow dr in table.Rows)
            {
                if (HTTPLogic.isRented(dr["URL"].ToString()) == 1) // not rented
                {
                    HTTPLogic.setTheme(_themeName, dr["URL"].ToString());

                    // reset the nearby objects
                    linkedUnits = GridOperations.readLinkedData(dr[0].ToString());
                    foreach (DataRow dr1 in linkedUnits.Rows)
                    {
                        HTTPLogic.setNearbyTheme(dr1[1].ToString(), dr["URL"].ToString(), dr1[2].ToString());
                    }

                }
            }

        }
    }

    public class CRUDOperations
    {
        public static BaseEnums.URLStatus createOrUpdateURL(string objectGuid, string URL, string OwnerName, string type, string ObjectName, string rentalUnitId, string isInitialCall)
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
                                    com.CommandText = String.Format(sqlQueryUpdate, URL, ObjectName, type, OwnerName, rentalUnitId, objectGuid);
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
                                    com.CommandText = String.Format(TextureChanger.Constants.QueryUpdateNoLinkedUnit, URL, ObjectName, type, OwnerName, objectGuid);

                                    com.ExecuteNonQuery();
                                    status = BaseEnums.URLStatus.Updated;
                                }

                            }

                        }
                        else
                        {
                            reader.Close();
                            com.CommandText = String.Format(sqlQueryInsert, objectGuid, URL, ObjectName, type, OwnerName, rentalUnitId);
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

        public static BaseEnums.URLStatus addLinkedUnit(string _sourceGUID, string _childGUID, string _theme)
        {
            string sqlQueryRead = @"select * from DefTextureLinkedObjects where ObjectGuid = '{0}' 
                and LinkedObjectGUID = '{1}'";
            string sqlQueryInsert = @"insert into DefTextureLinkedObjects(ObjectGuid,LinkedObjectGUID, TextureName) 
                                        values('{0}','{1}','{2}')";
            string sqlQueryUpdate = @"update DefTextureLinkedObjects set TextureName='{0}' where ObjectGuid = '{1}' 
                and LinkedObjectGUID = '{2}'";

            BaseEnums.URLStatus status;

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
                            if (reader["defaulttheme"].ToString() == _theme)

                            {
                                status = BaseEnums.URLStatus.AlreadyUpdate;
                            }
                            else
                            {
                                reader.Close();
                                com.CommandText = String.Format(sqlQueryUpdate, _theme, _sourceGUID, _childGUID);
                                com.ExecuteNonQuery();
                                status = BaseEnums.URLStatus.Updated;
                            }
                        }
                        else
                        {


                            reader.Close();
                            com.CommandText = String.Format(sqlQueryInsert, _sourceGUID, _childGUID, _theme);

                            com.ExecuteNonQuery();
                            status = BaseEnums.URLStatus.Created;
                        }

                    }



                }
            }



            return status;
        }
    }
}

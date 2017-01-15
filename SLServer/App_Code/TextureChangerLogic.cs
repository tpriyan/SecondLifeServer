using System;
using System.Web;
using TextureChanger.BaseTypes;
using System.Data;

namespace TextureChanger.Logic
{
    public class GridOperations
    {

        public GridOperations()
        {

        }

        public static DataTable DataTable()
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
            System.Data.DataTable dataTable = DataTable();
            System.Data.DataRow dataRow = null;

            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + HttpContext.Current.Server.MapPath("~/App_Data/" + TextureChanger.Variables.DatabaseName)))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();
                    com.CommandText = String.Format(sqlQueryRead, _sessionState["owner"].ToString()) ;
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
    }

    public class BulkOperations
    {
        public static void bulkSetThemeUnrented(string _themeName, System.Web.SessionState.HttpSessionState _sessionState)
        {
            DataTable table = GridOperations.ReadData(_sessionState);

            foreach (DataRow dr in table.Rows)
            {
                if (HTTPLogic.isRented(dr["URL"].ToString()) == 1) // not rented
                {
                    HTTPLogic.setTheme(_themeName, dr["URL"].ToString());
                }
            }
        }
    }

    public class CRUDOperations
    {
        public BaseEnums.URLStatus createOrUpdateURL(string objectGuid, string URL, string OwnerName, string type, string ObjectName, string rentalUnitId, string isInitialCall)
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


        public static string getLinkedGUID(string objectGuid)
        {
            string sqlQueryRead = TextureChanger.Constants.QueryReadFilterObjectId;

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
                            return reader["LinkedRentalUnitId"].ToString();

                        }
                    }
                }
            }

            return "";
        }
    }
}
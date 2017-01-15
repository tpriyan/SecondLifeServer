using System;
using System.Web;
using TextureChanger.BaseTypes;

namespace TextureChanger.Logic
{
    public class GridOperations
    {
        
        public GridOperations()
        {

        }

        public System.Data.DataTable DataTable()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("ObjectGUID");
            dt.Columns.Add("URL");
            dt.Columns.Add("Name");
            dt.Columns.Add("Type");
            dt.Columns.Add("Owner");

            return dt;
        }

        public void LoadData(System.Web.UI.WebControls.GridView gridView)
        {
            string sqlQueryRead = TextureChanger.Constants.QueryReadAll;
            System.Data.DataTable dataTable = this.DataTable();
            System.Data.DataRow dataRow = null;

            using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + HttpContext.Current.Server.MapPath("~/App_Data/" + TextureChanger.Variables.DatabaseName)))
            {
                using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                {
                    con.Open();
                    com.CommandText = sqlQueryRead;
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

            gridView.DataSource = dataTable;
            gridView.DataBind();

        }


    }
    public class CRUDOperations
    {
        public BaseEnums.URLStatus createOrUpdateURL(string objectGuid, string URL, string OwnerName, string type, string ObjectName)
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
                                com.CommandText = String.Format(sqlQueryUpdate, URL, ObjectName, type, OwnerName, objectGuid);
                                com.ExecuteNonQuery();
                                status = BaseEnums.URLStatus.Updated;
                            }

                        }
                        else
                        {
                            reader.Close();
                            com.CommandText = String.Format(sqlQueryInsert, objectGuid, URL, ObjectName, type, OwnerName);
                            com.ExecuteNonQuery();
                            status = BaseEnums.URLStatus.Created;
                        }
                    }
                    con.Close();
                }
            }
            return status;
        }
    }
}
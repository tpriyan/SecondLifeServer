using System;

public partial class CreateOrUpdate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params["slurl"] != null 
            && Request.Params["objectguid"] != null 
            && Request.Params["type"] != null
            && Request.Params["owner"] != null
             && Request.Params["IsInitialCall"] != null
            && Request.Params["LinkedRentalUnitId"] != null)
        {
            try
            {
                TextureChanger.Logic.CRUDOperations.createOrUpdateURL(Request.Params["objectguid"].ToString(),
                                         Request.Params["slurl"].ToString(),
                                         Request.Params["owner"].ToString(),
                                         Request.Params["type"].ToString(),
                                         Request.Params["name"].ToString(),
                                        Request.Params["LinkedRentalUnitId"].ToString(),
                                        Request.Params["isinitialcall"].ToString());


                Response.Write("OK");
                Response.Flush();
                Response.SuppressContent = true;
            }
            catch
            {
                Response.Write("NOTOK");
                Response.Flush();
                Response.SuppressContent = true;
            }

            

            
        }
    }
}
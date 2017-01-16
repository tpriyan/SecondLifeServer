using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TextureChangerAddChild : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params["objectguid"] != null
            && Request.Params["childobject"] != null
            && Request.Params["defaulttheme"] != null)
             
        {
            try
            {
                TextureChanger.Logic.CRUDOperations.addLinkedUnit(Request.Params["objectguid"].ToString(),
                                Request.Params["childobject"].ToString(),
                                Request.Params["defaulttheme"].ToString());

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
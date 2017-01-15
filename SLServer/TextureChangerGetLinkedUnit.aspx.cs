using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TextureChangerGetLinkedUnit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if 
             (Request.Params["objectguid"] != null)
            
           
        {
            Response.Write(TextureChanger.Logic.CRUDOperations.getLinkedGUID(Request.Params["objectguid"].ToString()));
            Response.Flush();
            Response.SuppressContent = true;
            
        }
    }
}
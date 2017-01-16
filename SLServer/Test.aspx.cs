using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Session["ownerid"] = "72a4670a-9160-40c9-a3bc-3f0d28b58340";
        Response.Write(DateTime.Now.ToString());
        TextureChanger.Logic.BulkOperations.bulkSetThemeUnrented("Vintage", Session);
        Response.Write(DateTime.Now.ToString());
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        Session["ownerid"] = "72a4670a-9160-40c9-a3bc-3f0d28b58340";
        Response.Write(DateTime.Now.ToString());
        TextureChanger.Logic.BulkOperations.bulkSetThemeUnrentedAsync("Vintage", Session);
        Response.Write(DateTime.Now.ToString());
    }
}
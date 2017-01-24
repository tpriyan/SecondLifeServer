using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Testing_TestGetAllDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        TextureChanger.HTTPLogic.getAllDetails("http://sim10527.agni.lindenlab.com:12046/cap/874aad00-69a3-fd4e-26b9-571ee7e6032f");
    }
}
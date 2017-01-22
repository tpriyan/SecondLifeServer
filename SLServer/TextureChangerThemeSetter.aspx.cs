using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TextureChangerSetDefTheme : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!TextureChanger.SessionHandler.IsSessionValid(Session, Response))
            Response.Redirect("Login.aspx");
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        
        TextureChanger.Logic.BulkOperations.bulkSetThemeUnrentedAsync(Session, TextBox1.Text);
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("TextureChangerMain.aspx");
    }

    protected void BtnSignOut_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("Login.aspx");
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TextureChangerMain : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!TextureChanger.SessionHandler.IsSessionValid(Session, Response))
            Response.Redirect("Login.aspx");
    }

    protected void BtnSettings_Click(object sender, EventArgs e)
    {
        Response.Redirect("TextureChangerSettings.aspx");

    }

    protected void BtnViewUnits_Click(object sender, EventArgs e)
    {
        Response.Redirect("TextureChanger.aspx");
    }

    protected void BtnSetDefaultTheme_Click(object sender, EventArgs e)
    {
        TextureChanger.Logic.BulkOperations.bulkSetThemeUnrented("Vintage", Session);
    }

    protected void BtnSignOut_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("Login.aspx");
    }
}
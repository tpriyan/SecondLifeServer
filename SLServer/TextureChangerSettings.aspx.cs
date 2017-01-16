using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TextureChangerSettings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!TextureChanger.SessionHandler.IsSessionValid(Session, Response))
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            loadSettings();
        }
        
    }

    public void loadSettings()
    {
        TextureChanger.GlobalSettings s = TextureChanger.Settings.getSettings();

        skipFetchCurrentTheme.Checked = s.skipFetchCurrentTheme;
        skipFetchThemeDataForRentedBoxes.Checked = s.skipFetchThemeDataForRentedBoxes;
        skipSkyboxThemesFetch.Checked = s.skipSkyboxThemesFetch;
        themes.Text = s.themes;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        TextureChanger.Settings.setSettings(skipFetchCurrentTheme.Checked,
            skipFetchThemeDataForRentedBoxes.Checked,
            skipSkyboxThemesFetch.Checked, themes.Text);
        loadSettings();
        Response.Redirect("TextureChangerMain.aspx");
    }
}
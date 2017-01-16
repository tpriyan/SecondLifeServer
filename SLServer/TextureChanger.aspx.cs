using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;

public partial class _Default : System.Web.UI.Page
{
    TextureChanger.GlobalSettings s;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!TextureChanger.SessionHandler.IsSessionValid(Session, Response))
            Response.Redirect("Login.aspx");

        //if (!IsPostBack)
        {
            

            //s = TextureChanger.Settings.getSettings();
            TextureChanger.Logic.DataBaseStaging.LoadData(GridView1, Session);
        }
       // else
        {
            //GridView1.DataBind();
        }

        

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            PlaceHolder ph3 = (PlaceHolder)e.Row.FindControl("PlaceHolder3");
            Label l1 = (Label)e.Row.FindControl("Label102");

            string[] tmp = l1.Text.ToString().Split(',');


            l1.Text = "";


            for (int i = 0; i < tmp.Length; i++)
            {
                Button b1 = new Button();
                b1.Text = tmp[i];
                b1.Click += new EventHandler(this.TextureChange_Click);
                ph3.Controls.Add(b1);
            }
            if (tmp.Length == 0)
            {
                Label l = new Label();
                l.Text = "No themes";
                ph3.Controls.Add(l);
            }
        }

     
    }


    protected void TextureChange_Click(object sender, EventArgs e)
    {
        //Get the button that raised the event
        Button btn = (Button)sender;

        //Get the row that contains this button
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;

        HiddenField field = (HiddenField)gvr.FindControl("IdHiddenURL");

        string url = field.Value.ToString();

        TextureChanger.HTTPLogic.setTheme(btn.Text, url);
        TextureChanger.Logic.DataBaseStaging.LoadData(GridView1, Session);

    }




    protected void BtnSetDefaultTheme_Click(object sender, EventArgs e)
    {
       // TextureChanger.Logic.BulkOperations.bulkSetThemeUnrentedAsync("Vintage", Session);
      //  TextureChanger.Logic.DataBaseStaging.LoadData(GridView1, Session);
    }

    protected void BtnReloadData_Click(object sender, EventArgs e)
    {
        TextureChanger.Logic.DataBaseStaging.LoadData(GridView1, Session);
    }

    protected void BtnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("TextureChangerMain.aspx");
    }

    protected void BtnSignOut_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("Login.aspx");
    }
}
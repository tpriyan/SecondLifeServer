using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        TextureChanger.Logic.GridOperations gridOperations = new TextureChanger.Logic.GridOperations();

        gridOperations.LoadData(GridView1);

        if (!IsPostBack)
        {
        }
        
    }
   
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string url = string.Empty;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr =  (DataRowView) e.Row.DataItem;

            DataRow dataRow = dr.Row;

            url = dataRow[1].ToString();

            if (e.Row.RowState != DataControlRowState.Edit)
            {
                PlaceHolder ph3 = (PlaceHolder)e.Row.FindControl("PlaceHolder3");
                string[] tmp = TextureChanger.HTTPLogic.getAllThemes(url);
                for(int i=0;i< tmp.Length; i++)
                {
                    Button b1 = new Button();
                    b1.Text = tmp[i];
                    b1.Click += new EventHandler(this.TextureChange_Click);
                    ph3.Controls.Add(b1);
                }
                if(tmp.Length == 0)
                {
                    Label l = new Label();
                    l.Text = "No themes";
                    ph3.Controls.Add(l);
                }

                PlaceHolder ph1 = (PlaceHolder)e.Row.FindControl("PlaceHolder1");

                Label l2 = new Label();
                l2.Text = TextureChanger.HTTPLogic.getCurrentTexture(url);
                ph1.Controls.Add(l2);

                PlaceHolder ph2 = (PlaceHolder)e.Row.FindControl("PlaceHolder2");

                Label l1 = new Label();
                switch(TextureChanger.HTTPLogic.isRented(url))
                {
                    case 0:
                        l1.Text = "Not linked";
                        break;
                    case 1:
                        l1.Text = "Not Rented";
                        break;
                    case 2:
                        l1.Text = "Rented";
                        break;
                    default:
                        l1.Text = "Error";
                        break;

                }
                ph2.Controls.Add(l1);

            }
        }
    }

    protected void TextureChange_Click(object sender, EventArgs e)
    {
        //Get the button that raised the event
        Button btn = (Button)sender;

        //Get the row that contains this button
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;

        HiddenField field = (HiddenField) gvr.FindControl("IdHiddenURL");

        string url = field.Value.ToString();

        TextureChanger.HTTPLogic.setTheme(btn.Text, url);
        GridView1.DataBind();

    }

    

}
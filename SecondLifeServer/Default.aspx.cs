using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using BusinessLogic;
using System.Data;

using System.Collections.Specialized;
using System.Net;

public partial class _Default : System.Web.UI.Page
{
    string url = "http://sim10543.agni.lindenlab.com:12046/cap/b8ee1548-f25d-1c9c-f76c-1955b973bdf3";
    protected void Page_Load(object sender, EventArgs e)
    {
        TextureChanger.Logic.GridOperations gridOperations = new TextureChanger.Logic.GridOperations();

        gridOperations.LoadData(GridView1);


        if (!IsPostBack)
        {
        }
        
    }

    private void AddLinkButton()
    {
        foreach (GridViewRow row in GridView1.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lb = new LinkButton();
                lb.Text = "Approve";
                lb.CommandName = "ApproveVacation";
                lb.Command += LinkButton_Command;
                row.Cells[2].Controls.Add(lb);
            }
        }
    }

    protected void LinkButton_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "ApproveVacation")
        {
            LinkButton lb = (LinkButton)sender;
            lb.Text = "OK";
        }
    }

    protected void gdvCustomer_DataBound(object sender, EventArgs e)
    {
        
    }

    protected void GridView1_DataBound(object sender, EventArgs e)
    {
      
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Boolean rented  = true;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dr =  (DataRowView) e.Row.DataItem;

            DataRow dataRow = dr.Row;

            url = dataRow[1].ToString();

            if (e.Row.RowState != DataControlRowState.Edit)
            {
               

                PlaceHolder ph3 = (PlaceHolder)e.Row.FindControl("PlaceHolder3");
                string[] tmp = this.getAllThemes();
                for(int i=0;i< tmp.Length; i++)
                {
                    Button b1 = new Button();
                    b1.Text = tmp[i];
                    b1.Click += new EventHandler(this.TextureChange_Click);
                    ph3.Controls.Add(b1);
                }

                PlaceHolder ph1 = (PlaceHolder)e.Row.FindControl("PlaceHolder1");

                Label l2 = new Label();
                l2.Text = this.getCurrentTexture();
                ph1.Controls.Add(l2);

                PlaceHolder ph2 = (PlaceHolder)e.Row.FindControl("PlaceHolder2");

                Label l1 = new Label();
                switch(this.isRented())
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
                }
                ph2.Controls.Add(l1);

            }
        }
    }

    protected void TextureChange_Click(object sender, EventArgs e)
    {
        Button b = (Button)sender;

        this.setTheme(b.Text);
        GridView1.DataBind();

    }

    public string getCurrentTexture()
    {
        var response = Http.Post(url, new NameValueCollection() {
            { "action", "getcurrenttheme" },
            { "texturename", "Forest" }
        });

        string x = System.Text.Encoding.UTF8.GetString(response);

        return x;
    }

    public string[] getAllThemes()
    {
        var response = Http.Post(url, new NameValueCollection() {
            { "action", "getallthemes" },
            { "texturename", "Forest" }
        });

        string x = System.Text.Encoding.UTF8.GetString(response);

        return x.Split(',');
    }

    public int isRented()
    {
        int rented = 2;

        var response = Http.Post(url, new NameValueCollection() {
            { "action", "isrented" },
            { "texturename", "Forest" }
        });

        string x = System.Text.Encoding.UTF8.GetString(response);

        if (x.ToLower() == "no unit linked" || x.ToLower() == string.Empty)
        {
            rented = 0;
        }
        else
        {
            string[] split = x.Split('|');

            if (Decimal.Parse(split[1].ToString()) == 0)
            {
                rented = 1;
            }
        }

        return rented;
    }

    public string setTheme(string themeName)
    {
        var response = Http.Post(url, new NameValueCollection() {
            { "action", "setcurrenttheme" },
            { "texturename",  themeName}
        });

        string x = System.Text.Encoding.UTF8.GetString(response);

        return x;
    }

}
public class Http
{
    public static byte[] Post(string uri, NameValueCollection pairs)
    {
        byte[] response = null;
        using (WebClient client = new WebClient())
        {
            response = client.UploadValues(uri, pairs);
        }
        return response;
    }

    

}
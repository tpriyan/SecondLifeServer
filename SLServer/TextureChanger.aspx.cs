using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!TextureChanger.SessionHandler.IsSessionValid(Session, Response))
            Response.Redirect("Login.aspx");

        if (!IsPostBack)
        {
            loadData();
        }
        else
        {
            if(ViewState["Table"] != null)
            {
                GridView1.DataSource = (DataTable) ViewState["Table"];
                GridView1.DataBind();
            }
        }
    }


    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
            //gvOrders.DataBind();
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
                if (tmp[i] == string.Empty)
                    continue;
                Button b1 = new Button();
                b1.Text = tmp[i];
                b1.Click += new EventHandler(this.TextureChange_Click);
                ph3.Controls.Add(b1);
            }

            Button b12 = new Button();
            b12.Text = "Set defaults";

            b12.Click += new EventHandler(this.TextureChange_Click);
            ph3.Controls.Add(b12);

            if (tmp.Length == 0)
            {
                Label l = new Label();
                l.Text = "No themes";
                ph3.Controls.Add(l);
            }

            GridViewRow gvr = e.Row;
            HiddenField field = (HiddenField)gvr.FindControl("IdHiddenFieldGuid");

            string objectGuid = field.Value.ToString();

            GridView gvOrders = e.Row.FindControl("gvOrders") as GridView;


            gvOrders.DataSource = TextureChanger.Logic.DataBaseStaging.readLinkedData(objectGuid);
            gvOrders.DataBind();
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

        if(btn.Text == "Set defaults")
        {
            HiddenField fldGuid = (HiddenField)gvr.FindControl("IdHiddenFieldGuid");
            TextureChanger.Logic.CRUDOperations.setDefaultTheme(Session, fldGuid.Value);
        }
        else
        { 
            TextureChanger.HTTPLogic.setTheme(btn.Text, url);
        }
        loadData();
    }

    protected void BtnReloadData_Click(object sender, EventArgs e)
    {
        loadData();
    }

    protected void BtnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("TextureChangerHome.aspx");
    }

    protected void BtnSignOut_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("Login.aspx");
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.DataSource = (DataTable) ViewState["Table"];
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
    }

    private void loadData()
    {
        DataTable dt = TextureChanger.Logic.DataBaseStaging.GetUnitsDataTable(Session);
        ViewState["Table"] = dt;
        if (this.ViewState["SortExpression"] != null)
        {
            dt.DefaultView.Sort = string.Format("{0} {1}", ViewState["SortExpression"].ToString(), this.ViewState["SortOrder"].ToString());
        }
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        
        if (e.CommandName.Equals("Sort"))
        {
            if (ViewState["SortExpression"] != null)
            {
                if (this.ViewState["SortExpression"].ToString() == e.CommandArgument.ToString())
                {
                    if (ViewState["SortOrder"].ToString() == "ASC")
                        ViewState["SortOrder"] = "DESC";
                    else
                        ViewState["SortOrder"] = "ASC";
                }
                else
                {
                    ViewState["SortOrder"] = "ASC";
                    ViewState["SortExpression"] = e.CommandArgument.ToString();
                }

            }
            else
            {
                ViewState["SortExpression"] = e.CommandArgument.ToString();
                ViewState["SortOrder"] = "ASC";
            }
        }
        //Re Bind The Grid to reflect the Sort Order
        loadData();
    
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (ViewState["SortExpression"] == null)
            return;

        if (e.Row != null && e.Row.RowType == DataControlRowType.Header)
        {
            foreach (TableCell cell in e.Row.Cells)
            {
                if (cell.HasControls())
                {
                    if (cell.Controls[1] is LinkButton)
                    {
                        LinkButton btn = (LinkButton)cell.Controls[1];
                        Image img = new Image();
                        img.Width = 10;
                        img.Height = 10;
                        img.ImageUrl = "";
                        if (ViewState["SortExpression"].ToString() == btn.CommandArgument)
                        {
                            if (ViewState["SortOrder"].ToString() == "ASC")
                            {
                                img.ImageUrl = "images/view_sort_ascending.png";
                            }
                            else
                            {
                                img.ImageUrl = "images/view_sort_descending.png";
                            }
                            cell.Controls.Add(img);
                        }
                    }
                }
            }
        }
    }

    protected void Lnk_LogOut_Click(object sender, EventArgs e)
    {
        TextureChanger.SessionHandler.logOut(Session, Response);
    }





    protected void btnApplyPageSize_Click(object sender, EventArgs e)
    {
        if (DropDownList1.SelectedValue == "All")
        {
            GridView1.AllowPaging = false;
            GridView1.DataSource = (DataTable)ViewState["Table"];
            GridView1.DataBind();
        }
        else
        {
            GridView1.AllowPaging = true;
            GridView1.PageSize =  Int32.Parse(DropDownList1.SelectedValue);
            GridView1.DataSource = (DataTable)ViewState["Table"];
            GridView1.DataBind();
        }
    }
}
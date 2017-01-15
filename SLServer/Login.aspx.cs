using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (TextureChanger.SessionHandler.IsSessionValid(Session, Response))
            Response.Redirect("TextureChangerMain.aspx");

    }



    protected void btnLogin_Click(object sender, EventArgs e)
    {
    //    if (TextureChanger.SessionHandler.login(TxtUserName.Value, TxtPassword.Value, Session))
     //       Response.Redirect("TextureChangerMain.aspx");
    //    else
          //  error.Text = "Invalid username or password";
    }

    protected void ValidateUser(object sender, EventArgs e)
    {
        if (TextureChanger.SessionHandler.login(Login1.UserName, Login1.Password, Session))
            Response.Redirect("TextureChangerMain.aspx");
        else
            Login1.FailureText = "Username or password incorrect!";
            
    }
}
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TextureChangerMain.aspx.cs" Inherits="TextureChangerMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <center> 
        <h1>Rental manager</h1>
        </center>
        <br />

    </div>
        <div align="center">
     
    
        <asp:Button ID="BtnViewUnits" runat="server" Text="View units" OnClick="BtnViewUnits_Click" Width="300px" />
        <br />
        <br />
        
        <asp:Button ID="BtnSetDefaultTheme" runat="server" Text="Bulk set themes" OnClick="BtnSetDefaultTheme_Click" Width="300px" />
        <br />
        <br />
        <asp:Button ID="BtnSignOut" runat="server" Text="Sign out" Width="300px" OnClick="BtnSignOut_Click" />
            <asp:Button ID="BtnSettings" runat="server" Text="Settings" OnClick="BtnSettings_Click" style="text-align: center" Width="300px" Visible ="false" />
        <br />
        <br />
            </div>
    </form>
</body>
</html>

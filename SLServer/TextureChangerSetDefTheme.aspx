<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TextureChangerSetDefTheme.aspx.cs" Inherits="TextureChangerSetDefTheme" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <h1> Set themes(Only non-rented units)</h1>
    </div>

        <div align="center">
            <asp:Label ID="Label1" runat="server" Text="Texture theme to be applied(Leave blank to apply texture in settings):" ></asp:Label>
            <asp:TextBox ID="TextBox1" runat="server" Width="337px"></asp:TextBox>

            <br />
            <br />
            <asp:Button ID="Button1" runat="server" Text="Apply theme" OnClick="Button1_Click" />

            <br />
            <br />
            <asp:Button ID="Button2" runat="server" Text="Home" OnClick="Button2_Click" />

            <br />
            <br />
            <asp:Button ID="BtnSignOut" runat="server" OnClick="BtnSignOut_Click" Text="Sign out" Width="300px" />

    </div>

    </form>
</body>
</html>

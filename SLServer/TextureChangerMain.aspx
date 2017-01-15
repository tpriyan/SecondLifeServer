<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TextureChangerMain.aspx.cs" Inherits="TextureChangerMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <center> Rental manager</center>
        <br />

    </div>
        <asp:Button ID="BtnViewUnits" runat="server" Text="View units" OnClick="BtnViewUnits_Click" style="text-align: center" />
        <br />
        <br />
        <asp:Button ID="BtnSettings" runat="server" Text="Settings" OnClick="BtnSettings_Click" style="text-align: center" />
        <br />
        <br />
        <asp:Button ID="BtnSetDefaultTheme" runat="server" Text="Set all not rented units to Vintage" OnClick="BtnSetDefaultTheme_Click" />
    </form>
</body>
</html>

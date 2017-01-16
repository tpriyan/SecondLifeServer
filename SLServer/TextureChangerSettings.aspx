﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TextureChangerSettings.aspx.cs" Inherits="TextureChangerSettings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    
        <h1>Settings</h1>
        </div>
    <div align ="center">
        <p>
            <asp:CheckBox ID="skipSkyboxThemesFetch" runat="server" Text="Skip fetching of skybox themes &amp; take data from local textbox below" />
        </p>
        <asp:CheckBox ID="skipFetchThemeDataForRentedBoxes" runat="server" Text="Skip fetching of theme data for rented skyboxes" />
        <p>
            <asp:CheckBox ID="skipFetchCurrentTheme" runat="server" Text="Skip fetch current theme(This need to be unchecked always)" />
        </p>
        <asp:Label ID="Label1" runat="server" Text="Themes(Separated by comma"></asp:Label>
        <asp:TextBox ID="themes" runat="server" Width="737px"></asp:TextBox>
        <p>
            <asp:Button ID="Button1" runat="server" Text="Save" OnClick="Button1_Click" />
        </p>
        </div>
    </form>
</body>
</html>

﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TextureChanger.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div align="center">
            <h1>Units List</h1>
        </div>
        <div align="center">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound" Width="80%" HorizontalAlign="Center" 
            HeaderStyle-Font-Bold="true"  >
            <Columns>
                <asp:TemplateField ItemStyle-Width ="25%">
                    <HeaderTemplate>
                        <asp:Label ID="Label5" runat="server" Text='Name'></asp:Label>
                        
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%#Bind("Name") %>'></asp:Label>
                        <asp:HiddenField ID="IdHiddenURL" runat="server" Value='<%#Bind("URL") %>'></asp:HiddenField>
                    </ItemTemplate>


                </asp:TemplateField>
                

                <asp:TemplateField ItemStyle-Width ="10%">
                    <HeaderTemplate>
                        <asp:Label ID="Label7" runat="server" Text='Type'></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label8" runat="server" Text='<%#Bind("Type") %>'></asp:Label>
                    </ItemTemplate>
                    
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width ="15%">
                    <HeaderTemplate>
                        <asp:Label ID="Label11" runat="server" Text='Current texture theme'></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                             
                        </asp:PlaceHolder>
                        <asp:Label ID="Label100" runat="server" Text='<%#Bind("CurrentTheme") %>'></asp:Label>
                    </ItemTemplate>
                     
                </asp:TemplateField>
                

                <asp:TemplateField ItemStyle-Width ="10%">
                    <HeaderTemplate>
                        <asp:Label ID="Label12" runat="server" Text='Is rented'></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
                        <asp:Label ID="Label101" runat="server" Text='<%#Bind("RentedStatus") %>'></asp:Label>
                    </ItemTemplate>
                    
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width ="40%">
                    <HeaderTemplate>
                        <asp:Label ID="Label13" runat="server" Text='Set Texture themes'></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:PlaceHolder ID="PlaceHolder3" runat="server">
                        </asp:PlaceHolder>
                        <asp:Label ID="Label102" runat="server" Text='<%#Bind("ThemesAvailable") %>'></asp:Label>
                    </ItemTemplate>
                    
                </asp:TemplateField>

            </Columns>

<HeaderStyle Font-Bold="True"></HeaderStyle>
        </asp:GridView>
            <br />
            <asp:Button ID="BtnReloadData" runat="server" Text="Reload data" OnClick="BtnReloadData_Click" />
        <br />
        <br />
            </div>
        <div align ="center">
      <center>  
          <asp:Button ID="BtnHome" runat="server" Text="Home" OnClick="BtnHome_Click" />
            <br />
          <br />
          <asp:Button ID="BtnSignOut" runat="server" OnClick="BtnSignOut_Click" Text="Sign out" Width="300px" />
            </center>
        </div>
    </form>
</body>
</html>
